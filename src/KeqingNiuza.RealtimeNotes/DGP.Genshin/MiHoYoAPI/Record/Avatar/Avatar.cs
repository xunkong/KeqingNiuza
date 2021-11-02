using Newtonsoft.Json;

namespace DGP.Genshin.MiHoYoAPI.Record.Avatar
{
    /// <summary>
    /// 包含一个角色的基础信息
    /// </summary>
    public class Avatar
    {
        [JsonProperty("id")] public int Id { get; set; }
        [JsonProperty("image")] public string Image { get; set; }
        [JsonProperty("name")] public string Name { get; set; }
        [JsonProperty("element")] public string Element { get; set; }
        [JsonProperty("fetter")] public int Fetter { get; set; }
        [JsonProperty("level")] public int Level { get; set; }
        [JsonProperty("rarity")] public int Rarity { get; set; }
        [JsonProperty("actived_constellation_num")] public int ActivedConstellationNum { get; set; }
    }
}
