using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace KeqingNiuza.Launcher
{
    class Downloader
    {
        private const int BUFFERSIZE = 4096;

        private HttpClient HttpClient;

        private bool isCancel;

        private System.Timers.Timer timer;

        private Stopwatch stopwatch;

        private long lastDownloadedSize;

        private long lastElapsedMilliseconds;

        private long downloadedSize;

        public long TotalSize { get; private set; }

        public long DownloadedSize { get => downloadedSize; private set => downloadedSize = value; }

        public long Speed { get; private set; }

        public bool IsDownloading { get; private set; }


        public event EventHandler<ProgressChangedEventArgs> ProgressChanged;

        public event EventHandler DownloadFinished;

        public Downloader(double progressInterval = 100)
        {
            HttpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(15) };
            HttpClient.DefaultRequestHeaders.Add("User-Agent", $"KeqingLauncher/{MetaData.FileVersion} UserId/{MetaData.UserId}");
            timer = new System.Timers.Timer(progressInterval);
            timer.Elapsed += ComputeSpeed;
            timer.AutoReset = true;
        }

        private void ComputeSpeed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (IsDownloading)
            {
                var detalTime = stopwatch.ElapsedMilliseconds - lastElapsedMilliseconds;
                if (detalTime > 500)
                {
                    var detalSize = DownloadedSize - lastDownloadedSize;
                    Speed = 1000 * detalSize / detalTime;
                    lastDownloadedSize = DownloadedSize;
                    lastElapsedMilliseconds = stopwatch.ElapsedMilliseconds;
                }

                var args = new ProgressChangedEventArgs(TotalSize, DownloadedSize, Speed);
                ProgressChanged?.Invoke(this, args);
            }
            else
            {
                timer.Stop();
                Speed = 0;
                lastDownloadedSize = 0;
                lastElapsedMilliseconds = 0;
                stopwatch?.Reset();
            }
        }



        public async Task DownloadAsync(List<KeqingNiuzaFileInfo> infos)
        {
            if (infos is null)
            {
                return;
            }
            TotalSize = infos.Sum(x => x.CompressedSize);
            DownloadedSize = 0;
            IsDownloading = true;
            stopwatch = Stopwatch.StartNew();
            timer.Start();

            using (var source = new CancellationTokenSource())
            {
                var tasks = infos.Select(async info => await DownloadOneFileAsync(info, source));
                await Task.WhenAll(tasks).ConfigureAwait(false);
            }

            var args = new ProgressChangedEventArgs(TotalSize, DownloadedSize, Speed);
            ProgressChanged?.Invoke(this, args);
            IsDownloading = false;
            DownloadFinished?.Invoke(this, null);
        }


        public void Cancel()
        {
            if (IsDownloading)
            {
                isCancel = true;
            }
        }


        private async Task DownloadOneFileAsync(KeqingNiuzaFileInfo info, CancellationTokenSource cts)
        {
            if (isCancel)
            {
                return;
            }
            try
            {
                byte[] buffer = new byte[BUFFERSIZE];
                MemoryStream ms = new MemoryStream();
                var message = await HttpClient.GetAsync(info.Url, HttpCompletionOption.ResponseHeadersRead, cts.Token);
                using (Stream hs = await message.Content.ReadAsStreamAsync())
                {
                    while (true)
                    {
                        if (isCancel)
                        {
                            IsDownloading = false;
                            return;
                        }
                        int readSize = await hs.ReadAsync(buffer, 0, BUFFERSIZE, cts.Token);
                        if (readSize == 0)
                        {
                            break;
                        }
                        await ms.WriteAsync(buffer, 0, readSize);
                        Interlocked.Add(ref downloadedSize, readSize);
                    }
                }
                ms.Position = 0;
                Directory.CreateDirectory(Path.GetDirectoryName(info.Path));
                using (var ds = new DeflateStream(ms, CompressionMode.Decompress))
                {
                    using (var fs = File.OpenWrite(info.Path))
                    {
                        fs.SetLength(info.Size);
                        await ds.CopyToAsync(fs);
                    }
                }
            }
            catch (Exception ex)
            {
                isCancel = true;
                IsDownloading = false;
                cts.Cancel();
                throw;
            }
        }

    }
}
