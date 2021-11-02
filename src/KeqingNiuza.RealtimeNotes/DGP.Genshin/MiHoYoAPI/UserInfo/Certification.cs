using Newtonsoft.Json;

namespace DGP.Genshin.MiHoYoAPI.UserInfo
{
    public class Certification
    {
        [JsonProperty("type")] public int Type { get; set; }
        [JsonProperty("label")] public string Label { get; set; }
    }
}
