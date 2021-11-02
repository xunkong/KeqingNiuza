using DGP.Genshin.Common.Request;
using DGP.Genshin.Common.Response;
using System.Net.Http;
using System.Threading.Tasks;

namespace DGP.Genshin.MiHoYoAPI.User
{
    public class UserGameRoleProvider
    {
        private const string ApiTakumi = @"https://api-takumi.mihoyo.com";

        private readonly string cookie;
        public UserGameRoleProvider(string cookie)
        {
            this.cookie = cookie;
        }

        /// <summary>
        /// 获取用户角色信息
        /// </summary>
        /// <returns>用户角色信息</returns>
        public async Task<UserGameRoleInfo> GetUserGameRolesAsync()
        {
            Requester requester = new Requester(new RequestOptions
            {
                {"Accept", RequestOptions.Json },
                {"User-Agent", RequestOptions.CommonUA2101 },
                {"Cookie", cookie },
                {"X-Requested-With", RequestOptions.Hyperion }
            });
            Response<UserGameRoleInfo> resp = await requester.GetAsync<UserGameRoleInfo>
                ($"{ApiTakumi}/binding/api/getUserGameRolesByCookie?game_biz=hk4e_cn");
            if (resp.ReturnCode == 0)
            {
                return resp?.Data;
            }
            else
            {
                throw new HttpRequestException($"{resp.ReturnCode} {resp.Message}");
            }
        }
    }
}
