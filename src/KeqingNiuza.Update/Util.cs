using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace KeqingNiuza.Update
{
    public static class Util
    {
        internal static bool MoveAllFile()
        {
            var result = true;
            var dir = "update\\KeqingNiuza\\";
            var list = Directory.GetFiles(dir, "*.*", SearchOption.AllDirectories);
            foreach (var file in list)
            {
                try
                {
                    var info = new FileInfo(file.Replace(dir, ""));
                    Directory.CreateDirectory(info.DirectoryName);
                    File.Copy(file, file.Replace(dir, ""), true);
                }
                catch (Exception ex)
                {
                    result = false;
                    Log.OutputLog(LogType.Fault, $"UpdateMove - {file.Replace(dir, "")}", ex);
                }
            }
            return result;
        }

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int MessageBox(int hWnd, string text, string caption, uint type);

    }
}
