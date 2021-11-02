using Newtonsoft.Json;
using System.Collections.Generic;

namespace DGP.Genshin.MiHoYoAPI.Record.SpiralAbyss
{
    /// <summary>
    /// 深境螺旋信息
    /// </summary>
    public class SpiralAbyss
    {
        [JsonProperty("schedule_id")] public int ScheduleId { get; set; }
        [JsonProperty("start_time")] public string StartTime { get; set; }
        [JsonProperty("end_time")] public string EndTime { get; set; }
        [JsonProperty("total_battle_times")] public int TotalBattleTimes { get; set; }
        [JsonProperty("total_win_times")] public int TotalWinTimes { get; set; }
        [JsonProperty("max_floor")] public string MaxFloor { get; set; }
        [JsonProperty("reveal_rank")] public List<Rank> RevealRank { get; set; }
        [JsonProperty("defeat_rank")] public List<Rank> DefeatRank { get; set; }
        [JsonProperty("damage_rank")] public List<Rank> DamageRank { get; set; }
        [JsonProperty("take_damage_rank")] public List<Rank> TakeDamageRank { get; set; }
        [JsonProperty("normal_skill_rank")] public List<Rank> NormalSkillRank { get; set; }
        [JsonProperty("energy_skill_rank")] public List<Rank> EnergySkillRank { get; set; }
        [JsonProperty("floors")] public List<Floor> Floors { get; set; }
        [JsonProperty("total_star")] public int TotalStar { get; set; }
        [JsonProperty("is_unlock")] public string IsUnlock { get; set; }
    }
}
