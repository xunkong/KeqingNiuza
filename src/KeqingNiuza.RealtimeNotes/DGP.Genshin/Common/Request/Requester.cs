using DGP.Genshin.Common.Data.Json;
using DGP.Genshin.Common.Response;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace DGP.Genshin.Common.Request
{
    /// <summary>
    /// MiHoYo API 专用请求器
    /// 同一个 <see cref="Requester"/> 若使用一代动态密钥不能长时间使用
    /// </summary>
    public class Requester
    {
        // HttpClient is intended to be instantiated once per application, rather than per-use.
        // .NET Framework中HttpClient的默认底层HttpWebRequest会造成没有登录的错误，改用.NET Core的SocketsHttpHandler
        private static readonly Lazy<HttpClient> LazyHttpClient = new Lazy<HttpClient>(() => new HttpClient(new StandardSocketsHttpHandler()) { Timeout = Timeout.InfiniteTimeSpan });

        public RequestOptions Headers { get; set; } = new RequestOptions();

        /// <summary>
        /// 构造一个新的 <see cref="Requester"/> 对象
        /// </summary>
        public Requester() { }

        /// <summary>
        /// 构造一个新的 <see cref="Requester"/> 对象
        /// </summary>
        /// <param name="headers">请求头</param>
        public Requester(RequestOptions headers)
        {
            Headers = headers;
        }

        private async Task<Response<T>> Request<T>(Func<HttpClient, Task<HttpResponseMessage>> requestMethod)
        {
            try
            {
                HttpClient client = LazyHttpClient.Value;
                client.DefaultRequestHeaders.Clear();
                foreach (KeyValuePair<string, string> entry in Headers)
                {
                    client.DefaultRequestHeaders.Add(entry.Key, entry.Value);
                }
                HttpResponseMessage response = await requestMethod(client);
                HttpContent content = response.Content;
                Response<T> resp = Json.ToObject<Response<T>>(await content.ReadAsStringAsync());
                return resp;
            }
            catch (Exception ex)
            {
                return new Response<T>
                {
                    ReturnCode = (int)KnownReturnCode.InternalFailure,
                    Message = ex.Message
                };
            }
        }

        /// <summary>
        /// GET 操作
        /// </summary>
        /// <typeparam name="T">返回的类类型</typeparam>
        /// <param name="url">地址</param>
        /// <returns>响应</returns>
        public async Task<Response<T>> GetAsync<T>(string url)
        {
            return url is null ? null : await Request<T>(client => client.GetAsync(url));
        }

        /// <summary>
        /// POST 操作
        /// </summary>
        /// <typeparam name="T">返回的类类型</typeparam>
        /// <param name="url">地址</param>
        /// <param name="data">要发送的.NET（匿名）对象</param>
        /// <returns>响应</returns>
        public async Task<Response<T>> PostAsync<T>(string url, dynamic data)
        {
            return url is null ? null : await Request<T>(client => client.PostAsync(url, new StringContent(Json.Stringify(data))));
        }

        /// <summary>
        /// POST 操作
        /// </summary>
        /// <typeparam name="T">返回的类类型</typeparam>
        /// <param name="url">地址</param>
        /// <param name="data">要发送的.NET（匿名）对象</param>
        /// <returns></returns>
        public async Task<Response<T>> PostAsync<T>(string url, string data)
        {
            return url is null ? null : await Request<T>(client => client.PostAsync(url, new StringContent(data)));
        }
    }
}