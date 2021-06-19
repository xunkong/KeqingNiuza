using System.Collections.Generic;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace KeqingNiuza.Wish
{
    public static class Const
    {
        public static List<WishEvent> WishEventList => LoadWishEventList();
        public static List<CharacterInfo> CharacterInfoList => LoadCharacterInfoList();
        public static List<WeaponInfo> WeaponInfoList => LoadWeaponInfoList();
        public static JsonSerializerOptions JsonOptions { get; set; }
        static Const()
        {
            JsonOptions = new JsonSerializerOptions() { Encoder = JavaScriptEncoder.Create(UnicodeRanges.All) };
        }

        public static readonly List<string> BrushList = new List<string>
        {
            "#FF0000",
            "#FF1493",
            "#FF7F50",
            "#FFB61E",
            "#402E4C",
            "#8C531B",
            "#a88462",
            "#008B8B",
            "#228B22",
            "#789262",
            "#7BBFEA",
            "#4169E1",
            "#0000FF",
            "#B0A4E3",
            "#800080",
        };

        public static List<WishEvent> LoadWishEventList()
        {
            var json = File.ReadAllText("resource\\list\\WishEventList.json");
            return JsonSerializer.Deserialize<List<WishEvent>>(json, JsonOptions);
        }

        public static List<CharacterInfo> LoadCharacterInfoList()
        {
            var json = File.ReadAllText("resource\\list\\CharacterInfoList.json");
            return JsonSerializer.Deserialize<List<CharacterInfo>>(json, JsonOptions);
        }

        public static List<WeaponInfo> LoadWeaponInfoList()
        {
            var json = File.ReadAllText("resource\\list\\WeaponInfoList.json");
            return JsonSerializer.Deserialize<List<WeaponInfo>>(json, JsonOptions);
        }
    }
}
