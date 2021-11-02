using System.Runtime.InteropServices;

namespace System
{
    internal class RuntimeUtils
    {
        public static bool IsDotNetFramework()
        {
            const string DotnetFrameworkDescription = ".NET Framework";
            string frameworkDescription = RuntimeInformation.FrameworkDescription;
            return frameworkDescription.StartsWith(DotnetFrameworkDescription);
        }
    }
}
