// DownloadItem.cs
// Represents a single download item

using System;
using System.ComponentModel;
using System.Threading;

namespace FileDownloadApp
{
    /// <summary>
    /// Represents a file download and its properties.
    /// </summary>
    public class DownloadItem : INotifyPropertyChanged
    {
        public string Url { get; set; }
        public string Destination { get; set; }

        private int progress;
        public int Progress
        {
            get => progress;
            set { progress = value; OnPropertyChanged(nameof(Progress)); }
        }

        private string status;
        public string Status
        {
            get => status;
            set { status = value; OnPropertyChanged(nameof(Status)); }
        }

        private double speed;
        public double Speed
        {
            get => speed;
            set { speed = value; OnPropertyChanged(nameof(Speed)); }
        }

        private string remainingTime;
        public string RemainingTime
        {
            get => remainingTime;
            set { remainingTime = value; OnPropertyChanged(nameof(RemainingTime)); }
        }

        private long bytesReceived;
        public long BytesReceived
        {
            get => bytesReceived;
            set
            {
                bytesReceived = value;
                OnPropertyChanged(nameof(BytesReceived));
                OnPropertyChanged(nameof(DownloadedSize));
                OnPropertyChanged(nameof(RemainingSize));
            }
        }

        private long totalBytes;
        public long TotalBytes
        {
            get => totalBytes;
            set
            {
                totalBytes = value;
                OnPropertyChanged(nameof(TotalBytes));
                OnPropertyChanged(nameof(TotalSize));
                OnPropertyChanged(nameof(RemainingSize));
            }
        }

        // Readable size strings for UI
        public string TotalSize => FormatBytes(TotalBytes);
        public string DownloadedSize => FormatBytes(BytesReceived);
        public string RemainingSize => FormatBytes(TotalBytes - BytesReceived);

        internal CancellationTokenSource CancellationTokenSource { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));

        /// <summary>
        /// Converts bytes to KB/MB/GB string.
        /// </summary>
        private static string FormatBytes(long bytes)
        {
            const long KB = 1024;
            const long MB = KB * 1024;
            const long GB = MB * 1024;

            if (bytes >= GB) return $"{bytes / (double)GB:0.##} GB";
            if (bytes >= MB) return $"{bytes / (double)MB:0.##} MB";
            if (bytes >= KB) return $"{bytes / (double)KB:0.##} KB";
            return $"{bytes} B";
        }
    }
}
