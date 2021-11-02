using Newtonsoft.Json;
using System;
using System.Linq;

namespace DGP.Genshin.Common.Request.DynamicSecret
{
    /// <summary>
    /// 为MiHoYo接口请求器 <see cref="Requester"/> 提供2代动态密钥
    /// </summary>
    public class DynamicSecretProvider2 : DynamicSecretProviderMd5Base
    {
        /// <summary>
        /// 防止从外部创建 <see cref="DynamicSecretProvider2"/> 的实例
        /// </summary>
        private DynamicSecretProvider2() { }

        public const string AppVersion = "2.11.1";
        /// <summary>
        /// @Azure99 respect original author
        /// </summary>
        private static readonly string APISalt = "xV8v4Qu54lUKrEYFZkJhB8cuOh9Asafs";

        public static string Create(string queryUrl, object postBody = null)
        {
            //unix timestamp
            int t = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            //random
            string r = GetRandomString();
            //body
            string b = postBody is null ? "" : Stringify(postBody);
            //query
            string q = "";
            string[] url = queryUrl.Split('?');
            if (url.Length == 2)
            {
                string[] queryParams = url[1].Split('&').OrderBy(x => x).ToArray();
                q = string.Join("&", queryParams);
            }
            //check
            string check = GetComputedMd5($"salt={APISalt}&t={t}&r={r}&b={b}&q={q}");
            string result = $"{t},{r},{check}";
            return result;
        }
        private static readonly Random random = new Random();
        private static string GetRandomString()
        {
            return random.Next(100000, 200000).ToString();
        }



        public static string Stringify(object value)
        {
            //set date format string to make it compatible to gachaData
            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings()
            { 
                NullValueHandling = NullValueHandling.Include,
                //兼容原神api格式
                DateFormatString = "yyyy'-'MM'-'dd' 'HH':'mm':'ss.FFFFFFFK",
                Formatting = Formatting.Indented,
            };
            return JsonConvert.SerializeObject(value, jsonSerializerSettings);
        }
    }
}
