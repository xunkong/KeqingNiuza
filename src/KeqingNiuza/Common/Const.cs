using KeqingNiuza.Wish;
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
        public static readonly Version Version = new Version(0, 1, 2, 21050810);

        public static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions() { Encoder = JavaScriptEncoder.Create(UnicodeRanges.All) };

        public static List<string> AvatarList { get; set; }

        public static WishEvent ZeroWishEvent { get; set; }

        static Const()
        {
            AvatarList = JsonSerializer.Deserialize<List<string>>(File.ReadAllText("Resource\\List\\AvatarList.json"), JsonOptions);
            ZeroWishEvent = new WishEvent
            {
                Name = "---",
                StartTime = new DateTime(2020, 9, 15, 0, 0, 0, DateTimeKind.Local),
                EndTime = DateTime.Now,
                UpStar5 = new List<string>(),
                UpStar4 = new List<string>()
            };
        }
    }
}
