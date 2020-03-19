using System;
using System.Text.RegularExpressions;

namespace ByteDev.DotNet.Solution.Parsers
{
    internal class MajorVersionParser : ISolutionTextParser<int>
    {
        public int Parse(string slnText)
        {
            if(string.IsNullOrEmpty(slnText))
                ParserExThrower.ThrowSlnTextNullOrEmpty(nameof(slnText));

            try
            {
                // Match: "# Visual Studio 15" or "# Visual Studio Version 16"

                var match = Regex.Match(slnText, @"# Visual Studio?( Version|) ([0-9]+)");

                return Convert.ToInt32(match.Groups[2].Value);
            }
            catch (Exception ex)
            {
                throw new InvalidDotNetSolutionException("A valid Major Version could not be found in the sln text.", ex);
            }
        }
    }
}