using LibGit2Sharp;
using RepoTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    internal class Program
    {
        private const string repoRoot = @"C:\Users\v-jofing\source\repos\bot-docs-pr";
        private const string filePath = @"articles\v4sdk\bot-builder-authentication.md";

        private static void Main(string[] args)
        {
            var helper = new RepoHelper();
            using (var repo = helper.GetRepository(repoRoot))
            {
                Console.WriteLine($"{repo.Commits.Count()} commits:");
                // 2 parents seems to imply a merge.
                //foreach(var commit in repo.Commits.Where(c=>c.Parents.Count()==1))
                //{
                //    Console.WriteLine($"- {commit.Author.Name} {commit.Author.When}, {commit.Parents.Count()} parents, {commit.MessageShort}");
                //}

                Console.WriteLine();

                var count = 10;
                Console.WriteLine($"Reviewing the last {count} commits:");
                var commits = repo.Commits.Where(c => c.Parents.Count() == 1).Take(count);
                foreach (var commit in commits)
                {
                    Console.WriteLine();
                    Console.WriteLine($"{commit.MessageShort}");
                    Console.WriteLine($"- {commit.Author.Name} {commit.Author.When}, {commit.Parents.Count()} parents");
                    //var diff = repo.Diff.Compare<TreeChanges>(new string[] { filePath });
                    var diff = repo.Diff.Compare<TreeChanges>(commit.Parents.First().Tree, commit.Tree);
                    Console.WriteLine($"  Added: {diff.Added.Count()}" +
                        $", Modified: {diff.Modified.Count()}" +
                        $", Deleted: {diff.Deleted.Count()}" +
                        $", Conflicted: {diff.Conflicted.Count()}" +
                        $"");
                    ListChanges(diff.Where(i => i.Status != ChangeKind.Unmodified));
                }

                //var commit = helper.LastCommitFor(repo, filePath);
                //Console.WriteLine($"{commit.Author.Name} {commit.Author.When} {commit.Id.Sha} {commit.Parents.Count()}");
                //Console.WriteLine($"{commit.MessageShort}");
                //Console.WriteLine();

                // This appears to be everything, not just changes.
                //var tree = commit.Tree;
                //WalkTree(tree, 0);

                //foreach(var parent in commit.Parents)
                //{
                //    // This appears to be everything at the previous point in time.
                //    Console.WriteLine($"{parent.Id.Sha} {parent.MessageShort} {parent.Parents.Count(),2} {parent.MessageShort}");
                //    WalkTree(parent.Tree, 2);
                //}
            }
        }

        private static void ListChanges(IEnumerable<TreeEntryChanges> changes)
        {
            foreach (var change in changes)
            {
                Console.WriteLine($"  - {change.Status} {change.Path}");
            }
        }

        private static void WalkTree(Tree tree, int indent)
        {
            foreach (var entry in tree)
            {
                var space = new string(' ', indent);
                Console.WriteLine($"{space}{entry.Name,-36} {entry.Path,-36} {entry.Mode,-18} {entry.TargetType}");
                if (entry.TargetType == TreeEntryTargetType.Tree)
                {
                    WalkTree(entry.Target.Peel<Tree>(), indent + 2);
                }
            }
        }
    }
}
