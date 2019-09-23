using System.Collections.Generic;
using System.Linq;

namespace ByteDev.DotNet
{
    internal static class StringExtensions
    {
        /// <summary>
        /// Splits the string on comma if it contains a comma.
        /// Otherwise splits on space.
        /// </summary>
        /// <param name="source">String to split on.</param>
        /// <returns>Enumerable list of elements.</returns>
        public static IEnumerable<string> SplitOnCommaOrSpace(this string source)
        {
            if (string.IsNullOrEmpty(source))
                return Enumerable.Empty<string>();

            if (source.Contains(','))
                return source.Split(',').Select(tag => tag.Trim());

            return source.Split(' ');
        }

        public static IEnumerable<string> SplitOnSemiColon(this string source)
        {
            if (string.IsNullOrEmpty(source))
                return Enumerable.Empty<string>();

            return source.Split(';');
        }
    }
}