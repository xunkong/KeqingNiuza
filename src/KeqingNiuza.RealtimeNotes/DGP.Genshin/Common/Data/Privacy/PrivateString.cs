using System;

namespace DGP.Genshin.Common.Data.Privacy
{
    /// <summary>
    /// 隐私字符串
    /// </summary>
    public class PrivateString
    {
        public static readonly Func<string, string> DefaultMasker = str => str.Substring(0, 2) + "****" + str.Substring(6, 3);

        private readonly Func<string, string> masker;

        /// <summary>
        /// 构造一个新的隐私字符串对象
        /// </summary>
        /// <param name="data">源字符串</param>
        /// <param name="masker">掩码算法</param>
        public PrivateString(string data, Func<string, string> masker, bool shouldNotMask)
        {
            UnMaskedValue = data;
            this.masker = masker;
            ShouldNotMask = shouldNotMask;
        }

        /// <summary>
        /// 经过隐私设置处理后的字符串
        /// </summary>
        public string Value => ShouldNotMask ? UnMaskedValue : masker.Invoke(UnMaskedValue);

        public string UnMaskedValue { get; }

        /// <summary>
        /// 告知隐私字符串是否需要设置掩码
        /// </summary>
        public bool ShouldNotMask { get; set; }

        public void SetShouldNotMask(bool s)
        {
            ShouldNotMask = s;
        }
    }
}
