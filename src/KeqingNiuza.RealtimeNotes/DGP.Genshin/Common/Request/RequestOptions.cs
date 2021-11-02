using System;
using System.Collections.Generic;

namespace DGP.Genshin.Common.Request
{
    public class RequestOptions : Dictionary<string, string>
    {
        public const string CommonUA2101 = @"Mozilla/5.0 (Windows NT 10.0; Win64; x64) miHoYoBBS/2.10.1";
        public const string CommonUA2111 = @"Mozilla/5.0 (Windows NT 10.0; Win64; x64) miHoYoBBS/2.11.1";
        public static readonly string DeviceId = Guid.NewGuid().ToString("D");
        public const string Json = @"application/json";
        public const string Hyperion = "com.mihoyo.hyperion";
    }
}