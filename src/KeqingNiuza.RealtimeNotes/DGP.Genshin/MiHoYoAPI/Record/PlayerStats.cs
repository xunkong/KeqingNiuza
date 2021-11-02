using Newtonsoft.Json;

namespace DGP.Genshin.MiHoYoAPI.Record
{
    /// <summary>
    /// 玩家统计数据
    /// </summary>
    public class PlayerStats
    {
        [JsonProperty("active_day_number")] public int ActiveDayNumber { get; set; }
        [JsonProperty("achievement_number")] public int AchievementNumber { get; set; }

        [JsonProperty("anemoculus_number")] public int AnemoculusNumber { get; set; }
        [JsonProperty("geoculus_number")] public int GeoculusNumber { get; set; }
        [JsonProperty("electroculus_number")] public int ElectroculusNumber { get; set; }
        [JsonProperty("avatar_number")] public int AvatarNumber { get; set; }
        [JsonProperty("way_point_number")] public int WayPointNumber { get; set; }
        [JsonProperty("domain_number")] public int DomainNumber { get; set; }
        [JsonProperty("spiral_abyss")] public string SpiralAbyss { get; set; }

        [JsonProperty("luxurious_chest_number")] public int LuxuriousChestNumber { get; set; }
        [JsonProperty("precious_chest_number")] public int PreciousChestNumber { get; set; }
        [JsonProperty("exquisite_chest_number")] public int ExquisiteChestNumber { get; set; }
        [JsonProperty("common_chest_number")] public int CommonChestNumber { get; set; }
        /// <summary>
        /// 奇馈宝箱
        /// </summary>
        [JsonProperty("magic_chest_number")] public int MagicChestNumber { get; set; }
    }
}
