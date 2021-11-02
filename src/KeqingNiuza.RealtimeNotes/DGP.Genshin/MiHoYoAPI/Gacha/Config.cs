using Newtonsoft.Json;
using System.Collections.Generic;

namespace DGP.Genshin.MiHoYoAPI.Gacha
{
    /// <summary>
    /// 奖池配置信息
    /// </summary>
    public class Config
    {
        [JsonProperty("gacha_type_list")] public List<ConfigType> Types { get; set; }
        [JsonProperty("region")] public string Region { get; set; }
    }
}
