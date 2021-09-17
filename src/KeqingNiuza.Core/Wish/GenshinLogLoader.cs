using System;
using System.IO;
using System.Text.RegularExpressions;

namespace KeqingNiuza.Core.Wish
{
    public class GenshinLogLoader
    {
        /// <summary>
        /// 从日志查找祈愿记录Url
        /// </summary>
        /// <exception cref="Exception">没有找到 Url</exception>
        /// <returns></returns>
        public static string FindUrlFromLogFile(bool isOversea = false)
        {
            string relativePath = isOversea ? @"AppData\LocalLow\miHoYo\Genshin Impact\output_log.txt"
                                            : @"AppData\LocalLow\miHoYo\原神\output_log.txt";
            string userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string sourceFile = Path.Combine(userProfile, relativePath);
            var stream = File.Open(sourceFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            var reader = new StreamReader(stream);
            var log = reader.ReadToEnd();
            var matches = Regex.Matches(log, @"OnGetWebViewPageFinish:.+#/log");
            if (matches.Count == 0)
            {
                throw new Exception("没有找到祈愿记录网址");
            }
            else
            {
                return matches[matches.Count - 1].Value.Replace("OnGetWebViewPageFinish:", "");
            }
        }
    }
}
