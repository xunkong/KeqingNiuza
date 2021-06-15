using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static KeqingNiuza.Common.Const;

namespace KeqingNiuza.Common
{
    public static class Util
    {

        private static readonly SHA256 _hash = SHA256.Create();
        internal static string GetFileHash(string path)
        {
            var stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            byte[] data = new byte[stream.Length];
            stream.Read(data, 0, (int)stream.Length);
            stream.Dispose();
            var bytes = _hash.ComputeHash(data);
            return BitConverter.ToString(bytes).Replace("-", "");
        }

        private static readonly string packageBaseUrl = @"https://cdn.jsdelivr.net/gh/Scighost/KeqingNiuza@cdn/KeqingNiuza/";


        public static List<UpdateFileInfo> VerifyFiles()
        {
            var json = File.ReadAllText("resource\\UpdateFileList.json");
            var fileList = JsonSerializer.Deserialize<UpdateFileList>(json, JsonOptions);
            var allFiles = fileList.AllFiles;
            var differentFiles = new List<UpdateFileInfo>();
            foreach (var item in allFiles)
            {
                if (item.SHA256 != GetFileHash(item.Path))
                {
                    differentFiles.Add(item);
                }
            }
            return differentFiles;
        }


        public static void ExportUpdateFile()
        {
            if (File.Exists("..\\UpdateFileList.json"))
            {
                var str = File.ReadAllText("..\\UpdateFileList.json");
                var fileList = JsonSerializer.Deserialize<UpdateFileList>(str, JsonOptions);
                var version = fileList.Version;
                File.Copy("..\\UpdateFileList.json", $"..\\UpdateFileList_{version}.json", true);
                File.Delete("..\\UpdateFileList.json");
            }
            var filestrings = Directory.GetFiles(".\\", "*", SearchOption.AllDirectories);
            var zipFileList = filestrings.Where(x => !(x.Contains(".\\Log")
                                                        || x.Contains(".\\UserData")
                                                        || x.Contains(".\\update")
                                                        || x.Contains(".\\resource\\avatar")
                                                        || x.Contains(".\\resource\\character")
                                                        || x.Contains(".\\resource\\midi")
                                                        || x.Contains(".\\resource\\weapon")));

            // 最新版本压缩包
            var zipPath = $"..\\KeqingNiuza.zip";
            if (File.Exists(zipPath))
            {
                File.Delete(zipPath);
            }
            using (var archive = ZipFile.Open(zipPath, ZipArchiveMode.Update))
            {
                foreach (var item in zipFileList)
                {
                    archive.CreateEntryFromFile(item, "KeqingNiuza" + item.Remove(0, 1));
                }
                archive.CreateEntryFromFile("..\\Update.exe", "KeqingNiuza\\update\\Update.exe");
            }

            // 自动更新版本压缩包
            zipPath = $"..\\KeqingNiuza_{Const.Version}.zip";
            if (File.Exists(zipPath))
            {
                File.Delete(zipPath);
            }
            using (var archive = ZipFile.Open(zipPath, ZipArchiveMode.Update))
            {
                foreach (var item in zipFileList)
                {
                    archive.CreateEntryFromFile(item, "KeqingNiuza" + item.Remove(0, 1));
                }
                archive.CreateEntryFromFile("..\\Update.exe", "Update.exe");
            }
            var filelist = new UpdateFileList
            {
                Version = Const.Version,
                UpdateTime = DateTime.Now,
                PackageUrl = $"{packageBaseUrl}KeqingNiuza_{Const.Version}.zip",
                PackageName = $"KeqingNiuza_{Const.Version}.zip",
                PackageSHA256 = GetFileHash(zipPath)
            };
            filelist.AllFiles = zipFileList.ToList().ConvertAll(x => new UpdateFileInfo(x));
            File.WriteAllText("..\\UpdateFileList.json", JsonSerializer.Serialize(filelist, JsonOptions));
            File.Copy("..\\UpdateFileList.json", $"..\\UpdateFileList{Const.Version}.json", true);
        }


        public static void ExportResourceFile()
        {
            var list = new ResourceFileList();
            var files = Directory.GetFiles(".\\resource", "*.*", SearchOption.AllDirectories).ToList();
            list.Files = files.ConvertAll(x => new ResourceFileInfo(x));
            list.FileCount = files.Count;
            list.TotalSize = files.Sum(x => x.Length);
            list.LastUpdateTime = DateTime.Now;
            var json = JsonSerializer.Serialize(list, JsonOptions);
            File.WriteAllText("..\\ResourceFileList.json", json);
            foreach (var x in files)
            {
                Directory.CreateDirectory(Path.GetDirectoryName("." + x));
                File.Copy(x, "." + x, true);
            }
        }


    }
}
