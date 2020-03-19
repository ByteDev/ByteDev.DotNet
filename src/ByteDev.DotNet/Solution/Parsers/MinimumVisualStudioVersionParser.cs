using System;
using System.Text.RegularExpressions;

namespace ByteDev.DotNet.Solution.Parsers
{
    internal class MinimumVisualStudioVersionParser : ISolutionTextParser<string>
    {
        private const string ExMessage = "A valid Minimum Visual Studio Version could not be found in the sln text.";

        public string Parse(string slnText)
        {
            if(string.IsNullOrEmpty(slnText))
                ParserExThrower.ThrowSlnTextNullOrEmpty(nameof(slnText));

            try
            {
                var match = Regex.Match(slnText, "^MinimumVisualStudioVersion = ([0-9.]+)", RegexOptions.Multiline);

                var version = match.Groups[1].Value;

                if (version == string.Empty)
                    throw new InvalidDotNetSolutionException(ExMessage);

                return version;
            }
            catch (Exception ex)
            {
                throw new InvalidDotNetSolutionException(ExMessage, ex);
            }
        }
    }
}