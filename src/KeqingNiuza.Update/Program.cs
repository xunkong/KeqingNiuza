using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;

namespace KeqingNiuza.Update
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                return;
            }
            if (args[0] == "ExportFileList")
            {
                Util.ExportFileList();
                return;
            }
            if (args[0] == "KeqingNiuza.Update")
            {
                Thread.Sleep(2000);
                var proes = Process.GetProcessesByName("刻记牛杂店");
                if (proes.Any())
                {
                    Array.ForEach(proes, x => x.Kill());
                }
                Thread.Sleep(2000);
                var str = File.ReadAllText(@"Update\UpdatedFileList.json");
                var list = JsonSerializer.Deserialize<UpdatedFileList>(str);
                var sourceDirPath = list.SourceDirPath;
                list.IsUpdateFinished = true;
                foreach (var item in list.UpdatedFiles)
                {
                    try
                    {
                        if (item.Mode == 1)
                        {
                            File.Copy(Path.Combine(sourceDirPath, item.Path), item.Path, true);
                        }
                        if (item.Mode == -1)
                        {
                            File.Delete(item.Path);
                        }
                    }
                    catch (Exception e)
                    {
                        list.IsUpdateFinished = false;
                        //list.FailedFiles.Add(item);
                    }
                }
                //var diffList = Util.VerifyFiles();
                //if (diffList.Any())
                //{
                //    list.FailedFiles.AddRange(diffList);
                //    list.FailedFiles.Distinct();
                //}
                //else
                //{
                //    list.IsUpdateFinished = true;
                //}
                str = JsonSerializer.Serialize(list, Util.JsonOptions);
                File.WriteAllText("Update\\UpdatedFileList.json", str);
                //Directory.Delete(".\\Update", true);
                //if (args[1] == "Restart")
                //{
                //    Process.Start("Genshin Helper.exe");
                //}

                Directory.Delete(".\\Update", true);
            }
        }
    }
}
