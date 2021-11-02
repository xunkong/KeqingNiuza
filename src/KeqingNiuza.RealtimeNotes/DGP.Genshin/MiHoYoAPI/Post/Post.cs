using Newtonsoft.Json;

namespace DGP.Genshin.MiHoYoAPI.Post
{
    public class Post
    {
        [JsonProperty("post_id")] public string PostId { get; set; }
        [JsonProperty("subject")] public string Subject { get; set; }
        [JsonProperty("banner")] public string Banner { get; set; }
        [JsonProperty("official_type")] public int OfficialType { get; set; }
    }
}
