using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using static KeqingNiuza.Common.Const;

namespace KeqingNiuza.Common
{
    class Updater
    {
        private readonly HttpClient HttpClient;

        private const string _UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/89.0.4389.114 Safari/537.36 Edg/89.0.774.68";


        private readonly string _FileListUrl = "https://cdn.jsdelivr.net/gh/Scighost/KeqingNiuza@cdn/FileList.json";


        private FileList _FileList;


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


        public async Task<(bool IsUpdating, bool IsAutoUpdate)> GetUpdateInfo()
        {
            var fileListContent = await HttpClient.GetStringAsync(_FileListUrl);
            _FileList = JsonSerializer.Deserialize<FileList>(fileListContent, JsonOptions);
            if (_FileList.Version > Const.Version)
            {
                File.WriteAllText("Resource\\FileList.json", fileListContent);
                return (true, _FileList.AutoUpdate);
            }
            else
            {
                return (false, false);
            }
        }


        public async Task PrepareUpdatedFiles()
        {
            var bytes = await HttpClient.GetByteArrayAsync(_FileList.PackageUrl);
            var name = $"Update\\{_FileList.PackageName}";
            Directory.CreateDirectory(".\\Update");
            File.WriteAllBytes(name, bytes);
            if (Directory.Exists($"{name.Replace(".zip", "")}"))
            {
                Directory.Delete($"{name.Replace(".zip", "")}", true);
            }
            ZipFile.ExtractToDirectory(name, $"{name.Replace(".zip", "")}");
            UpdatedFileList updatedFileList = new UpdatedFileList
            {
                SourceDirPath = $"{name.Replace(".zip", "")}",
                ShowUpdateLogView = _FileList.ShowUpdateLogView
            };
            updatedFileList.UpdatedFiles.AddRange(_FileList.UpdatedFiles);
            foreach (var list in _FileList.VersionHistory)
            {
                if (list.Version > Const.Version)
                {
                    updatedFileList.UpdatedFiles.AddRange(list.UpdatedFiles.Where(x => x.Mode == -1).Select(x => x).ToList());
                }
            }
            updatedFileList.UpdatedFiles.Distinct();
            updatedFileList.UpdatedFiles.OrderBy(x => x.Mode);
            var updateFile = updatedFileList.UpdatedFiles.Find(x => x.Name == "Update.exe");
            if (updateFile != null)
            {
                File.Copy(Path.Combine(updatedFileList.SourceDirPath, updateFile.Path), updateFile.Path, true);
                updatedFileList.UpdatedFiles.Remove(updateFile);
            }
            var json = JsonSerializer.Serialize(updatedFileList, JsonOptions);
            File.WriteAllText("Update\\UpdatedFileList.json", json);
        }

        public bool IsDeleteUpdateDirectory()
        {
            if (!Directory.Exists(".\\Update"))
            {
                return false;
            }
            if (File.Exists("Update\\UpdatedFileList.json"))
            {
                var json = File.ReadAllText("Update\\UpdatedFileList.json");
                var list = JsonSerializer.Deserialize<UpdatedFileList>(json);
                return list.IsUpdateFinished;
            }
            else
            {
                return false;
            }
        }

    }
}
