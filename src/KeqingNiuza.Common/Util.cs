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

        private static readonly SHA256 _hash = System.Security.Cryptography.SHA256.Create();
        internal static string GetFileHash(string path)
        {
            var stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
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


        public static void ExportUpdateFileList()
        {
            if (File.Exists("..\\UpdateFileList.json"))
            {
                var str = File.ReadAllText("..\\UpdateFileList.json");
                var fileList = JsonSerializer.Deserialize<UpdateFileList>(str, JsonOptions);
                var version = fileList.Version;
                File.Copy("..\\UpdateFileList.json", $"..\\UpdateFileList_{version}.json", true);
                File.Delete("..\\UpdateFileList.json");
            }
            var zipPath = $"..\\KeqingNiuza.zip";
            if (File.Exists(zipPath))
            {
                File.Delete(zipPath);
            }
            ZipFile.CreateFromDirectory(".\\", zipPath);
            zipPath = $"..\\KeqingNiuza_{Const.Version}.zip";
            if (File.Exists(zipPath))
            {
                File.Delete(zipPath);
            }
            ZipFile.CreateFromDirectory(".\\", zipPath, CompressionLevel.Optimal, true);
            using (var archive = ZipFile.Open(zipPath, ZipArchiveMode.Update))
            {
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
            var dir = new DirectoryInfo(".\\");
            var files = dir.GetFiles("*.*", SearchOption.AllDirectories).ToList();
            filelist.AllFiles = files.ConvertAll(x => new UpdateFileInfo(x));
            File.WriteAllText("..\\UpdateFileList.json", JsonSerializer.Serialize(filelist, JsonOptions));
        }


        public static void ExportResourceFileList()
        {
            var list = new ResourceFileList();
            var dir = new DirectoryInfo(".\\resource");
            var files = dir.GetFiles("*.*", SearchOption.AllDirectories).ToList();
            list.Files = files.ConvertAll(x => new ResourceFileInfo(x));
            list.FileCount = files.Count;
            list.TotalSize = files.Sum(x => x.Length);
            list.LastUpdateTime = DateTime.Now;
            var json = JsonSerializer.Serialize(list, JsonOptions);
            File.WriteAllText("..\\ResourceFileList.json", json);
        }


    }
}
