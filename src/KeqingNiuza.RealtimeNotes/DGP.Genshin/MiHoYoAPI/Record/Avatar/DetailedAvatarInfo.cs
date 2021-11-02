using Newtonsoft.Json;
using System.Collections.Generic;

namespace DGP.Genshin.MiHoYoAPI.Record.Avatar
{
    /// <summary>
    /// 包装详细角色信息列表
    /// </summary>
    public class DetailedAvatarInfo
    {
        [JsonProperty("avatars")] public List<DetailedAvatar> Avatars { get; set; }
    }
}
