using Newtonsoft.Json;

namespace DGP.Genshin.MiHoYoAPI.Sign
{
    public class ReSignInfo
    {
        [JsonProperty("resign_cnt_daily")] public bool ResignCountDaily { get; set; }
        [JsonProperty("resign_cnt_monthly")] public bool ResignCountMonthly { get; set; }
        [JsonProperty("resign_limit_daily")] public bool ResignLimitDaily { get; set; }
        [JsonProperty("resign_limit_monthly")] public bool ResignLimitMonthly { get; set; }
        [JsonProperty("sign_cnt_missed")] public bool SignCountMissed { get; set; }
        [JsonProperty("coin_cnt")] public bool CoinCount { get; set; }
        [JsonProperty("coin_cost")] public bool CoinCost { get; set; }
        [JsonProperty("rule")] public string Rule { get; set; }
    }
}
