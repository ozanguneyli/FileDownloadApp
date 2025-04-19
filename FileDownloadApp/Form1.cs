using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace FileDownloadApp
{
    public partial class Form1 : Form
    {
        private BindingList<DownloadItem> downloadItems = new BindingList<DownloadItem>();
        private DownloadService service;

        public Form1()
        {
            InitializeComponent();

            // Manually create DataGridView columns
            dgvDownloads.AutoGenerateColumns = false;

            dgvDownloads.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Url",
                HeaderText = "URL",
                Width = 200
            });
            dgvDownloads.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Destination",
                HeaderText = "Destination",
                Width = 180
            });
            dgvDownloads.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "TotalSize",
                HeaderText = "Size",
                Width = 80
            });
            dgvDownloads.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "DownloadedSize",
                HeaderText = "Downloaded",
                Width = 80
            });
            dgvDownloads.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "RemainingSize",
                HeaderText = "Remaining",
                Width = 80
            });
            dgvDownloads.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Status",
                HeaderText = "Status",
                Width = 80
            });
            dgvDownloads.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Speed",
                HeaderText = "Speed (KB/s)",
                Width = 80,
                DefaultCellStyle = { Format = "F2" }
            });
            dgvDownloads.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "RemainingTime",
                HeaderText = "ETA",
                Width = 80
            });
            dgvDownloads.Columns.Add(new DataGridViewProgressBarColumn
            {
                DataPropertyName = "Progress",
                HeaderText = "Progress",
                Width = 120
            });
            dgvDownloads.Columns.Add(new DataGridViewButtonColumn
            {
                HeaderText = "Action",
                UseColumnTextForButtonValue = false,
                Width = 80
            });

            dgvDownloads.DataSource = downloadItems;

            // Allow max 3 concurrent downloads
            service = new DownloadService(downloadItems, 3);

            dgvDownloads.CellContentClick += DgvDownloads_CellContentClick;
            dgvDownloads.CellFormatting += DgvDownloads_CellFormatting;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUrl.Text) ||
                string.IsNullOrWhiteSpace(txtDestination.Text))
                return;

            var item = new DownloadItem
            {
                Url = txtUrl.Text.Trim(),
                Destination = txtDestination.Text.Trim()
            };
            service.EnqueueDownload(item);
        }

        private void DgvDownloads_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (dgvDownloads.Columns[e.ColumnIndex] is DataGridViewButtonColumn)
            {
                var item = downloadItems[e.RowIndex];
                if (item.Status == "Downloading")
                    service.PauseDownload(item);
                else
                    service.ResumeDownload(item);
            }
        }

        private void DgvDownloads_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (dgvDownloads.Columns[e.ColumnIndex] is DataGridViewButtonColumn)
            {
                var item = downloadItems[e.RowIndex];
                e.Value = (item.Status == "Downloading") ? "Pause" : "Resume";
            }
        }
    }
}
