using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Timers;
using Newtonsoft.Json;

namespace KeqingNiuza.Launcher
{
    static class Util
    {

        private static SHA256 _SHA256;

        private static readonly string jsDelivrUrl = "https://cdn.jsdelivr.net/gh/Scighost/KeqingNiuza@";

        private static readonly string fastgitUrl = "https://raw.fastgit.org/Scighost/KeqingNiuza/";

        private static readonly string qcloudUrl = "https://xw6dp97kei-1306705684.file.myqcloud.com/keqingniuza/";


        public static string HashData(byte[] bytes)
        {
            if (_SHA256 == null)
            {
                _SHA256 = new SHA256Managed();
            }
            var hash = _SHA256.ComputeHash(bytes);
            return BitConverter.ToString(hash).Replace("-", "");
        }


        public static async Task DeleteSelf(string path, int processId)
        {
            if (!Directory.Exists(path))
            {
                return;
            }
            var process = Process.GetProcessById(processId);
            if (process == null)
            {
                await Task.Delay(1000);
                Directory.Delete(path, true);
            }
            else
            {
                process.WaitForExit();
                await Task.Delay(1000);
                Directory.Delete(path, true);
            }
        }

        /// <summary>
        /// 一定时间后强行结束此进程
        /// </summary>
        /// <param name="interval">毫秒</param>
        public static void CountdownToShutdown(double interval = 15000)
        {
            Timer timer = new Timer(interval);
            timer.Elapsed += (s, e) => Environment.Exit(-1);
            timer.Start();
        }


        public static void OutputLog(Exception ex)
        {
            Directory.CreateDirectory(".\\Log");
            var str = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} | Launcher {MetaData.FileVersion}]\n{ex}\n\n";
            var name = $".\\Log\\launcher-{DateTime.Now:yyMMdd}.txt";
            File.AppendAllText(name, str);
        }


        public static void Deploy()
        {
            if (Directory.Exists(".\\cdn2"))
            {
                Directory.Delete(".\\cdn2", true);
            }
            Directory.CreateDirectory(".\\cdn2");
            var files = Directory.GetFiles(".\\", "*", SearchOption.AllDirectories);
            var infos = files.Select(x => KeqingNiuzaFileInfo.Create(x)).ToList();
            foreach (var info in infos)
            {
                var path = $@"cdn2/obj/{info.SHA256[0]}/{info.SHA256}";
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                info.Url = fastgitUrl + path;
                info.Url_CN = fastgitUrl + path;
                info.Url_OS = jsDelivrUrl + path;
                using (var fs = File.OpenRead(info.Path))
                {
                    using (var dfs = File.Create(path))
                    {
                        using (var cs = new DeflateStream(dfs, CompressionMode.Compress, true))
                        {
                            fs.CopyTo(cs);
                        }
                        info.CompressedSize = dfs.Length;
                    }
                }
            }
            var ver = FileVersionInfo.GetVersionInfo(".\\KeqingNiuza Launcher.exe");
            var version = new VersionInfo
            {
                Version = ver.ProductVersion,
                LastUpdateTime = DateTime.Now,
                KeqingNiuzaFiles = infos,
            };
            var json = JsonConvert.SerializeObject(version, Formatting.Indented);
            var compressingBytes = System.Text.Encoding.UTF8.GetBytes(json);
            Directory.CreateDirectory(".\\cdn2\\meta");
            File.WriteAllText(".\\cdn2\\meta\\version.json", json);
            using (var fs = File.Create(".\\cdn2\\meta\\version"))
            {
                using (var dfs = new DeflateStream(fs, CompressionMode.Compress))
                {
                    dfs.Write(compressingBytes, 0, compressingBytes.Length);
                }
            }
            var rootFiles = Directory.GetFiles(".\\", "*", SearchOption.TopDirectoryOnly);
            using (var fs = File.Create(".\\cdn2\\meta\\KeqingNiuza.zip"))
            {
                using (var zip = new ZipArchive(fs, ZipArchiveMode.Create))
                {
                    foreach (var file in rootFiles)
                    {
                        ZipFileExtensions.CreateEntryFromFile(zip, file, "KeqingNiuza" + file.Substring(1));
                    }
                }
            }
        }


        public static void DeployWallpaper()
        {
            if (Directory.Exists(".\\cdn"))
            {
                Directory.Delete(".\\cdn", true);
            }
            Directory.CreateDirectory(".\\cdn");
            var files = Directory.GetFiles(".\\wallpaper", "*", SearchOption.AllDirectories);
            var infos = files.Select(x => KeqingNiuzaFileInfo.Create(x)).ToList();
            foreach (var info in infos)
            {
                var path = $@"cdn/wallpaper/{info.SHA256}";
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                info.Url = fastgitUrl + path;
                info.Url_CN = fastgitUrl + path;
                info.Url_OS = jsDelivrUrl + path;
                using (var fs = File.OpenRead(info.Path))
                {
                    using (var dfs = File.Create(path))
                    {
                        using (var cs = new DeflateStream(dfs, CompressionMode.Compress, true))
                        {
                            fs.CopyTo(cs);
                        }
                        info.CompressedSize = dfs.Length;
                    }
                }
            }
            var json = JsonConvert.SerializeObject(infos, Formatting.Indented);
            File.WriteAllText(".\\cdn\\wallpaper.json", json);
        }


        [DllImport("user32.dll", SetLastError = true)]
        public static extern void MessageBox(IntPtr hwnd, string test, string caption, uint type);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern void SwitchToThisWindow(IntPtr hWnd, bool fUnknown);

    }
}
