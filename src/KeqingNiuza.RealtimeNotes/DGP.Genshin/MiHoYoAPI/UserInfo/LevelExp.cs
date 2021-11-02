using Newtonsoft.Json;

namespace DGP.Genshin.MiHoYoAPI.UserInfo
{
    public class LevelExp
    {
        [JsonProperty("level")] public int Level { get; set; }
        [JsonProperty("exp")] public int Exp { get; set; }
        [JsonProperty("game_id")] public int GameId { get; set; }
    }
}
