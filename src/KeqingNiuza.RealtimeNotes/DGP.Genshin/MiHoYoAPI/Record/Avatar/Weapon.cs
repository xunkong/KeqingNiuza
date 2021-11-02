using Newtonsoft.Json;

namespace DGP.Genshin.MiHoYoAPI.Record.Avatar
{
    /// <summary>
    /// 武器信息
    /// </summary>
    public class Weapon
    {
        [JsonProperty("id")] public int Id { get; set; }
        [JsonProperty("name")] public string Name { get; set; }
        [JsonProperty("icon")] public string Icon { get; set; }
        [JsonProperty("type")] public int Type { get; set; }
        [JsonProperty("rarity")] public int Rarity { get; set; }
        [JsonProperty("level")] public int Level { get; set; }
        [JsonProperty("promote_level")] public int PromoteLevel { get; set; }
        [JsonProperty("type_name")] public string TypeName { get; set; }
        [JsonProperty("desc")] public string Description { get; set; }
        /// <summary>
        /// it's refine Level actually
        /// </summary>
        [JsonProperty("affix_level")] public int AffixLevel { get; set; }
    }
}
