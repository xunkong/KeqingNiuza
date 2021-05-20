using System.Collections.Generic;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace KeqingNiuza.Wish
{
    public static class Const
    {
        public static List<WishEvent> WishEventList { get; set; }
        public static List<CharacterInfo> CharacterInfoList { get; set; }
        public static List<WeaponInfo> WeaponInfoList { get; set; }
        public static JsonSerializerOptions JsonOptions { get; set; }
        static Const()
        {
            JsonOptions = new JsonSerializerOptions() { Encoder = JavaScriptEncoder.Create(UnicodeRanges.All) };
            var json = File.ReadAllText("resource\\list\\WishEventList.json");
            WishEventList = JsonSerializer.Deserialize<List<WishEvent>>(json, JsonOptions);
            json = File.ReadAllText("resource\\list\\CharacterInfoList.json");
            CharacterInfoList = JsonSerializer.Deserialize<List<CharacterInfo>>(json, JsonOptions);
            json = File.ReadAllText("resource\\list\\WeaponInfoList.json");
            WeaponInfoList = JsonSerializer.Deserialize<List<WeaponInfo>>(json, JsonOptions);
        }

        public static readonly List<string> BrushList = new List<string>
        {
            "#0000FF",
            "#A52A2A",
            "#FF7F50",
            "#DC143C",
            "#008B8B",
            "#8B008B",
            "#FF1493",
            "#B22222",
            "#228B22",
            "#800080",
            "#FF0000",
            "#4169E1",
        };
    }
}
