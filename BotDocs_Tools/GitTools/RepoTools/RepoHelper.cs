using GitTools.Git;
using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;

namespace RepoTools
{
    public class RepoHelper
    {
        public IEnumerable<string> AcceptableDirectories { get; set; }
        public IEnumerable<string> AcceptableExtensions { get; set; }

        public Repository GetRepository(string rootDir)
        {
            Contract.Requires(Directory.Exists(rootDir),
                $"Directory `{rootDir}` does not exist or is not a valid directory");

            Console.WriteLine($"Getting local repository at `{rootDir}`...");
            var repo = RepositoryLoader.GetRepo(rootDir);

            Console.WriteLine($"Repo info {repo.Info}.");

            if (!repo.Head.IsCurrentRepositoryHead)
            {
                Console.Error.WriteLine($"{repo.Head.FriendlyName} is not current branch head.");
                repo.Dispose();
                return null;
            }
            if (repo.Head.IsRemote)
            {
                Console.Error.WriteLine($"{repo.Head.FriendlyName} is remote?!");
                repo.Dispose();
                return null;
            }
            return repo;
        }

        private static readonly char[] PathSeparatorCharacters = new char[] { '/' };

        /// <summary>Returns the top-level directory, followed by the remainder of the path.</summary>
        /// <param name="path">The path to get the top-level directory from.</param>
        /// <returns>(head, tail), where head is the top-level directory, and the tail is the remainder of the path.
        /// If the path represents a file without any directory info, head will be null.</returns>
        private static (string, string) SplitPath(string path)
        {
            var i = path.IndexOfAny(PathSeparatorCharacters);
            if (i < 0)
            {
                return (null, path);
            }

            var bits = path.Split(PathSeparatorCharacters, 2);
            return (bits[0], bits[1]);
        }

        public static Blob FindFileInTree(Tree tree, string relPath)
        {
            Contract.Requires(tree != null);
            Contract.Requires(relPath != null);

            var parts = relPath.Split(PathSeparatorCharacters, StringSplitOptions.None);
            if (parts.Any(p => string.IsNullOrWhiteSpace(p)))
            {
                throw new ArgumentException($"Invalid path `{relPath}`.");
            }

            // Crawl the tree structure.
            var entry = tree.First(e => e.Path.Equals(parts[0], StringComparison.InvariantCultureIgnoreCase));
            var i = 1;
            while (i < parts.Length
                && entry != null
                && entry.TargetType != TreeEntryTargetType.Blob)
            {
                var subtree = entry.Target.Peel<Tree>();
                entry = subtree.First(e => e.Path.Equals(parts[i], StringComparison.InvariantCultureIgnoreCase));
                i++;
            }

            // Check for failure.
            if (i != parts.Length
                || entry is null
                || entry.TargetType != TreeEntryTargetType.Blob)
            {
                return null;
            }

            var blob = entry.Target.Peel<Blob>();
            return blob;
        }

        /// <summary>Find the most recent commit in the repo for a given file.</summary>
        /// <param name="repo">The repo to find the file in.</param>
        /// <param name="relPath">The path of the file, relative to the local root of the repo.</param>
        /// <returns>The tree entry of the file; or null if not found.</returns>
        public Commit LastCommitFor(Repository repo, string relPath)
        {
            Contract.Requires(repo != null);
            Contract.Requires(relPath != null);

            var blob1 = FindFileInTree(repo.Head.Tip.Tree, relPath);
            if (blob1 is null)
            {
                throw new ArgumentException($"Could not find file `{relPath}` in the repo.");
            }

            foreach(var commit in repo.Commits)
            {
                var blob2 = FindFileInTree(commit.Tree, relPath);
                if (blob2 != null)
                {
                    if (blob2.Id != blob1.Id)
                    {
                        throw new ApplicationException($"Found a commit for file `{relPath}`, but" +
                            $"the ID is a mismatch with the head.");
                    }
                    else { return commit; }
                }
            }

            throw new ApplicationException($"Failed to find a recent commit for file `{relPath}`.");
        }

        public Dictionary<string, ChangeInfo> GetChanges(Repository repo, DateTimeOffset sinceDate)
        {
            var branch = repo.Head;
            Console.WriteLine($"Branch {branch.FriendlyName}...");

            var changes = new Dictionary<string, ChangeInfo>();
            try
            {
                var e = branch.Commits.GetEnumerator();
                var count = 0;
                const int maxCount = 20;
                while (e.MoveNext() && count < maxCount)
                {
                    var when = e.Current.Author.When;
                    if (when < sinceDate)
                    {
                        Console.Error.WriteLine($"Stopping at commit {e.Current.Id} from {when}.");
                        break;
                    }

                    ReviewCommit(repo, e.Current, changes);
                    count++;
                }

                return changes;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(
                    $"{ex.GetType().FullName} thrown in LocalRepo.GetChanges: {ex.Message}");
                return null;
            }
        }

        private void ReviewCommit(
            Repository repo,
            Commit commit,
            Dictionary<string, ChangeInfo> changes)
        {
            DisplayCommitInfo(commit);

            // Commits are in date order from most recent back.
            foreach (var parent in commit.Parents)
            {
                if (commit.Parents.Count() is 0)
                {
                    Console.WriteLine($"Skipping non-merge commit {commit.Id} | {commit.MessageShort}");
                    continue;
                }

                foreach (var change in repo.Diff.Compare<TreeChanges>(parent.Tree, commit.Tree))
                {
                    if (IsIgnorable(change))
                    {
                        continue;
                    }

                    Console.WriteLine(" - {0} : {1}", change.Status, change.Path);
                    var id = change.Path;
                    if (changes.ContainsKey(id))
                    {
                        // Skip files already in the list. May need to revisit this assumption.
                        continue;
                    }

                    changes[id] = new ChangeInfo(commit, change);
                }
            }
        }

        private readonly List<ChangeKind> _changesOfInterest = new List<ChangeKind>
        {
            ChangeKind.Added,
            ChangeKind.Deleted,
            ChangeKind.Modified,
            ChangeKind.Renamed,
        };

        private bool IsIgnorable(TreeEntryChanges change)
        {
            // Filter out changes we're not interested in:
            // - files outside monitored directories
            // - files without monitored extension
            // - change kinds that won't break code includes
            return !AcceptableDirectories.Any(
                        d => change.Path.StartsWith(d, StringComparison.InvariantCultureIgnoreCase))
                || !AcceptableExtensions.Any(
                        f => change.Path.EndsWith(f, StringComparison.InvariantCultureIgnoreCase))
                || !_changesOfInterest.Contains(change.Status);
        }

        private static void DisplayCommitInfo(Commit commit)
        {
            Console.WriteLine();
            Console.WriteLine($"commit {commit.Id}");
            if (commit.Parents.Count() > 1)
            {
                Console.WriteLine("Merge: {0}",
                    string.Join(" ", commit.Parents.Select(p => p.Id.Sha.Substring(0, 7)).ToArray()));
            }
            Console.WriteLine($"Author: {commit.Author.Name}");
            Console.WriteLine($"Date:   {commit.Author.When}");
            Console.WriteLine();
            Console.WriteLine(commit.Message);
        }
    }
}
