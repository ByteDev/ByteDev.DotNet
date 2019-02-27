using System;
using System.Text.RegularExpressions;

namespace ByteDev.DotNet.Solution.Parsers
{
    internal class VisualStudioVersionParser : ISolutionTextParser<string>
    {
        private const string Marker = "VisualStudioVersion = ";

        public string Parse(string slnText)
        {
            try
            {
                return Regex.Match(slnText, "^" + Marker + "([0-9.]+)", RegexOptions.Multiline).Value.Substring(Marker.Length);
            }
            catch (Exception ex)
            {
                throw new InvalidDotNetSolutionException("A valid Visual Studio Version could not be found in the sln text.", ex);
            }
        }
    }
}