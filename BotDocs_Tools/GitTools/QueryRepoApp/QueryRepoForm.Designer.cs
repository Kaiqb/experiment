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
            this.SelectDirectoryButton = new System.Windows.Forms.Button();
            this.OutputDirectoryTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SinceDatePicker = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.FolderPicker = new System.Windows.Forms.FolderBrowserDialog();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.OutputRichTextBox = new System.Windows.Forms.RichTextBox();
            this.RunButton = new System.Windows.Forms.Button();
            this.OutputBrowser = new System.Windows.Forms.FolderBrowserDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
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
            this.RepoRootTextBox.TextChanged += new System.EventHandler(this.RepoRootTextBox_TextChanged);
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
            // SelectDirectoryButton
            // 
            this.SelectDirectoryButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SelectDirectoryButton.Location = new System.Drawing.Point(489, 62);
            this.SelectDirectoryButton.Margin = new System.Windows.Forms.Padding(2);
            this.SelectDirectoryButton.Name = "SelectDirectoryButton";
            this.SelectDirectoryButton.Size = new System.Drawing.Size(102, 19);
            this.SelectDirectoryButton.TabIndex = 5;
            this.SelectDirectoryButton.Text = "Select directory...";
            this.SelectDirectoryButton.UseVisualStyleBackColor = true;
            this.SelectDirectoryButton.Click += new System.EventHandler(this.SelectDirectoryButton_Click);
            // 
            // OutputDirectoryTextBox
            // 
            this.OutputDirectoryTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.OutputDirectoryTextBox.Location = new System.Drawing.Point(9, 61);
            this.OutputDirectoryTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.OutputDirectoryTextBox.Name = "OutputDirectoryTextBox";
            this.OutputDirectoryTextBox.Size = new System.Drawing.Size(476, 20);
            this.OutputDirectoryTextBox.TabIndex = 4;
            this.OutputDirectoryTextBox.TextChanged += new System.EventHandler(this.OutputDirectoryTextBox_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 46);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Output Directory";
            // 
            // SinceDatePicker
            // 
            this.SinceDatePicker.Location = new System.Drawing.Point(9, 106);
            this.SinceDatePicker.MaxDate = new System.DateTime(2019, 4, 15, 0, 0, 0, 0);
            this.SinceDatePicker.Name = "SinceDatePicker";
            this.SinceDatePicker.Size = new System.Drawing.Size(200, 20);
            this.SinceDatePicker.TabIndex = 6;
            this.SinceDatePicker.Value = new System.DateTime(2019, 4, 15, 0, 0, 0, 0);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 90);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Since date";
            // 
            // RepoBrowser
            // 
            this.FolderPicker.Description = "Select the repository in which you want to find changes.";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.OutputRichTextBox);
            this.groupBox1.Location = new System.Drawing.Point(9, 132);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(582, 222);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Output";
            // 
            // OutputRichTextBox
            // 
            this.OutputRichTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.OutputRichTextBox.Location = new System.Drawing.Point(3, 16);
            this.OutputRichTextBox.Name = "OutputRichTextBox";
            this.OutputRichTextBox.ReadOnly = true;
            this.OutputRichTextBox.Size = new System.Drawing.Size(576, 203);
            this.OutputRichTextBox.TabIndex = 0;
            this.OutputRichTextBox.Text = "";
            // 
            // RunButton
            // 
            this.RunButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.RunButton.Location = new System.Drawing.Point(487, 109);
            this.RunButton.Margin = new System.Windows.Forms.Padding(2);
            this.RunButton.Name = "RunButton";
            this.RunButton.Size = new System.Drawing.Size(102, 19);
            this.RunButton.TabIndex = 9;
            this.RunButton.Text = "Run";
            this.RunButton.UseVisualStyleBackColor = true;
            this.RunButton.Click += new System.EventHandler(this.RunButton_Click);
            // 
            // QueryRepoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 366);
            this.Controls.Add(this.RunButton);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.SinceDatePicker);
            this.Controls.Add(this.SelectDirectoryButton);
            this.Controls.Add(this.OutputDirectoryTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.SelectRepoButton);
            this.Controls.Add(this.RepoRootTextBox);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "QueryRepoForm";
            this.Text = "Query Repo App";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox RepoRootTextBox;
        private System.Windows.Forms.Button SelectRepoButton;
        private System.Windows.Forms.Button SelectDirectoryButton;
        private System.Windows.Forms.TextBox OutputDirectoryTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker SinceDatePicker;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.FolderBrowserDialog FolderPicker;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RichTextBox OutputRichTextBox;
        private System.Windows.Forms.Button RunButton;
        private System.Windows.Forms.FolderBrowserDialog OutputBrowser;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
    }
}

