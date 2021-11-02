using Newtonsoft.Json;

namespace DGP.Genshin.MiHoYoAPI.User
{
    public class UserGameRole
    {
        [JsonProperty("game_biz")] public string GameBiz { get; set; }
        [JsonProperty("region")] public string Region { get; set; }
        [JsonProperty("game_uid")] public string GameUid { get; set; }
        [JsonProperty("nickname")] public string Nickname { get; set; }
        [JsonProperty("level")] public int Level { get; set; }
        [JsonProperty("is_chosen")] public string IsChosen { get; set; }
        [JsonProperty("region_name")] public string RegionName { get; set; }
        [JsonProperty("is_official")] public string IsOfficial { get; set; }

        /// <summary>
        /// 重写的方法
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{Nickname} | {Level}级 | {RegionName}";
        }
    }
}
