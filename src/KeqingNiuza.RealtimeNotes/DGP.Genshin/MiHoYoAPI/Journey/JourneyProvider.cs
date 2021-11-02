using DGP.Genshin.Common.Request;
using DGP.Genshin.Common.Response;
using System.Threading.Tasks;

namespace DGP.Genshin.MiHoYoAPI.Journey
{
    public class JourneyProvider 
    {
        private const string ApiHk4e = "https://hk4e-api.mihoyo.com";
        private const string ReferBaseUrl = @"https://webstatic.mihoyo.com/bbs/event/e20200709ysjournal/index.html";

        private const string BBSQueryString = "bbs_presentation_style=fullscreen&bbs_auth_required=true&utm_source=bbs&utm_medium=mys&utm_campaign=icon";

        private static readonly string Referer = $"{ReferBaseUrl}?{BBSQueryString}";

        private readonly string cookie;

        public JourneyProvider(string cookie)
        {
            this.cookie = cookie;
        }

        /// <summary>
        /// 获取月份信息
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="region"></param>
        /// <param name="month">0为起始请求</param>
        /// <returns></returns>
        public async Task<JourneyInfo> GetMonthInfoAsync(string uid, string region, int month = 0)
        {
            if (uid == null || region == null)
            {
                return null;
            }

            Requester requester = new Requester(new RequestOptions
            {
                {"User-Agent", RequestOptions.CommonUA2101 },
                {"Referer",Referer },
                {"Cookie", cookie },
                {"X-Requested-With", RequestOptions.Hyperion }
            });
            Response<JourneyInfo> resp = await requester.GetAsync<JourneyInfo>
                ($@"{ApiHk4e}/event/ys_ledger/monthInfo?month={month}&bind_uid={uid}&bind_region={region}&{BBSQueryString}");
            return resp?.Data;
        }

        /// <summary>
        /// 一次请求10条记录
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="region"></param>
        /// <param name="month"></param>
        /// <param name="type">1：原石，2：摩拉</param>
        /// <param name="page">请求的页码</param>
        /// <returns>当返回列表的数量不足10个时应停止请求</returns>
        public async Task<JourneyDetail> GetMonthDetailAsync(string uid, string region, int month, int type, int page = 1)
        {
            Requester requester = new Requester(new RequestOptions
            {
                {"User-Agent", RequestOptions.CommonUA2101 },
                {"Referer",Referer },
                {"Cookie", cookie },
                {"X-Requested-With", RequestOptions.Hyperion }
            });
            Response<JourneyDetail> resp = await requester.GetAsync<JourneyDetail>
                ($@"{ApiHk4e}/event/ys_ledger/monthDetail?page={page}&month={month}&limit=10&type=2&bind_uid={uid}&bind_region={region}&{BBSQueryString}");
            return resp?.Data;
        }
    }
}
