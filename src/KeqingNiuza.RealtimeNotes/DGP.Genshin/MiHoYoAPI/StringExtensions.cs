using System.Text.RegularExpressions;

namespace DGP.Genshin.MiHoYoAPI
{
    public static class StringExtensions
    {
        /// <summary>
        /// 将html字符串转换为普通字符串
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string RemoveXmlFormat(this string s)
        {
            //color
            s = new Regex(@"<color=.*?>").Replace(s, "");
            s = s.Replace("</color>", "");
            //important mark
            s = s.Replace("<i>", "").Replace("</i>", "");
            //apply \n & \r char
            s = s.Replace(@"\n", "\n").Replace(@"\r", "\r");
            return s;
        }
    }
}
