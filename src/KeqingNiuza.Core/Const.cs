using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace KeqingNiuza.Core
{
    internal class Const
    {
        public static JsonSerializerOptions JsonOptions { get; } = new JsonSerializerOptions() { Encoder = JavaScriptEncoder.Create(UnicodeRanges.All), WriteIndented = true };

        public static string UserDataPath { get; } = "..\\UserData";

        public static string LogPath { get; } = "..\\Log";
    }
}
