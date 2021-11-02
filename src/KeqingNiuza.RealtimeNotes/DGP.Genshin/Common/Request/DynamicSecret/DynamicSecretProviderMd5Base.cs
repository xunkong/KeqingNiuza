using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Text;

namespace DGP.Genshin.Common.Request.DynamicSecret
{
    /// <summary>
    /// 为动态密钥提供器提供Md5算法
    /// </summary>
    public abstract class DynamicSecretProviderMd5Base
    {
        [SuppressMessage("", "CA5351")]
        protected static string GetComputedMd5(string source)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] result = md5.ComputeHash(Encoding.UTF8.GetBytes(source));

                StringBuilder builder = new StringBuilder();
                foreach (byte b in result)
                {
                    _ = builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}