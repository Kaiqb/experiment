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
            this.DatePicker = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.dlg_ChooseRepoRoot = new System.Windows.Forms.FolderBrowserDialog();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rtb_Output = new System.Windows.Forms.RichTextBox();
            this.GoButton = new System.Windows.Forms.Button();
            this.OutputBrowser = new System.Windows.Forms.FolderBrowserDialog();
            this.dlg_SaveOutput = new System.Windows.Forms.SaveFileDialog();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 7);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(130, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Repo to report changes in";
            // 
            // RepoRootTextBox
            // 
            this.RepoRootTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RepoRootTextBox.Location = new System.Drawing.Point(9, 24);
            this.RepoRootTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.RepoRootTextBox.Name = "RepoRootTextBox";
            this.RepoRootTextBox.Size = new System.Drawing.Size(492, 20);
            this.RepoRootTextBox.TabIndex = 1;
            this.RepoRootTextBox.TextChanged += new System.EventHandler(this.RepoRoot_TextChanged);
            // 
            // SelectRepoButton
            // 
            this.SelectRepoButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SelectRepoButton.Location = new System.Drawing.Point(505, 24);
            this.SelectRepoButton.Margin = new System.Windows.Forms.Padding(2);
            this.SelectRepoButton.Name = "SelectRepoButton";
            this.SelectRepoButton.Size = new System.Drawing.Size(86, 19);
            this.SelectRepoButton.TabIndex = 2;
            this.SelectRepoButton.Text = "Select repo...";
            this.SelectRepoButton.UseVisualStyleBackColor = true;
            this.SelectRepoButton.Click += new System.EventHandler(this.SelectRepoButton_Click);
            // 
            // DatePicker
            // 
            this.DatePicker.Location = new System.Drawing.Point(12, 62);
            this.DatePicker.MaxDate = new System.DateTime(2019, 4, 15, 0, 0, 0, 0);
            this.DatePicker.Name = "DatePicker";
            this.DatePicker.Size = new System.Drawing.Size(200, 20);
            this.DatePicker.TabIndex = 6;
            this.DatePicker.Value = new System.DateTime(2019, 4, 15, 0, 0, 0, 0);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 46);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Since date";
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
            this.groupBox1.Location = new System.Drawing.Point(9, 89);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(582, 265);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Output";
            // 
            // rtb_Output
            // 
            this.rtb_Output.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtb_Output.Location = new System.Drawing.Point(3, 16);
            this.rtb_Output.Name = "rtb_Output";
            this.rtb_Output.ReadOnly = true;
            this.rtb_Output.Size = new System.Drawing.Size(576, 246);
            this.rtb_Output.TabIndex = 0;
            this.rtb_Output.Text = "";
            // 
            // GoButton
            // 
            this.GoButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.GoButton.Location = new System.Drawing.Point(312, 65);
            this.GoButton.Margin = new System.Windows.Forms.Padding(2);
            this.GoButton.Name = "GoButton";
            this.GoButton.Size = new System.Drawing.Size(102, 19);
            this.GoButton.TabIndex = 9;
            this.GoButton.Text = "Go";
            this.GoButton.UseVisualStyleBackColor = true;
            this.GoButton.Click += new System.EventHandler(this.RunButton_Click);
            // 
            // dlg_SaveOutput
            // 
            this.dlg_SaveOutput.DefaultExt = "csv";
            this.dlg_SaveOutput.Filter = "CSV file|.csv|All files|*.*";
            this.dlg_SaveOutput.Title = "Save output as";
            // 
            // QueryRepoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 366);
            this.Controls.Add(this.GoButton);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.DatePicker);
            this.Controls.Add(this.SelectRepoButton);
            this.Controls.Add(this.RepoRootTextBox);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(2);
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
        private System.Windows.Forms.DateTimePicker DatePicker;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.FolderBrowserDialog dlg_ChooseRepoRoot;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RichTextBox rtb_Output;
        private System.Windows.Forms.Button GoButton;
        private System.Windows.Forms.FolderBrowserDialog OutputBrowser;
        private System.Windows.Forms.SaveFileDialog dlg_SaveOutput;
    }
}

