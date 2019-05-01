using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ReportUtils
{
    public class CodeLinkReport : BaseReport
    {
        public const string DocConfigFile = ".openpublishing.publish.config.json";

        public FolderBrowserDialog ChooseFolder { get; private set; }

        public string DocPath { get; set; }

        private CodeLinkMap LinkMap { get; } = new CodeLinkMap();

        public CodeLinkReport(
            RichTextBox status, FolderBrowserDialog chooseFolder, SaveFileDialog saveDialog)
            : base(status, saveDialog)
        {
            ChooseFolder = chooseFolder;
        }

        public override bool Run()
        {
            base.Run();

            Contract.Requires(DocPath != null);
            Contract.Requires(Directory.Exists(DocPath));
            Contract.Requires(Directory.Exists(Path.Combine(DocPath, ".git")));
            Contract.Requires(Directory.Exists(Path.Combine(DocPath, ArticlesRoot)));
            Contract.Requires(File.Exists(Path.Combine(DocPath, DocConfigFile)));

            // Is there a relevant entry in the doc repo's config to support code links.

            LinkMap.Clear();

            var dir = Path.Combine(DocPath, ArticlesRoot);
            var files = Directory.GetFiles(dir, "*.md", SearchOption.AllDirectories);

            // Find the links.
            // Back fill the commit date info for the referenced and referencing files.

            if (LinkMap.IsEmpty)
            {
                Status.WriteLine(Severity.Warning, "No code links found in this repo.");
                return false;
            }
            else
            {
                Status.WriteLine(Severity.Information, "Found links to " +
                    $"{LinkMap.CodeFileIndex.Count} code files, linked to from" +
                    $"{LinkMap.DocFileIndex.Count} doc files.");

                SaveDialog.Title = "Choose where to save the code link report:";
                var result = SaveDialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    WriteReport();
                }

                Status.WriteLine(Severity.Information, $"Report written to {SaveDialog.FileName}.");
                return true;
            }
        }

        private void WriteReport()
        {
            using (TextWriter writer = new StreamWriter(SaveDialog.FileName, false))
            {
                // Write header row.
                // Write out all the link data.
                writer.Flush();
                writer.Close();
            }
        }
    }
}
