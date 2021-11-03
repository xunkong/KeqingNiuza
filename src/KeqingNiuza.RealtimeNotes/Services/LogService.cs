using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeqingNiuza.RealtimeNotes.Services
{
    internal class LogService
    {
        public static void Log(string log)
        {
            Directory.CreateDirectory("..\\Log");
            var file = $"..\\Log\\note-{DateTime.Now:yyMMdd}.txt";
            var content = $"[{DateTime.Now:yy-MM-dd HH:mm:ss.fff}]\n{log}\n";
            File.AppendAllText(file, content);
        }
    }
}
