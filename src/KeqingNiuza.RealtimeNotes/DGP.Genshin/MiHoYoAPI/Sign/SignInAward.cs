using Newtonsoft.Json;

namespace DGP.Genshin.MiHoYoAPI.Sign
{
    public class SignInAward
    {
        [JsonProperty("icon")] public string Icon { get; set; }
        [JsonProperty("name")] public string Name { get; set; }
        [JsonProperty("cnt")] public string Count { get; set; }

        /// <summary>
        /// 仅用于Snap Genshin 内调整透明度
        /// </summary>
        [JsonIgnore] public double Opacity { get; set; }
    }
}
