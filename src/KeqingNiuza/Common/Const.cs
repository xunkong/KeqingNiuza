using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace KeqingNiuza.Common
{
    static class Const
    {
        public static readonly Version Version = new Version(0, 1, 0, 21042520);

        public static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions() { Encoder = JavaScriptEncoder.Create(UnicodeRanges.All) };

        public static List<string> AvatarList { get; set; }

        static Const()
        {
            AvatarList = JsonSerializer.Deserialize<List<string>>(File.ReadAllText("Resource\\List\\AvatarList.json"), JsonOptions);
        }
    }
}
