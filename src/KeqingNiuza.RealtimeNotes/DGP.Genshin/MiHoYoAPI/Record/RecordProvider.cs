using DGP.Genshin.Common.Request;
using DGP.Genshin.Common.Request.DynamicSecret;
using DGP.Genshin.MiHoYoAPI.Record.Avatar;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace DGP.Genshin.MiHoYoAPI.Record
{
    public class RecordProvider
    {
        private const string BaseUrl = @"https://api-takumi-record.mihoyo.com/game_record/app/genshin/api";
        private const string Referer = @"https://webstatic.mihoyo.com/app/community-game-records/index.html?v=6";

        private readonly Requester requester;

        /// <summary>
        /// 使用同一个提供器可用重复请求
        /// </summary>
        /// <param name="cookie"></param>
        public RecordProvider(string cookie)
        {
            requester = new Requester(new RequestOptions
            {
                {"Accept", RequestOptions.Json },
                {"x-rpc-app_version", DynamicSecretProvider2.AppVersion },
                {"User-Agent", RequestOptions.CommonUA2111 },
                {"x-rpc-client_type", "5" },
                {"Referer",Referer },
                {"Cookie", cookie },
                {"X-Requested-With", RequestOptions.Hyperion }
            });
        }

        /// <summary>
        /// 解析玩家服务器
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public string EvaluateUidRegion(string uid)
        {
            if (string.IsNullOrWhiteSpace(uid))
            {
                return null;
            }
            switch (uid[0])
            {
                case '1':
                case '2':
                case '3':
                case '4':
                    return "cn_gf01";
                case '5':
                    return "cn_qd01";
                case '6':
                    return "os_usa";
                case '7':
                    return "os_euro";
                case '8':
                    return "os_asia";
                case '9':
                    return "os_cht";
                default:
                    return null;
            }
        }

        /// <summary>
        /// 获取玩家基础信息
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="server"></param>
        /// <returns></returns>
        public async Task<PlayerInfo> GetPlayerInfoAsync(string uid, string server)
        {
            return await requester.GetWhileUpdateDynamicSecret2Async<PlayerInfo>(
                $@"{BaseUrl}/index?server={server}&role_id={uid}");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="server"></param>
        /// <param name="type">1：当期，2：上期</param>
        /// <returns></returns>
        public async Task<SpiralAbyss.SpiralAbyss> GetSpiralAbyssAsync(string uid, string server, int type)
        {
            return await requester.GetWhileUpdateDynamicSecret2Async<SpiralAbyss.SpiralAbyss>(
                $@"{BaseUrl}/spiralAbyss?schedule_type={type}&server={server}&role_id={uid}");
        }

        /// <summary>
        /// 获取玩家活动信息
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="server"></param>
        /// <returns></returns>
        public async Task<dynamic> GetActivitiesAsync(string uid, string server)
        {
            return await requester.GetWhileUpdateDynamicSecret2Async<dynamic>(
                $@"{BaseUrl}/activities?server={server}&role_id={uid}");
        }

        /// <summary>
        /// 获取玩家角色详细信息
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="server"></param>
        /// <param name="playerInfo">玩家的基础信息</param>
        /// <returns></returns>
        [SuppressMessage("", "IDE0050")]
        public async Task<DetailedAvatarInfo> GetDetailAvaterInfoAsync(string uid, string server, PlayerInfo playerInfo)
        {
            List<Avatar.Avatar> avatars = playerInfo.Avatars;

            var data = new
            {
                //but normally avatars will not be null
                character_ids = avatars is null ? new List<int>() : avatars.Select(x => x.Id).ToList(),
                role_id = uid,
                server = server
            };
            return await requester.PostWhileUpdateDynamicSecret2Async<DetailedAvatarInfo>(
                $@"{BaseUrl}/character", data);
        }
    }
}
