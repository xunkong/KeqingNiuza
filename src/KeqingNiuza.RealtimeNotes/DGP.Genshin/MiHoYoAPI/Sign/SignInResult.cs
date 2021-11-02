using Newtonsoft.Json;

namespace DGP.Genshin.MiHoYoAPI.Sign
{
    public class SignInResult
    {
        /// <summary>
        /// 通常是 ""
        /// </summary>
        [JsonProperty("code")] public string Code { get; set; }
    }
}
