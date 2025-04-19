using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;

namespace FileDownloadApp
{
    /// <summary>
    /// Manages download queue and concurrent downloads.
    /// </summary>
    public class DownloadService
    {
        private readonly BindingList<DownloadItem> downloads;
        private readonly ConcurrentQueue<DownloadItem> queue = new ConcurrentQueue<DownloadItem>();
        private readonly SemaphoreSlim semaphore;
        private readonly SynchronizationContext context;

        public DownloadService(BindingList<DownloadItem> downloads, int maxConcurrentDownloads)
        {
            this.downloads = downloads;
            this.semaphore = new SemaphoreSlim(maxConcurrentDownloads);
            this.context = SynchronizationContext.Current;

            // Listen for network availability changes
            NetworkChange.NetworkAvailabilityChanged += (s, e) =>
            {
                if (e.IsAvailable)
                    ResumeAllPending();
            };
        }

        /// <summary>
        /// Add a download to the queue.
        /// </summary>
        public void EnqueueDownload(DownloadItem item)
        {
            item.Status = "Queued";
            downloads.Add(item);
            queue.Enqueue(item);
            ProcessQueue();
        }

        /// <summary>
        /// Process the download queue.
        /// </summary>
        private async void ProcessQueue()
        {
            while (queue.TryDequeue(out var item))
            {
                if (item.Status != "Queued")
                    continue;

                await semaphore.WaitAsync();
                StartDownload(item);
            }
        }

        /// <summary>
        /// Start downloading a file.
        /// </summary>
        private void StartDownload(DownloadItem item)
        {
            item.Status = "Downloading";
            item.CancellationTokenSource = new CancellationTokenSource();
            var cts = item.CancellationTokenSource;

            Task.Run(async () =>
            {
                try
                {
                    long existing = 0;
                    if (File.Exists(item.Destination))
                        existing = new FileInfo(item.Destination).Length;
                    item.BytesReceived = existing;

                    var request = (HttpWebRequest)WebRequest.Create(item.Url);
                    request.AddRange(existing);

                    using (var response = (HttpWebResponse)await request.GetResponseAsync())
                    using (var stream = response.GetResponseStream())
                    using (var fs = new FileStream(item.Destination, FileMode.Append, FileAccess.Write, FileShare.None))
                    {
                        item.TotalBytes = existing + response.ContentLength;
                        byte[] buffer = new byte[8192];
                        var sw = Stopwatch.StartNew();
                        int bytesRead;

                        while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, cts.Token)) > 0)
                        {
                            fs.Write(buffer, 0, bytesRead);
                            item.BytesReceived += bytesRead;

                            double elapsedSec = sw.Elapsed.TotalSeconds;
                            double currSpeed = elapsedSec > 0
                                ? item.BytesReceived / 1024d / elapsedSec
                                : 0;
                            int currProgress = (int)(item.BytesReceived / (double)item.TotalBytes * 100);
                            double remBytes = item.TotalBytes - item.BytesReceived;
                            double remSec = currSpeed > 0
                                ? (remBytes / 1024d) / currSpeed
                                : 0;
                            var remaining = TimeSpan.FromSeconds(remSec);

                            context.Post(_ =>
                            {
                                item.Speed = currSpeed;
                                item.Progress = currProgress;
                                item.RemainingTime = remaining.ToString(@"hh\:mm\:ss");
                            }, null);
                        }
                    }

                    context.Post(_ => item.Status = "Completed", null);
                }
                catch (OperationCanceledException)
                {
                    context.Post(_ => item.Status = "Paused", null);
                }
                catch
                {
                    context.Post(_ => item.Status = "Error", null);
                }
                finally
                {
                    semaphore.Release();
                    ProcessQueue();
                }
            });
        }

        /// <summary>
        /// Pause a download.
        /// </summary>
        public void PauseDownload(DownloadItem item)
        {
            if (item.Status == "Downloading")
                item.CancellationTokenSource?.Cancel();
        }

        /// <summary>
        /// Resume a paused or errored download.
        /// </summary>
        public void ResumeDownload(DownloadItem item)
        {
            if (item.Status == "Paused" || item.Status == "Error")
            {
                item.Status = "Queued";
                queue.Enqueue(item);
                ProcessQueue();
            }
        }

        /// <summary>
        /// Resume all paused or errored downloads.
        /// </summary>
        private void ResumeAllPending()
        {
            foreach (var item in downloads)
            {
                if (item.Status == "Paused" || item.Status == "Error")
                    ResumeDownload(item);
            }
        }
    }
}
