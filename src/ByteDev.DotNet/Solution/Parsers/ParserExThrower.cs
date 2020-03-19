using System;

namespace ByteDev.DotNet.Solution.Parsers
{
    internal static class ParserExThrower
    {
        public static void ThrowSlnTextNullOrEmpty(string paramName)
        {
            throw new ArgumentException("Solution text is null or empty.", paramName);
        }
    }
}