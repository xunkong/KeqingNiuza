using System;
using System.Net.Http;

namespace KeqingNiuza.Core.DailyCheck
{
    /// <summary>
    /// 原神请求Request
    /// </summary>
    public class GenshinHttpRequestMessage : HttpRequestMessage
    {
        /// <summary>
        /// 构建一个基础请求
        /// </summary>
        /// <param name="method">请求方式</param>
        /// <param name="uri">请求连接</param>
        /// <param name="cookie">米游社cookie</param>
        public GenshinHttpRequestMessage(HttpMethod method, Uri uri, string cookie) : base(method, uri)
        {
            if (string.IsNullOrEmpty(cookie))
            {
                throw new ArgumentException("必须设置cookie后才能请求");
            }

            Headers.Add("Accept-Encoding", Config.AcceptEncoding);
            Headers.Add("User-Agent", Config.Ua);
            Headers.Add("Referer", Config.RefererUrl);
            Headers.Add("Cookie", cookie);
            Headers.Add("x-rpc-device_id", Guid.NewGuid().ToString("D"));
        }

        /// <summary>
        /// 有关额外的请求Header
        /// </summary>
        public void GenshinExtraHeader()
        {
            Headers.Add("x-rpc-client_type", Config.ClientType);
            Headers.Add("x-rpc-app_version", Config.AppVersion);
            Headers.Add("DS", Config.GetDs());
        }
    }
}
