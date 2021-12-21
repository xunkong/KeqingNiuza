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
            HttpClient = new HttpClient();
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

            await ParallelForEachAsync(infos, async info =>
            {
                byte[] buffer = new byte[BUFFERSIZE];
                MemoryStream ms = new MemoryStream();
                using (Stream hs = await HttpClient.GetStreamAsync(info.Url))
                {
                    while (true)
                    {
                        if (isCancel)
                        {
                            isCancel = false;
                            IsDownloading = false;
                            return;
                        }
                        int readSize = await hs.ReadAsync(buffer, 0, BUFFERSIZE);
                        if (readSize == 0)
                        {
                            break;
                        }
                        await ms.WriteAsync(buffer, 0, readSize);
                        Interlocked.Add(ref downloadedSize, readSize);
                    }
                }
                ms.Position = 0;
                await WriteStreamToFile(info, ms);
            });
            var args = new ProgressChangedEventArgs(TotalSize, DownloadedSize, Speed);
            ProgressChanged?.Invoke(this, args);
            IsDownloading = false;
            DownloadFinished?.Invoke(this, null);
        }

        private async Task WriteStreamToFile(KeqingNiuzaFileInfo info, MemoryStream ms)
        {
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

        public async Task ParallelForEachAsync<T>(IEnumerable<T> source, Func<T, Task> asyncAction)
        {
            //Environment.ProcessorCount 逻辑处理器
            SemaphoreSlim throttler = new SemaphoreSlim(Environment.ProcessorCount * 4);
            IEnumerable<Task> tasks = source.Select(async item =>
            {
                await throttler.WaitAsync();
                try
                {
                    await asyncAction(item);
                }
                catch
                {
                    throw;
                }
                finally
                {
                    throttler.Release();
                }
            });
            await Task.WhenAll(tasks);
        }

        public void Cancel()
        {
            if (IsDownloading)
            {
                isCancel = true;
            }
        }
    }
}
