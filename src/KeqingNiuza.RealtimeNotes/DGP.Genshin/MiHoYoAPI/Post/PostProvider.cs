using DGP.Genshin.Common.Request;
using DGP.Genshin.Common.Request.DynamicSecret;
using DGP.Genshin.Common.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DGP.Genshin.MiHoYoAPI.Post
{
    public class PostProvider
    {
        private const string Referer = @"https://bbs.mihoyo.com/";
        private const string PostBaseUrl = @"https://bbs-api.mihoyo.com/post/wapi";

        private readonly string cookie;
        public PostProvider(string cookie)
        {
            this.cookie = cookie;
        }
        /// <summary>
        /// 获取推荐的帖子列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<Post>> GetOfficialRecommendedPostsAsync()
        {
            Requester requester = new Requester(new RequestOptions
            {
                {"DS", DynamicSecretProvider.Create() },
                {"x-rpc-app_version", DynamicSecretProvider.AppVersion },
                {"User-Agent", RequestOptions.CommonUA2101 },
                {"x-rpc-device_id", RequestOptions.DeviceId },
                {"Accept", RequestOptions.Json },
                {"x-rpc-client_type", "4" },
                {"Referer",Referer },
                {"Cookie", cookie }
            });
            Response<ListWrapper<Post>> resp =
                await requester.GetAsync<ListWrapper<Post>>($"{PostBaseUrl}/getOfficialRecommendedPosts?gids=2");
            return resp?.Data?.List;
        }
        /// <summary>
        /// 获取单个帖子的详细信息
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        public async Task<dynamic> GetPostFullAsync(string postId)
        {
            Requester requester = new Requester(new RequestOptions
            {
                {"DS", DynamicSecretProvider.Create() },
                {"x-rpc-app_version", DynamicSecretProvider.AppVersion },
                {"User-Agent", RequestOptions.CommonUA2101 },
                {"x-rpc-device_id", RequestOptions.DeviceId },
                {"Accept", RequestOptions.Json },
                {"x-rpc-client_type", "4" },
                {"Referer",Referer },
                {"Cookie", cookie }
            });
            Response<dynamic> resp =
                await requester.GetAsync<dynamic>($"{PostBaseUrl}/getPostFull?gids=2&post_id={postId}&read=1");
            return resp?.Data;
        }
    }
}
