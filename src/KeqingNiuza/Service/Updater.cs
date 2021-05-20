using KeqingNiuza.Common;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using static KeqingNiuza.Common.Const;

namespace KeqingNiuza.Service
{
    class Updater
    {
        private readonly HttpClient HttpClient;

        private const string _UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/89.0.4389.114 Safari/537.36 Edg/89.0.774.68";


        private readonly string _FileListUrl = "https://cdn.jsdelivr.net/gh/Scighost/KeqingNiuza@cdn/UpdateFileList.json";

        private readonly string _ResourceListUrl = "https://cdn.jsdelivr.net/gh/Scighost/KeqingNiuza@cdn/ResourceFileList.json";


        private UpdateFileList _FileList;
        private ResourceFileList _ResourceList;


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
                //todo 版本检测
                if (_FileList.Version > Const.Version)
                {
                    result = (true, _FileList.AutoUpdate);
                }
                else
                {
                    result = (false, false);
                }
            });
            return result;
        }


        /// <summary>
        /// 更新资源文件
        /// </summary>
        /// <returns>true更新成功，false无需更新</returns>
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
                 var downloadFiles = _ResourceList.Files.Except(localFiles);
                 if (downloadFiles.Any())
                 {
                     foreach (var file in downloadFiles)
                     {
                         var bytes = await HttpClient.GetByteArrayAsync(file.Url);
                         File.WriteAllBytes(file.Path, bytes);
                     }
                     result = true;
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




    }
}
