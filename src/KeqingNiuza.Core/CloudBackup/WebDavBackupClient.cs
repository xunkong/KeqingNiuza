using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WebDav;
using static KeqingNiuza.Core.CloudBackup.Const;


namespace KeqingNiuza.Core.CloudBackup
{
    public class WebDavBackupClient : CloudClient
    {



        private readonly Uri _BaseAddress;



        private readonly WebDavClient _WebDevClient;


        public WebDavBackupClient(string username, string password, string url)
        {
            UserName = username;
            Password = password;
            if (!url.EndsWith("/"))
            {
                url += "/";
            }
            _BaseAddress = new Uri(url);
            if (url.Contains("jianguoyun"))
            {
                IsJianguo = true;
            }
            WebDavUrl = _BaseAddress.Host;
            _WebDevClient = new WebDavClient(new WebDavClientParams()
            {
                BaseAddress = _BaseAddress,
                Credentials = new System.Net.NetworkCredential(username, password)
            });
        }


        public override async Task<(bool isSuccessful, int code, string msg)> ConfirmAccount()
        {
            var result = await _WebDevClient.Mkcol("KeqingNiuza");
            return (result.IsSuccessful, result.StatusCode, result.Description);
        }

        [Obsolete("因为同步逻辑问题暂时不使用")]
        public override async Task SyncFiles()
        {
            var result = await _WebDevClient.GetRawFile("KeqingNiuza/BackupFileList.json");
            var dirInfo = new DirectoryInfo(UserDataPath);
            var localFiles = dirInfo.GetFiles().ToList().ConvertAll(x => new BackupFileinfo(x));
            if (result.StatusCode == 404)
            {
                result.Dispose();
                foreach (var item in localFiles)
                {
                    await UploadFile(item);
                }
                await PutBackupFileList();
                return;
            }

            StreamReader streamReader = new StreamReader(result.Stream);
            var json = streamReader.ReadToEnd();
            streamReader.Dispose();
            result.Dispose();
            var remoteFiles = JsonSerializer.Deserialize<List<BackupFileinfo>>(json, JsonOptions);

            var uploadFiles = localFiles.Where(x => remoteFiles.Any(y => x.Name == y.Name && x.LastUpdateTime > y.LastUpdateTime) || !remoteFiles.Any(y => x.Name == y.Name)).Select(x => x);

            var downloadFiles = remoteFiles.Where(x => localFiles.Any(y => x.Name == y.Name && x.LastUpdateTime > y.LastUpdateTime) || !localFiles.Any(y => x.Name == y.Name)).Select(x => x);

            foreach (var item in downloadFiles)
            {
                await DownloasFile(item);
            }
            foreach (var item in uploadFiles)
            {
                await UploadFile(item);
            }

            await PutBackupFileList();

            LastSyncTime = DateTime.Now;

            SaveEncyptedAccount();

        }


        private async Task UploadFile(BackupFileinfo info)
        {
            await _WebDevClient.PutFile($"KeqingNiuza/{info.Name}", File.Open($"{UserDataPath}\\{info.Name}", FileMode.Open, FileAccess.ReadWrite));
        }

        private async Task DownloasFile(BackupFileinfo info)
        {
            var result = _WebDevClient.GetRawFile($"KeqingNiuza/{info.Name}");
            var fileinfo = new FileInfo($"{UserDataPath}\\{info.Name}");
            fileinfo.Delete();
            using (var stream = fileinfo.Create())
            {
                (await result).Stream.CopyTo(stream);
            }
            fileinfo.LastWriteTime = info.LastUpdateTime;
        }

        private async Task PutBackupFileList()
        {
            var dirInfo = new DirectoryInfo(UserDataPath);
            var localFiles = dirInfo.GetFiles().ToList().ConvertAll(x => new BackupFileinfo(x));
            localFiles.RemoveAll(x => x.Name == "Account");
            var json = JsonSerializer.Serialize(localFiles, JsonOptions);
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                await _WebDevClient.PutFile("KeqingNiuza/BackupFileList.json", ms);
            }
        }

        public override async Task BackupFileArchive()
        {
            await Task.Run(async () =>
            {
                var date = DateTime.Now.ToString("yyMMddHH");
                var name = $"UserData_{date}.zip";
                File.Delete(name);
                ZipFile.CreateFromDirectory(UserDataPath, name);
                using (var archive = ZipFile.Open(name, ZipArchiveMode.Update))
                {
                    var entry = archive.GetEntry("Account");
                    entry?.Delete();
                }
                _ = _WebDevClient.Mkcol("KeqingNiuza/Archive").Result;
                var result = await _WebDevClient.PutFile($"KeqingNiuza/Archive/{name}", File.OpenRead(name));
                File.Delete(name);
                if (!result.IsSuccessful)
                {
                    throw new Exception(result.Description);
                }
                await PutBackupFileList();
                LastSyncTime = DateTime.Now;
                SaveEncyptedAccount();
            });
        }


        public override async Task RestoreFileArchive()
        {
            var propfindResponse = await _WebDevClient.Propfind("KeqingNiuza/Archive");
            if (!propfindResponse.IsSuccessful)
            {
                throw new Exception(propfindResponse.Description);
            }
            if (propfindResponse.Resources.Count < 2)
            {
                throw new Exception("没有已备份的数据");
            }
            int date = 0;
            foreach (var resource in propfindResponse.Resources)
            {
                if (resource.DisplayName.Contains("UserData"))
                {
                    var id = int.Parse(resource.DisplayName.Replace("UserData_", "").Replace(".zip", ""));
                    if (id > date)
                    {
                        date = id;
                    }
                }
            }
            var url = $"KeqingNiuza/Archive/UserData_{date}.zip";
            var streamResponse = await _WebDevClient.GetRawFile(url);
            if (!streamResponse.IsSuccessful)
            {
                throw new Exception(streamResponse.Description);
            }
            var zipArchive = new ZipArchive(streamResponse.Stream);
            var entries = zipArchive.Entries;
            foreach (var entry in entries)
            {
                entry.ExtractToFile($"{UserDataPath}\\" + entry.FullName, true);
            }
        }

        public override void SaveEncyptedAccount()
        {
            var account = new AccountInfo()
            {
                Url = _BaseAddress.OriginalString,
                CloudType = CloudType.WebDav,
                UserName = UserName,
                Password = Password,
                LastSyncTime = LastSyncTime
            };
            var json = JsonSerializer.Serialize(account, JsonOptions);
            var bytes = Endecryption.Encrypt(json);
            File.WriteAllBytes($"{UserDataPath}\\Account", bytes);
        }


    }
}
