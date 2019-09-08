using System;
using System.Text.RegularExpressions;

namespace ByteDev.DotNet.Solution.Parsers
{
    internal class MajorVersionParser : ISolutionTextParser<int>
    {
        public int Parse(string slnText)
        {
            try
            {
                var value = Regex.Match(slnText, "# Visual Studio ([0-9]+)").Groups[1].Value;

                return Convert.ToInt32(value);
            }
            catch (Exception ex)
            {
                throw new InvalidDotNetSolutionException("A valid Major Version could not be found in the sln text.", ex);
            }
        }
    }
}