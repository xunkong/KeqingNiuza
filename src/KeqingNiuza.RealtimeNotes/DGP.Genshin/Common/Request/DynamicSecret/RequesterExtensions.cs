using DGP.Genshin.Common.Response;
using System.Net.Http;
using System.Threading.Tasks;

namespace DGP.Genshin.Common.Request.DynamicSecret
{
    public static class RequesterExtensions
    {
        /// <summary>
        /// 使用2代动态密钥需要调用此扩展方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="requester"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<T> GetWhileUpdateDynamicSecret2Async<T>(this Requester requester, string url) where T : class
        {
            requester.Headers["DS"] = DynamicSecretProvider2.Create(url);
            Response<T> resp = await requester.GetAsync<T>(url);
            if (resp.ReturnCode == 0)
            {
                return resp?.Data;
            }
            else
            {
                throw new HttpRequestException($"{resp.ReturnCode} {resp.Message}");
            }
        }

        /// <summary>
        /// 使用2代动态密钥需要调用此扩展方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="requester"></param>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static async Task<T> PostWhileUpdateDynamicSecret2Async<T>(this Requester requester, string url, object data) where T : class
        {
            requester.Headers["DS"] = DynamicSecretProvider2.Create(url, data);
            Response<T> resp = await requester.PostAsync<T>(url, data);
            return resp?.Data;
        }
    }
}
