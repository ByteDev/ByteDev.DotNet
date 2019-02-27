using System;
using System.Text.RegularExpressions;

namespace ByteDev.DotNet.Solution.Parsers
{
    internal class MinimumVisualStudioVersionParser : ISolutionTextParser<string>
    {
        private const string Marker = "MinimumVisualStudioVersion = ";

        public string Parse(string slnText)
        {
            try
            {
                return Regex.Match(slnText, "^" + Marker + "([0-9.]+)", RegexOptions.Multiline).Value.Substring(Marker.Length);
            }
            catch (Exception ex)
            {
                throw new InvalidDotNetSolutionException("A valid Minimum Visual Studio Version could not be found in the sln text.", ex);
            }
        }
    }
}