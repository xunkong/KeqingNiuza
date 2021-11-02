using Newtonsoft.Json;
using System.Collections.Generic;

namespace DGP.Genshin.MiHoYoAPI.UserInfo
{
    public class UserInfoWrapper
    {
        [JsonProperty("user_info")] public UserInfo UserInfo { get; set; }
        [JsonProperty("follow_relation")] public string FollowRelation { get; set; }
        [JsonProperty("auth_relations")] public List<string> AuthRelations { get; set; }
        [JsonProperty("is_in_blacklist")] public bool IsInBlacklist { get; set; }
        [JsonProperty("is_has_collection")] public bool IsHasCollection { get; set; }
        [JsonProperty("is_creator")] public bool IsCreator { get; set; }
    }
}
