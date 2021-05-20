using KeqingNiuza.Common;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using static KeqingNiuza.Common.Const;

namespace KeqingNiuza.Service
{
    class Updater
    {
        private readonly HttpClient HttpClient;

        private const string _UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/89.0.4389.114 Safari/537.36 Edg/89.0.774.68";

#if TestCDN

        private readonly string _FileListUrl = "https://cdn.jsdelivr.net/gh/Scighost/KeqingNiuza@cdn/UpdateFileList_Debug.json";

        private readonly string _ResourceListUrl = "https://cdn.jsdelivr.net/gh/Scighost/KeqingNiuza@cdn/ResourceFileList_Debug.json";

#else

        private readonly string _FileListUrl = "https://cdn.jsdelivr.net/gh/Scighost/KeqingNiuza@cdn/UpdateFileList.json";

        private readonly string _ResourceListUrl = "https://cdn.jsdelivr.net/gh/Scighost/KeqingNiuza@cdn/ResourceFileList.json";

#endif




        private UpdateFileList _FileList;
        private ResourceFileList _ResourceList;

        public event EventHandler DownloadStarted;

        public event EventHandler OneFileDownloaded;

        public long DownloadSize { get; set; }

        public int AllFilesCount { get; set; }

        private int downloadedFilesCount;
        public int DownloadedFilesCount
        {
            get { return downloadedFilesCount; }
            set
            {
                downloadedFilesCount = value;
                OneFileDownloaded?.Invoke(this, null);
            }
        }




        public Updater()
        {
            HttpClient = new HttpClient();
            HttpClient.DefaultRequestHeaders.Add("User-Agent", _UserAgent);
        }


        public Updater(string fileListUrl)
        {
            _FileListUrl = fileListUrl;
            HttpClient = new HttpClient();
            HttpClient.DefaultRequestHeaders.Add("User-Agent", _UserAgent);
        }


        public async Task<(bool IsNeedUpdate, bool IsAutoUpdate)> GetUpdateInfo()
        {
            (bool IsNeedUpdate, bool IsAutoUpdate) result = (false, false);
            await Task.Run(async () =>
            {
                var fileListContent = await HttpClient.GetStringAsync(_FileListUrl);
                _FileList = JsonSerializer.Deserialize<UpdateFileList>(fileListContent, JsonOptions);
                if (_FileList.Version > Const.Version)
                {
                    result.IsNeedUpdate = true;
                    // 主次内版本号相同，修订号不同，则自动更新
                    if (_FileList.Version.ToString(3) == Const.Version.ToString(3))
                    {
                        result.IsAutoUpdate = true;
                    }
                }
            });
            return result;
        }


        /// <summary>
        /// 更新资源文件
        /// </summary>
        /// <returns>true代表需要唤醒更新程序，false无需更新</returns>
        public async Task<bool> UpdateResourceFiles()
        {
            bool result = false;
            await Task.Run(async () =>
             {
                 var resourceContent = await HttpClient.GetStringAsync(_ResourceListUrl);
                 _ResourceList = JsonSerializer.Deserialize<ResourceFileList>(resourceContent, JsonOptions);
                 var dir = new DirectoryInfo(".\\resource");
                 var files = dir.GetFiles("*.*", SearchOption.AllDirectories).ToList();
                 var localFiles = files.ConvertAll(x => new ResourceFileInfo(x));
                 var downloadFiles = _ResourceList.Files.Except(localFiles).ToList();
                 if (downloadFiles.Any())
                 {
                     DownloadSize = downloadFiles.Sum(x => x.Size);
                     downloadedFilesCount = 0;
                     AllFilesCount = downloadFiles.Count;
                     DownloadStarted?.Invoke(this, null);
                     Parallel.ForEach(downloadFiles, async file =>
                     {
                         var bytes = await HttpClient.GetByteArrayAsync(file.Url);
                         Directory.CreateDirectory(Path.GetDirectoryName(file.Path));
                         try
                         {
                             File.WriteAllBytes(file.Path, bytes);
                         }
                         catch (Exception ex)
                         {
                             var path = "update\\KeqingNiuza\\" + file.Path;
                             Directory.CreateDirectory(Path.GetDirectoryName(path));
                             File.WriteAllBytes(path, bytes);
                             result = true;
                             Console.WriteLine(ex.Message);
                         }
                         lock (this)
                         {
                             DownloadedFilesCount++;
                         }
                     });
                 }
                 else
                 {
                     result = false;
                 }
             });
            return result;
        }


        public async Task PrepareUpdateFiles()
        {
            await Task.Run(async () =>
            {
                var bytes = await HttpClient.GetByteArrayAsync(_FileList.PackageUrl);
                var name = $"update\\{_FileList.PackageName}";
                if (Directory.Exists(".\\update"))
                {
                    Directory.Delete(".\\update", true);
                }
                Directory.CreateDirectory(".\\update");
                File.WriteAllBytes(name, bytes);
                if (Directory.Exists($"{name.Replace(".zip", "")}"))
                {
                    Directory.Delete($"{name.Replace(".zip", "")}", true);
                }
                ZipFile.ExtractToDirectory(name, "update");
                if (_FileList.ShowUpdateLogView)
                {
                    File.Create("update\\ShowUpdateLog");
                }
            });
        }


        public void CallUpdateWhenExit()
        {
            Application.Current.Dispatcher.Invoke(() => Application.Current.Exit += CallUpdate);
        }


        private void CallUpdate(object sender, EventArgs e)
        {
            Process.Start("update\\Update.exe", "KeqingNiuza.Update");
            Log.OutputLog(LogType.Info, "Has called Update.exe");
        }


    }
}
