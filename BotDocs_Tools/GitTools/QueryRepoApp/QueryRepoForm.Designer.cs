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
            this.tb_DocRepoRoot = new System.Windows.Forms.TextBox();
            this.btn_SelectDocRepo = new System.Windows.Forms.Button();
            this.dlg_ChooseRepoRoot = new System.Windows.Forms.FolderBrowserDialog();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rtb_Output = new System.Windows.Forms.RichTextBox();
            this.btn_RunAkaLinkReport = new System.Windows.Forms.Button();
            this.OutputBrowser = new System.Windows.Forms.FolderBrowserDialog();
            this.dlg_SaveOutput = new System.Windows.Forms.SaveFileDialog();
            this.btn_RunCodeLinkReport = new System.Windows.Forms.Button();
            this.btn_SelectCodeRepo = new System.Windows.Forms.Button();
            this.tb_CodeRepoRoot = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.dateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(142, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Doc repo (local root):";
            // 
            // tb_DocRepoRoot
            // 
            this.tb_DocRepoRoot.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tb_DocRepoRoot.Enabled = false;
            this.tb_DocRepoRoot.Location = new System.Drawing.Point(174, 6);
            this.tb_DocRepoRoot.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tb_DocRepoRoot.Name = "tb_DocRepoRoot";
            this.tb_DocRepoRoot.Size = new System.Drawing.Size(790, 22);
            this.tb_DocRepoRoot.TabIndex = 1;
            // 
            // btn_SelectDocRepo
            // 
            this.btn_SelectDocRepo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_SelectDocRepo.Location = new System.Drawing.Point(971, 6);
            this.btn_SelectDocRepo.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_SelectDocRepo.Name = "btn_SelectDocRepo";
            this.btn_SelectDocRepo.Size = new System.Drawing.Size(115, 23);
            this.btn_SelectDocRepo.TabIndex = 2;
            this.btn_SelectDocRepo.Text = "Select repo...";
            this.btn_SelectDocRepo.UseVisualStyleBackColor = true;
            this.btn_SelectDocRepo.Click += new System.EventHandler(this.SelectDocRepoButton_Click);
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
            this.groupBox1.Location = new System.Drawing.Point(12, 139);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(1074, 394);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Output";
            // 
            // rtb_Output
            // 
            this.rtb_Output.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtb_Output.Font = new System.Drawing.Font("Lucida Console", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtb_Output.Location = new System.Drawing.Point(4, 19);
            this.rtb_Output.Margin = new System.Windows.Forms.Padding(4);
            this.rtb_Output.Name = "rtb_Output";
            this.rtb_Output.ReadOnly = true;
            this.rtb_Output.Size = new System.Drawing.Size(1066, 371);
            this.rtb_Output.TabIndex = 0;
            this.rtb_Output.Text = "";
            // 
            // btn_RunAkaLinkReport
            // 
            this.btn_RunAkaLinkReport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_RunAkaLinkReport.Location = new System.Drawing.Point(473, 110);
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
            this.btn_RunCodeLinkReport.Location = new System.Drawing.Point(644, 110);
            this.btn_RunCodeLinkReport.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_RunCodeLinkReport.Name = "btn_RunCodeLinkReport";
            this.btn_RunCodeLinkReport.Size = new System.Drawing.Size(165, 23);
            this.btn_RunCodeLinkReport.TabIndex = 10;
            this.btn_RunCodeLinkReport.Text = "Run code link report";
            this.btn_RunCodeLinkReport.UseVisualStyleBackColor = true;
            this.btn_RunCodeLinkReport.Click += new System.EventHandler(this.RunCodeLinkReport_Click);
            // 
            // btn_SelectCodeRepo
            // 
            this.btn_SelectCodeRepo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_SelectCodeRepo.Location = new System.Drawing.Point(971, 33);
            this.btn_SelectCodeRepo.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_SelectCodeRepo.Name = "btn_SelectCodeRepo";
            this.btn_SelectCodeRepo.Size = new System.Drawing.Size(115, 23);
            this.btn_SelectCodeRepo.TabIndex = 13;
            this.btn_SelectCodeRepo.Text = "Select repo...";
            this.btn_SelectCodeRepo.UseVisualStyleBackColor = true;
            this.btn_SelectCodeRepo.Click += new System.EventHandler(this.SelectCodeRepoButton_Click);
            // 
            // tb_CodeRepoRoot
            // 
            this.tb_CodeRepoRoot.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tb_CodeRepoRoot.Enabled = false;
            this.tb_CodeRepoRoot.Location = new System.Drawing.Point(174, 33);
            this.tb_CodeRepoRoot.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tb_CodeRepoRoot.Name = "tb_CodeRepoRoot";
            this.tb_CodeRepoRoot.Size = new System.Drawing.Size(790, 22);
            this.tb_CodeRepoRoot.TabIndex = 12;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(150, 17);
            this.label2.TabIndex = 11;
            this.label2.Text = "Code repo (local root):";
            // 
            // dateTimePicker
            // 
            this.dateTimePicker.Checked = false;
            this.dateTimePicker.Location = new System.Drawing.Point(174, 60);
            this.dateTimePicker.Name = "dateTimePicker";
            this.dateTimePicker.ShowCheckBox = true;
            this.dateTimePicker.Size = new System.Drawing.Size(308, 22);
            this.dateTimePicker.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 65);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(110, 17);
            this.label3.TabIndex = 14;
            this.label3.Text = "Freshness date:";
            // 
            // QueryRepoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1098, 546);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.dateTimePicker);
            this.Controls.Add(this.btn_SelectCodeRepo);
            this.Controls.Add(this.tb_CodeRepoRoot);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btn_RunCodeLinkReport);
            this.Controls.Add(this.btn_RunAkaLinkReport);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btn_SelectDocRepo);
            this.Controls.Add(this.tb_DocRepoRoot);
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
        private System.Windows.Forms.TextBox tb_DocRepoRoot;
        private System.Windows.Forms.Button btn_SelectDocRepo;
        private System.Windows.Forms.FolderBrowserDialog dlg_ChooseRepoRoot;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RichTextBox rtb_Output;
        private System.Windows.Forms.Button btn_RunAkaLinkReport;
        private System.Windows.Forms.FolderBrowserDialog OutputBrowser;
        private System.Windows.Forms.SaveFileDialog dlg_SaveOutput;
        private System.Windows.Forms.Button btn_RunCodeLinkReport;
        private System.Windows.Forms.Button btn_SelectCodeRepo;
        private System.Windows.Forms.TextBox tb_CodeRepoRoot;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dateTimePicker;
        private System.Windows.Forms.Label label3;
    }
}

