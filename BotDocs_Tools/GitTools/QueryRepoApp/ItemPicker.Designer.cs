namespace QueryRepoApp
{
    partial class ItemPicker
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
            this.btn_Ok = new System.Windows.Forms.Button();
            this.btn_Cancel = new System.Windows.Forms.Button();
            this.listBox = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // btn_Ok
            // 
            this.btn_Ok.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_Ok.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btn_Ok.Location = new System.Drawing.Point(12, 142);
            this.btn_Ok.Name = "btn_Ok";
            this.btn_Ok.Size = new System.Drawing.Size(115, 23);
            this.btn_Ok.TabIndex = 0;
            this.btn_Ok.Text = "OK";
            this.btn_Ok.UseVisualStyleBackColor = true;
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_Cancel.Location = new System.Drawing.Point(133, 142);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(115, 23);
            this.btn_Cancel.TabIndex = 1;
            this.btn_Cancel.Text = "Cancel";
            this.btn_Cancel.UseVisualStyleBackColor = true;
            // 
            // listBox
            // 
            this.listBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox.FormattingEnabled = true;
            this.listBox.ItemHeight = 16;
            this.listBox.Location = new System.Drawing.Point(12, 12);
            this.listBox.Name = "listBox";
            this.listBox.Size = new System.Drawing.Size(435, 100);
            this.listBox.TabIndex = 1;
            // 
            // ItemPicker
            // 
            this.AcceptButton = this.btn_Ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btn_Cancel;
            this.ClientSize = new System.Drawing.Size(459, 203);
            this.ControlBox = false;
            this.Controls.Add(this.listBox);
            this.Controls.Add(this.btn_Cancel);
            this.Controls.Add(this.btn_Ok);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ItemPicker";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Choose the code repo";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_Ok;
        private System.Windows.Forms.Button btn_Cancel;
        private System.Windows.Forms.ListBox listBox;
    }
}