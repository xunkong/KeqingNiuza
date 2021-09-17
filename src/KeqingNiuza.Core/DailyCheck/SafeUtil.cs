using System;
using System.Security.Cryptography;
using System.Text;

namespace GenshinDailyHelper.Util
{
    public static class SafeUtil
    {
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string UserMd5(string str)
        {
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(str));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));//转化为小写的16进制
            }
            return sBuilder.ToString();
        }

        /// <summary>
        ///     获取时间戳
        /// </summary>
        /// <returns></returns>
        public static long GetCurrentTimestamp()
        {
            var number = 10000 * 1000;
            return (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / number;
        }

        /// <summary>
        /// 固定长度的随机字符串
        /// </summary>
        /// <param name="length">长度</param>
        /// <returns>随机串</returns>
        public static string GetRandString(int length)
        {
            char[] charList = {
                'a','b','c','d','e','f','g','h','i','j','k','l','m',
                'n','o','p','q','r','s','t','u','v','w','x','y','z',
                'A','B','C','D','E','F','G','H','I','J','K','L','M',
                'N','O','P','Q','R','S','T','U','V','W','X','Y','Z',
                '0','1','2','3','4','5','6','7','8','9',
            };
            char[] rev = new char[length];
            Random random = new Random();
            for (int i = 0; i < length; i++)
            {
                var index = random.Next(0, charList.Length - 1);
                rev[i] = charList[index];
            }
            return new string(rev);
        }
    }
}
