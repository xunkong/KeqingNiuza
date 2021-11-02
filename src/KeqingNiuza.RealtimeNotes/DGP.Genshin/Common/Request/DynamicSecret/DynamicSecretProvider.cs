using System;
using System.Text;

namespace DGP.Genshin.Common.Request.DynamicSecret
{

    /// <summary>
    /// 为MiHoYo接口请求器 <see cref="Requester"/> 提供动态密钥
    /// </summary>
    public class DynamicSecretProvider : DynamicSecretProviderMd5Base
    {
        /// <summary>
        /// 防止从外部创建 <see cref="DynamicSecretProvider"/> 的实例
        /// </summary>
        private DynamicSecretProvider() { }

        public const string AppVersion = "2.10.1";

        private static readonly string APISalt = "4a8knnbk5pbjqsrudp3dq484m9axoc5g"; // @Azure99 respect original author
        public static string Create()
        {
            var time = new DateTime(2021, 11, 1, 11, 14, 00);
            int t = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            string r = GetRandomString(t);
            string check = GetComputedMd5($"salt={APISalt}&t={t}&r={r}");
            string result = $"{t},{r},{check}";
            return result;
        }
        private static string GetRandomString(int time)
        {
            StringBuilder sb = new StringBuilder(6);
            Random random = new Random(time);

            for (int i = 0; i < 6; i++)
            {
                int v8 = random.Next(0, 32768) % 26;
                int v9 = 87;
                if (v8 < 10)
                {
                    v9 = 48;
                }
                _ = sb.Append((char)(v8 + v9));
            }
            return sb.ToString();
        }
    }

}