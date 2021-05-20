using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WebDav;
using static KeqingNiuza.CloudBackup.Const;


namespace KeqingNiuza.CloudBackup
{
    public class JianguoClient : CloudClient
    {



        private static readonly string _BaseAddress = "https://dav.jianguoyun.com/dav/";



        private readonly WebDavClient _WebDevClient;


        public JianguoClient(string username, string password)
        {
            UserName = username;
            Password = password;
            _WebDevClient = new WebDavClient(new WebDavClientParams()
            {
                BaseAddress = new Uri(_BaseAddress),
                Credentials = new System.Net.NetworkCredential(username, password)
            });
        }


        public override async Task<bool> ConfirmAccount()
        {
            var result = await _WebDevClient.Mkcol("KeqingNiuza");
            return result.IsSuccessful;
        }

        [Obsolete("因为同步逻辑问题暂时不使用")]
        public override async Task SyncFiles()
        {
            var result = await _WebDevClient.GetRawFile("KeqingNiuza/BackupFileList.json");
            var dirInfo = new DirectoryInfo(".\\UserData");
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
            await _WebDevClient.PutFile($"KeqingNiuza/{info.Name}", File.Open($"UserData\\{info.Name}", FileMode.Open, FileAccess.ReadWrite));
        }

        private async Task DownloasFile(BackupFileinfo info)
        {
            var result = _WebDevClient.GetRawFile($"KeqingNiuza/{info.Name}");
            var fileinfo = new FileInfo($"UserData\\{info.Name}");
            fileinfo.Delete();
            using (var stream = fileinfo.Create())
            {
                (await result).Stream.CopyTo(stream);
            }
            fileinfo.LastWriteTime = info.LastUpdateTime;
        }

        private async Task PutBackupFileList()
        {
            var dirInfo = new DirectoryInfo(".\\UserData");
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
                ZipFile.CreateFromDirectory(".\\UserData", name);
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
                entry.ExtractToFile("UserData\\" + entry.FullName, true);
            }
        }

        public override void SaveEncyptedAccount()
        {
            var account = new AccountInfo()
            {
                CloudType = CloudType.Jianguoyun,
                UserName = UserName,
                Password = Password,
                LastSyncTime = LastSyncTime
            };
            var json = JsonSerializer.Serialize(account, JsonOptions);
            var bytes = Endecryption.Encrypt(json);
            File.WriteAllBytes("UserData\\Account", bytes);
        }


    }
}
