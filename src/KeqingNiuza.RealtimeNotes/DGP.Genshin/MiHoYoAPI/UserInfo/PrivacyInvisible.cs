using Newtonsoft.Json;

namespace DGP.Genshin.MiHoYoAPI.UserInfo
{
    public class PrivacyInvisible
    {
        [JsonProperty("post")] public bool Post { get; set; }
        [JsonProperty("collect")] public bool Collect { get; set; }
        [JsonProperty("watermark")] public bool Watermark { get; set; }
        [JsonProperty("reply")] public bool Reply { get; set; }
        [JsonProperty("post_and_instant")] public bool PostAndInstant { get; set; }
    }

}
