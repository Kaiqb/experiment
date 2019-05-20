namespace QueryRepoApp
{
    partial class QueryRepoForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.RepoRootTextBox = new System.Windows.Forms.TextBox();
            this.SelectRepoButton = new System.Windows.Forms.Button();
            this.dlg_ChooseRepoRoot = new System.Windows.Forms.FolderBrowserDialog();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rtb_Output = new System.Windows.Forms.RichTextBox();
            this.btn_RunAkaLinkReport = new System.Windows.Forms.Button();
            this.OutputBrowser = new System.Windows.Forms.FolderBrowserDialog();
            this.dlg_SaveOutput = new System.Windows.Forms.SaveFileDialog();
            this.btn_RunCodeLinkReport = new System.Windows.Forms.Button();
            this.btn_RunIssuesReport = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(183, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Select doc repo (local root):";
            // 
            // RepoRootTextBox
            // 
            this.RepoRootTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RepoRootTextBox.Location = new System.Drawing.Point(12, 30);
            this.RepoRootTextBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.RepoRootTextBox.Name = "RepoRootTextBox";
            this.RepoRootTextBox.Size = new System.Drawing.Size(544, 22);
            this.RepoRootTextBox.TabIndex = 1;
            this.RepoRootTextBox.TextChanged += new System.EventHandler(this.RepoRoot_TextChanged);
            // 
            // SelectRepoButton
            // 
            this.SelectRepoButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SelectRepoButton.Location = new System.Drawing.Point(562, 30);
            this.SelectRepoButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.SelectRepoButton.Name = "SelectRepoButton";
            this.SelectRepoButton.Size = new System.Drawing.Size(115, 23);
            this.SelectRepoButton.TabIndex = 2;
            this.SelectRepoButton.Text = "Select repo...";
            this.SelectRepoButton.UseVisualStyleBackColor = true;
            this.SelectRepoButton.Click += new System.EventHandler(this.SelectRepoButton_Click);
            // 
            // dlg_ChooseRepoRoot
            // 
            this.dlg_ChooseRepoRoot.Description = "Select the repository in which you want to find changes.";
            this.dlg_ChooseRepoRoot.RootFolder = System.Environment.SpecialFolder.MyComputer;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.rtb_Output);
            this.groupBox1.Location = new System.Drawing.Point(12, 110);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(665, 423);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Output";
            // 
            // rtb_Output
            // 
            this.rtb_Output.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtb_Output.Location = new System.Drawing.Point(4, 19);
            this.rtb_Output.Margin = new System.Windows.Forms.Padding(4);
            this.rtb_Output.Name = "rtb_Output";
            this.rtb_Output.ReadOnly = true;
            this.rtb_Output.Size = new System.Drawing.Size(657, 400);
            this.rtb_Output.TabIndex = 0;
            this.rtb_Output.Text = "";
            // 
            // btn_RunAkaLinkReport
            // 
            this.btn_RunAkaLinkReport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_RunAkaLinkReport.Location = new System.Drawing.Point(58, 78);
            this.btn_RunAkaLinkReport.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_RunAkaLinkReport.Name = "btn_RunAkaLinkReport";
            this.btn_RunAkaLinkReport.Size = new System.Drawing.Size(165, 23);
            this.btn_RunAkaLinkReport.TabIndex = 9;
            this.btn_RunAkaLinkReport.Text = "Run aka link report";
            this.btn_RunAkaLinkReport.UseVisualStyleBackColor = true;
            this.btn_RunAkaLinkReport.Click += new System.EventHandler(this.RunAkaLinkReport_Click);
            // 
            // dlg_SaveOutput
            // 
            this.dlg_SaveOutput.DefaultExt = "csv";
            this.dlg_SaveOutput.Filter = "CSV file|.csv|All files|*.*";
            this.dlg_SaveOutput.Title = "Save output as";
            // 
            // btn_RunCodeLinkReport
            // 
            this.btn_RunCodeLinkReport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_RunCodeLinkReport.Location = new System.Drawing.Point(229, 78);
            this.btn_RunCodeLinkReport.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_RunCodeLinkReport.Name = "btn_RunCodeLinkReport";
            this.btn_RunCodeLinkReport.Size = new System.Drawing.Size(165, 23);
            this.btn_RunCodeLinkReport.TabIndex = 10;
            this.btn_RunCodeLinkReport.Text = "Run code link report";
            this.btn_RunCodeLinkReport.UseVisualStyleBackColor = true;
            this.btn_RunCodeLinkReport.Click += new System.EventHandler(this.RunCodeLinkReport_Click);
            // 
            // btn_RunIssuesReport
            // 
            this.btn_RunIssuesReport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_RunIssuesReport.Location = new System.Drawing.Point(400, 78);
            this.btn_RunIssuesReport.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_RunIssuesReport.Name = "btn_RunIssuesReport";
            this.btn_RunIssuesReport.Size = new System.Drawing.Size(165, 23);
            this.btn_RunIssuesReport.TabIndex = 11;
            this.btn_RunIssuesReport.Text = "Run issues report";
            this.btn_RunIssuesReport.UseVisualStyleBackColor = true;
            this.btn_RunIssuesReport.Click += new System.EventHandler(this.RunGitHubIssuesReportAsync);
            // 
            // QueryRepoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(689, 547);
            this.Controls.Add(this.btn_RunIssuesReport);
            this.Controls.Add(this.btn_RunCodeLinkReport);
            this.Controls.Add(this.btn_RunAkaLinkReport);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.SelectRepoButton);
            this.Controls.Add(this.RepoRootTextBox);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "QueryRepoForm";
            this.Text = "Query Repo App";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.QueryRepoForm_FormClosing);
            this.Load += new System.EventHandler(this.QueryRepoForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox RepoRootTextBox;
        private System.Windows.Forms.Button SelectRepoButton;
        private System.Windows.Forms.FolderBrowserDialog dlg_ChooseRepoRoot;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RichTextBox rtb_Output;
        private System.Windows.Forms.Button btn_RunAkaLinkReport;
        private System.Windows.Forms.FolderBrowserDialog OutputBrowser;
        private System.Windows.Forms.SaveFileDialog dlg_SaveOutput;
        private System.Windows.Forms.Button btn_RunCodeLinkReport;
        private System.Windows.Forms.Button btn_RunIssuesReport;
    }
}

