using System;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace KeqingNiuza.Core.DailyCheck
{
    /// <summary>
    /// 返回头部信息
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RootEntity<T>
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("retcode")]
        public int Retcode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("message")]
        public string Message { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("data")]
        public T Data { get; set; } = Activator.CreateInstance<T>();

        /// <summary>
        /// 判断返回码并延迟
        /// </summary>
        /// <returns></returns>
        public string CheckOutCodeAndSleep()
        {
            Task.Delay(3 * 1000).Wait();
            switch (Retcode)
            {
                case 0:
                    return "执行成功";
                case -5003:
                    return $"{Message}";
                default:
                    throw new GenShinException(Message);
            }
        }

        public override string ToString()
        {
            return $"返回码为{Retcode}";
        }
    }
}
