using Newtonsoft.Json;

namespace DGP.Genshin.MiHoYoAPI.Record
{
    /// <summary>
    /// 供奉信息
    /// </summary>
    public class Offering
    {
        [JsonProperty("name")] public string Name { get; set; }
        [JsonProperty("level")] public string Level { get; set; }
    }
}
