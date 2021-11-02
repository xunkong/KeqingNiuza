using Newtonsoft.Json;

namespace DGP.Genshin.Common.Response
{
    /// <summary>
    /// 提供 <see cref="Response{T}"/> 的非泛型基类
    /// </summary>
    public class Response
    {
        /// <summary>
        /// 0 is OK
        /// </summary>
        [JsonProperty("retcode")] public int ReturnCode { get; set; }
        [JsonProperty("message")] public string Message { get; set; }

        public override string ToString()
        {
            return $"状态：{ReturnCode} | 信息：{Message}";
        }
    }

    /// <summary>
    /// Mihoyo 标准API响应
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    public class Response<T> : Response
    {
        [JsonProperty("data")] public T Data { get; set; }
    }
}