using Newtonsoft.Json;

namespace GenshinDailyHelper.Entities
{
    /// <summary>
    /// 最后签到结果
    /// </summary>
    public class SignResultEntity : RootEntity<SignResultData>
    {

    }

    public class SignResultData
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("code")]
        public string Code { get; set; }
    }
}
