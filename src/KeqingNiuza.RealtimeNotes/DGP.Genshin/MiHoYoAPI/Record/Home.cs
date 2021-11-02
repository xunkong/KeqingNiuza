using Newtonsoft.Json;

namespace DGP.Genshin.MiHoYoAPI.Record
{
    /// <summary>
    /// 家园信息
    /// </summary>
    public class Home
    {
        [JsonProperty("level")] public int Level { get; set; }
        [JsonProperty("visit_num")] public int VisitNum { get; set; }
        [JsonProperty("comfort_num")] public int ComfortNum { get; set; }
        [JsonProperty("item_num")] public int ItemNum { get; set; }
        [JsonProperty("name")] public string Name { get; set; }
        [JsonProperty("icon")] public string Icon { get; set; }
        [JsonProperty("comfort_level_name")] public string ComfortLevelName { get; set; }
        [JsonProperty("comfort_level_icon")] public string ComfortLevelIcon { get; set; }
    }
}
