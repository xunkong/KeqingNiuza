using System;

namespace KeqingNiuza.Core.DailyCheck
{
    /// <summary>
    /// 输出格式化
    /// </summary>
    public static class WriteLineUtil
    {
        public static void WriteLineLog(object e)
        {
            var time = DateTime.Now.ToString("HH:mm:ss");

            Console.WriteLine($"[{time}]:{e}");
        }
    }
}
