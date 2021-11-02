using Newtonsoft.Json;
using System.Collections.Generic;

namespace DGP.Genshin.MiHoYoAPI.Record
{
    /// <summary>
    /// 玩家信息
    /// </summary>
    public class PlayerInfo
    {
        [JsonProperty("avatars")] public List<Avatar.Avatar> Avatars { get; set; }
        [JsonProperty("stats")] public PlayerStats PlayerStat { get; set; }
        [JsonProperty("world_explorations")] public List<WorldExploration> WorldExplorations { get; set; }
        [JsonProperty("homes")] public List<Home> Homes { get; set; } = new List<Home>();
    }
}
