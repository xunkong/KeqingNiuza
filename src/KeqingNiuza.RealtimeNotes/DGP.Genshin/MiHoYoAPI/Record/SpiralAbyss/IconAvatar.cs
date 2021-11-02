using Newtonsoft.Json;

namespace DGP.Genshin.MiHoYoAPI.Record.SpiralAbyss
{
    /// <summary>
    /// 仅包含头像的角色信息
    /// </summary>
    public class IconAvatar
    {
        [JsonProperty("id")] public int Id { get; set; }
        [JsonProperty("icon")] public string Icon { get; set; }
        [JsonProperty("level")] public int Level { get; set; }
        [JsonProperty("rarity")] public int Rarity { get; set; }
    }
}
