using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeqingNiuza.Update
{
    static class Log
    {
        internal static void OutputLog(LogType type, Exception ex)
        {
            Directory.CreateDirectory(".\\Log");
            var fileName = $@"Log\\log_{DateTime.Now:yyMMdd}.txt";
            var str = $"[{type}][{DateTime.Now:yyMMdd.HHmmss.fff}]\r\n{ex}\r\n\r\n";
            File.AppendAllText(fileName, str);
        }


        internal static void OutputLog(LogType type, string content)
        {
            Directory.CreateDirectory(".\\Log");
            var fileName = $@"Log\\log_{DateTime.Now:yyMMdd}.txt";
            var str = $"[{type}][{DateTime.Now:yyMMdd.HHmmss.fff}]\r\n{content}\r\n\r\n";
            File.AppendAllText(fileName, str);

        }

        internal static void OutputLog(LogType type, string step, Exception ex)
        {
            Directory.CreateDirectory(".\\Log");
            var fileName = $@"Log\\log_{DateTime.Now:yyMMdd}.txt";
            var str = $"[{type}][{DateTime.Now:yyMMdd.HHmmss.fff}][{step}]\r\n{ex}\r\n\r\n";
            File.AppendAllText(fileName, str);
        }
    }
}
