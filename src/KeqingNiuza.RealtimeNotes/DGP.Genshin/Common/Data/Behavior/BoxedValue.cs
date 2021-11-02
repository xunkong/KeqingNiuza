using System.Diagnostics.CodeAnalysis;

namespace DGP.Genshin.Common.Data.Behavior
{
    /// <summary>
    /// increase a slightly performances
    /// </summary>
    [SuppressMessage("Usage", "CA2211:非常量字段应当不可见")]
    public static class BoxedValue
    {

        public static object TrueBox = true;
        public static object FalseBox = false;

        public static object Double0Box = .0;
        public static object Double01Box = .1;
        public static object Double1Box = 1.0;
        public static object Double10Box = 10.0;
        public static object Double20Box = 20.0;
        public static object Double100Box = 100.0;
        public static object Double200Box = 200.0;
        public static object Double300Box = 300.0;
        public static object DoubleNeg1Box = -1.0;

        public static object Int0Box = 0;
        public static object Int1Box = 1;
        public static object Int2Box = 2;
        public static object Int5Box = 5;
        public static object Int99Box = 99;

        public static object Boolean(bool value)
        {
            return value ? TrueBox : FalseBox;
        }
    }
}
