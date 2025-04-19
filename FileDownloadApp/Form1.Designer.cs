namespace FileDownloadApp
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TextBox txtUrl;
        private System.Windows.Forms.TextBox txtDestination;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.DataGridView dgvDownloads;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support.
        /// </summary>
        private void InitializeComponent()
        {
            txtUrl = new TextBox();
            txtDestination = new TextBox();
            btnAdd = new Button();
            dgvDownloads = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)dgvDownloads).BeginInit();
            SuspendLayout();
            // 
            // txtUrl
            // 
            txtUrl.Location = new Point(14, 14);
            txtUrl.Margin = new Padding(4, 3, 4, 3);
            txtUrl.Name = "txtUrl";
            txtUrl.PlaceholderText = "File URL";
            txtUrl.Size = new Size(524, 23);
            txtUrl.TabIndex = 0;
            // 
            // txtDestination
            // 
            txtDestination.Location = new Point(14, 44);
            txtDestination.Margin = new Padding(4, 3, 4, 3);
            txtDestination.Name = "txtDestination";
            txtDestination.PlaceholderText = "Destination File Path";
            txtDestination.Size = new Size(524, 23);
            txtDestination.TabIndex = 1;
            // 
            // btnAdd
            // 
            btnAdd.Location = new Point(546, 28);
            btnAdd.Margin = new Padding(4, 3, 4, 3);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(88, 27);
            btnAdd.TabIndex = 2;
            btnAdd.Text = "Add";
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += btnAdd_Click;
            // 
            // dgvDownloads
            // 
            dgvDownloads.AllowUserToAddRows = false;
            dgvDownloads.AllowUserToDeleteRows = false;
            dgvDownloads.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvDownloads.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvDownloads.Location = new Point(14, 81);
            dgvDownloads.Margin = new Padding(4, 3, 4, 3);
            dgvDownloads.Name = "dgvDownloads";
            dgvDownloads.ReadOnly = true;
            dgvDownloads.Size = new Size(1003, 438);
            dgvDownloads.TabIndex = 3;
            dgvDownloads.CellContentClick += DgvDownloads_CellContentClick;
            dgvDownloads.CellFormatting += DgvDownloads_CellFormatting;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1031, 533);
            Controls.Add(dgvDownloads);
            Controls.Add(btnAdd);
            Controls.Add(txtDestination);
            Controls.Add(txtUrl);
            Margin = new Padding(4, 3, 4, 3);
            Name = "Form1";
            Text = "Multi Threaded Downloader";
            ((System.ComponentModel.ISupportInitialize)dgvDownloads).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
    }
}
