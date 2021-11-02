using Newtonsoft.Json;
using System.Collections.Generic;

namespace DGP.Genshin.MiHoYoAPI.UserInfo
{
    public class UserInfo
    {
        [JsonProperty("uid")] public string Uid { get; set; }
        [JsonProperty("nickname")] public string Nickname { get; set; }
        [JsonProperty("introduce")] public string Introduce { get; set; }
        [JsonProperty("avatar")] public string Avatar { get; set; }
        [JsonProperty("gender")] public int Gender { get; set; }
        [JsonProperty("certification")] public Certification Certification { get; set; }
        [JsonProperty("level_exps")] public List<LevelExp> LevelExps { get; set; }
        [JsonProperty("achieve")] public Achieve Achieve { get; set; }
        [JsonProperty("community_info")] public CommunityInfo CommunityInfo { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        [JsonProperty("avatar_url")] public string AvatarUrl { get; set; }
        [JsonProperty("certifications")] public List<string> Certifications { get; set; }
        [JsonProperty("level_exp")] public LevelExp LevelExp { get; set; }
        /// <summary>
        /// 头像框
        /// </summary>
        [JsonProperty("pendant")] public string Pendant { get; set; }
    }
}
