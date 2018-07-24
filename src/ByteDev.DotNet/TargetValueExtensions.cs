using System.Globalization;

namespace ByteDev.DotNet
{
    internal static class TargetValueExtensions
    {
        public static bool IsCore(this string source)
        {
            return source.StartsWith("netcoreapp", true, CultureInfo.InvariantCulture);
        }

        public static bool IsStandard(this string source)
        {
            return source.StartsWith("netstandard", true, CultureInfo.InvariantCulture);
        }

        public static bool IsFramework(this string source)
        {
            return source.StartsWith("v", true, CultureInfo.InvariantCulture);
        }
    }
}