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
        internal static void MoveAllFile()
        {
            var dir = "update\\Release\\";
            var list = Directory.GetFiles(dir, "*.*", SearchOption.AllDirectories);
            foreach (var file in list)
            {
                try
                {
                    File.Copy(file, file.Replace(dir, ""), true);
                }
                catch (Exception ex)
                {
                    Log.OutputLog(LogType.Fault, "UpdateMove", ex);
                }
            }
        }

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int MessageBox(int hWnd, String text, String caption, uint type);

    }
}
