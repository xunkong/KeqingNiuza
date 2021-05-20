using System.Collections.Generic;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace KeqingNiuza.Wish
{
    static class Const
    {
        public static Dictionary<string, string> ElementDictionary { get; set; }
        public static List<WishEvent> WishEventList { get; set; }
        public static JsonSerializerOptions JsonOptions { get; set; }
        static Const()
        {
            JsonOptions = new JsonSerializerOptions() { Encoder = JavaScriptEncoder.Create(UnicodeRanges.All) };
            var json = File.ReadAllText("resource\\list\\ElementDictionary.json");
            ElementDictionary = JsonSerializer.Deserialize<Dictionary<string, string>>(json, JsonOptions);
            json = File.ReadAllText("resource\\list\\WishEventList.json");
            WishEventList = JsonSerializer.Deserialize<List<WishEvent>>(json, JsonOptions);
        }
    }
}
