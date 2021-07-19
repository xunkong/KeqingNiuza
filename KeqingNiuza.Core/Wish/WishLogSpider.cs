using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace KeqingNiuza.Core.Wish
{
    public class WishLogSpider
    {
        private const string baseRequestUrl = @"https://hk4e-api.mihoyo.com/event/gacha_info/api/getGachaLog";

        private readonly string authString;

        private readonly HttpClient HttpClient;

        public event EventHandler<(WishType WishType, int Page)> ProgressChanged;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="url">祈愿记录网页的Url，以https://webstatic.mihoyo.com开头，以#/log结尾</param>
        public WishLogSpider(string url)
        {
            if (url.StartsWith("https://webstatic.mihoyo.com") && url.EndsWith("#/log"))
            {
                authString = url.Substring(url.IndexOf('?')).Replace("#/log", "");
                HttpClient = new HttpClient();
            }
            else
            {
                throw new ArgumentException("Url错误");
            }
        }


        public static string GetQueryParam(WishType type,int page,int size,long endId)
        {
            return $@"gacha_type={(int)type}&page={page}&size={size}&end_id={endId}";
        }


    }
}
