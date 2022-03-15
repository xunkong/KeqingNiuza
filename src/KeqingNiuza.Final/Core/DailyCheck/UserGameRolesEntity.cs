using System.Collections.Generic;
using Newtonsoft.Json;

namespace KeqingNiuza.Core.DailyCheck
{
    /// <summary>
    /// 游戏角色信息
    /// </summary>
    public class UserGameRolesEntity : RootEntity<UserGameRolesData>
    {

    }

    public class UserGameRolesData
    {
        [JsonProperty("list")]
        public List<UserGameRolesListItem> List { get; set; } = new List<UserGameRolesListItem>();
    }

    public class UserGameRolesListItem
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("game_biz")]
        public string GameBiz { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("region")]
        public string Region { get; set; }
        /// <summary>
        /// UID
        /// </summary>
        [JsonProperty("game_uid")]
        public string GameUid { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [JsonProperty("nickname")]
        public string Nickname { get; set; }
        /// <summary>
        /// 等级
        /// </summary>
        [JsonProperty("level")]
        public int Level { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("is_chosen")]
        public string IsChosen { get; set; }
        /// <summary>
        /// 区域
        /// </summary>
        [JsonProperty("region_name")]
        public string RegionName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("is_official")]
        public string IsOfficial { get; set; }

        public override string ToString()
        {
            return $"昵称:{Nickname},等级:{Level},区域:{RegionName}";
        }
    }
}
