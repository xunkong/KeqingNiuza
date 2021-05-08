using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace KeqingNiuza.Update
{
    public static class Util
    {
        private static readonly SHA256 SHA256 = SHA256.Create();

        private static readonly string packageBaseUrl = @"https://cdn.jsdelivr.net/gh/Scighost/KeqingNiuza@cdn/KeqingNiuza/";

        public static JsonSerializerOptions JsonOptions { get; set; } = new JsonSerializerOptions() { Encoder = JavaScriptEncoder.Create(UnicodeRanges.All) };



        public static string GetFileHash(string path)
        {
            var stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            byte[] data = new byte[stream.Length];
            stream.Read(data, 0, (int)stream.Length);
            stream.Dispose();
            var bytes = SHA256.ComputeHash(data);
            return BitConverter.ToString(bytes).Replace("-", "");
        }



        public static List<UpdatedFileInfo> VerifyFiles()
        {
            var json = File.ReadAllText("Resource\\FileList.json");
            var fileList = JsonSerializer.Deserialize<FileList>(json, JsonOptions);
            var allFiles = fileList.AllFiles;
            var differentFiles = new List<UpdatedFileInfo>();
            foreach (var item in allFiles)
            {
                if (item.SHA256 != GetFileHash(item.Path))
                {
                    differentFiles.Add(item);
                }
            }
            return differentFiles;
        }


        public static void ExportFileList()
        {
            var dirinfo = new DirectoryInfo(".\\");
            string dir = dirinfo.FullName;
            var fileinfos = dirinfo.GetFiles("*.*", SearchOption.AllDirectories).ToList();
            fileinfos.RemoveAll(x => x.Name.Contains("FileList"));
            var allFiles = new List<UpdatedFileInfo>(fileinfos.Count);
            foreach (var file in fileinfos)
            {
                var info = new UpdatedFileInfo()
                {
                    Name = file.Name,
                    Path = file.FullName.Replace(dir, ""),
                    SHA256 = GetFileHash(file.FullName)
                };
                allFiles.Add(info);
            }
            FileList fileList;
            if (File.Exists("..\\FileList.json"))
            {
                var str = File.ReadAllText("..\\FileList.json");
                fileList = JsonSerializer.Deserialize<FileList>(str, JsonOptions);
                // 查找需要更新的文件
                var changelist = allFiles.Except(fileList.AllFiles).ToList().ConvertAll(x => new UpdatedFileInfo(x));
                changelist.ForEach(x => x.Mode = 1);
                var deletelist = fileList.AllFiles.Except(allFiles).ToList().ConvertAll(x => new UpdatedFileInfo(x));
                deletelist = deletelist.Where(x => !changelist.Any(y => x.Path == y.Path)).Select(x => x).ToList();
                deletelist.ForEach(x => x.Mode = -1);
                var updatedlist = Enumerable.Union(changelist, deletelist).ToList();
                if (!updatedlist.Any())
                {
                    return;
                }
                var newversion = new Version(0, 1, 2, int.Parse(DateTime.Now.ToString("yyMMddHH")));
                var oldversion = fileList.Version;
                // 避免同一时间多次构建时不更新FileList.json
                if (newversion > oldversion)
                {
                    File.Copy("..\\FileList.json", $"..\\FileList_{oldversion}.json", true);
                    fileList.VersionHistory.Add(new FileList(fileList));
                    fileList.Version = newversion;
                }
                fileList.AllFiles = allFiles;
                fileList.UpdateTime = DateTime.Now;
                fileList.UpdatedFiles = updatedlist;
            }
            else
            {
                fileList = new FileList()
                {
                    Version = new Version(0, 1, 0, int.Parse(DateTime.Now.ToString("yyMMddHH"))),
                    UpdateTime = DateTime.Now,
                    AllFiles = allFiles
                };
            }
            var zipPath = $"..\\KeqingNiuza_{fileList.Version}.zip";
            if (File.Exists(zipPath))
            {
                File.Delete(zipPath);
            }
            ZipFile.CreateFromDirectory(".\\", zipPath);
            var zipArchive = ZipFile.Open(zipPath, ZipArchiveMode.Update);
            var entry = zipArchive.GetEntry("Resource/FileList.json");
            entry?.Delete();
            zipArchive.Dispose();
            File.Copy(zipPath, "..\\KeqingNiuza.zip", true);
            fileList.PackageUrl = $"{packageBaseUrl}KeqingNiuza_{fileList.Version}.zip";
            fileList.PackageName = $"KeqingNiuza_{fileList.Version}.zip";
            fileList.PackageSHA256 = GetFileHash(zipPath);
            File.WriteAllText("..\\FileList.json", JsonSerializer.Serialize(fileList, JsonOptions));
        }
    }
}
