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

        public DateTimeOffset SinceDate { get; set; }

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

        public Dictionary<string, ChangeInfo> GetChanges(Repository repo)
        {

            if (DateTime.Now < SinceDate)
            {
                Console.Error.WriteLine($"{nameof(SinceDate)} must be in the past.");
                return null;
            }

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
                    if (when < SinceDate)
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
