using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace KeqingNiuza.Core.CloudBackup
{
    class Const
    {
        public static JsonSerializerOptions JsonOptions = new JsonSerializerOptions() { Encoder = JavaScriptEncoder.Create(UnicodeRanges.All) };

        public static string UserDataPath { get; } = "..\\UserData";
    }
}
