using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ReportUtils
{
    public class CodeLinkReport : BaseReport
    {
        public FolderBrowserDialog ChooseFolder { get; private set; }

        public string DocPath { get; set; }

        public CodeLinkReport(RichTextBox status, SaveFileDialog saveDialog) : base(status, saveDialog)
        {
        }
    }

    public class AkaLinkReport : BaseReport
    {
        private const string AkaLede1 = "http://aka.ms/";
        private const string AkaLede2 = "https://aka.ms/";
        private const string ArticlesRoot = "articles";

        public string DocPath { get; set; }

        private AkaLinkMap LinkMap { get; } = new AkaLinkMap();

        public AkaLinkReport(RichTextBox status, SaveFileDialog saveDialog)
            : base(status, saveDialog) { }


        public override bool Run()
        {
            Contract.Requires(DocPath != null);
            Contract.Requires(Directory.Exists(DocPath));
            Contract.Requires(Directory.Exists(Path.Combine(DocPath, ".git")));
            Contract.Requires(Directory.Exists(Path.Combine(DocPath, ArticlesRoot)));

            base.Run();

            LinkMap.Clear();
            var dir = Path.Combine(DocPath, ArticlesRoot);
            var files = Directory.GetFiles(dir, "*.md", SearchOption.AllDirectories);
            var links = new HashSet<string>();
            var linkCount = 0;
            foreach (var file in files)
            {
                FindLinksInFile(links, file);
                if (links.Count > 0)
                {
                    var relPath = file.Substring(DocPath.Length);
                    LinkMap.Add(relPath, links);
                    linkCount += links.Count;
                }
            }
            if (linkCount is 0)
            {
                Status.WriteLine(Severity.Warning, "No aka links found in this repo.");
                return false;
            }
            else
            {
                Status.WriteLine(Severity.Information, $"Found {linkCount} aka links: " +
                    $"{LinkMap.FileIndex.Count} unique files, {LinkMap.LinkIndex.Count} unique urls.");
                SaveDialog.Title = "Choose where to save the aka link report:";
                var result = SaveDialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    using (TextWriter writer = new StreamWriter(SaveDialog.FileName, false))
                    {
                        writer.WriteLine("In file,Aka link,Short URL");
                        foreach (var entry in LinkMap.FileIndex.Values)
                        {
                            foreach (var url in entry.ContainedAkaLinks)
                            {
                                writer.WriteLine($"{entry.FullPath.CsvEscape()}" +
                                    $",{url.CsvEscape()}" +
                                    $",{AkaLinkData.GetShortUrl(url).CsvEscape()}");
                            }
                        }
                        writer.Flush();
                        writer.Close();
                    }
                }
                Status.WriteLine(Severity.Information, $"Report written to {SaveDialog.FileName}.");
                return true;
            }
        }

        private void FindLinksInFile(HashSet<string> links, string file)
        {
            links.Clear();
            foreach (var line in File.ReadAllLines(file))
            {
                FindLinksInLine(links, file, line, AkaLede1);
                FindLinksInLine(links, file, line, AkaLede2);
            }
            PostProcessLinkList(links);
        }

        private void FindLinksInLine(HashSet<string> links, string file, string line, string lede)
        {
            int start = 0, index, end;
            while (true)
            {
                if ((index = line.IndexOf(lede, start)) < 0)
                {
                    // Our work here is done.
                    return;
                }

                var c = line[index - 1];
                if (c == '(')
                {
                    // In-line []()-style links.
                    end = line.IndexOf(')', index);
                    if (end > 0)
                    {
                        links.Add(line.Substring(index, end - index).Trim());
                        start = end;
                    }
                    else
                    {
                        start = index + 1;
                    }
                }
                else if (c == '"')
                {
                    // HTML href=""-style links.
                    end = line.IndexOf('"', index);
                    if (end > 0)
                    {
                        links.Add(line.Substring(index, end - index).Trim());
                        start = end;
                    }
                    else
                    {
                        start = index + 1;
                    }
                }
                else if (c == ':' || char.IsWhiteSpace(c))
                {
                    // [Presumably] footnote-style links, []: url. There should only be one per line.
                    // We're also catching aka links in snippet code comments. :|
                    end = line.IndexOf(i => char.IsWhiteSpace(i), index);
                    if (end > 0)
                    {
                        links.Add(line.Substring(index, end - index).Trim());
                        start = end;
                    }
                    else
                    {
                        links.Add(line.Substring(index).Trim());
                        start = line.Length - 1;
                    }
                }
                else
                {
                    Status.WriteLine(Severity.Warning, $"Unrecognized aka link format in {file} in this line:");
                    Status.WriteLine(Severity.Warning, line);
                    start = index + 1;
                }
            }
        }

        private static readonly char[] chars = new char[] { '`', '#' };

        private static void PostProcessLinkList(HashSet<string> links)
        {
            // Enumerate over a copy, so we can modify the original set.
            // This is probably not the most efficient way to do this.
            foreach (var link in links.ToArray())
            {
                var newLink = link;
                var swap = false;
                var i = link.IndexOfAny(chars);
                if (i > -1)
                {
                    newLink = link.Substring(0, i);
                    swap = true;
                }
                if (newLink.EndsWith("/"))
                {
                    newLink = newLink.Substring(0, newLink.Length - 1);
                    swap = true;
                }
                if (swap)
                {
                    links.Remove(link);
                    links.Add(newLink);
                }
            }
        }
    }
}
