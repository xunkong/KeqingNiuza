namespace DGP.Genshin.MiHoYoAPI.Gacha
{
    /// <summary>
    /// 请求祈愿记录页面进度
    /// </summary>
    public class FetchProgress
    {
        public string Type { get; set; }
        public int Page { get; set; }
        public override string ToString()
        {
            return $"{Type} 第 {Page} 页";
        }
    }
}
