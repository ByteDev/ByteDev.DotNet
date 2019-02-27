using System;
using System.Text.RegularExpressions;

namespace ByteDev.DotNet.Solution.Parsers
{
    internal class FormatVersionParser : ISolutionTextParser<string>
    {
        private const string ErrorMessage = "A valid Format Version could not be found in the sln text.";

        public string Parse(string slnText)
        {
            try
            {
                var value = Regex.Match(slnText, "[\r\n]?Microsoft Visual Studio Solution File, Format Version ([0-9.]+)", RegexOptions.Multiline).Groups[1].Value;

                if (string.IsNullOrEmpty(value))
                    throw new InvalidDotNetSolutionException(ErrorMessage);

                return value;
            }
            catch (Exception ex)
            {
                throw new InvalidDotNetSolutionException(ErrorMessage, ex);
            }
        }
    }
}