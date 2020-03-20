using System;
using System.Text.RegularExpressions;

namespace ByteDev.DotNet.Solution.Parsers
{
    internal class VisualStudioVersionParser : ISolutionTextParser<string>
    {
        private const string ExMessage = "A valid Visual Studio Version could not be found in the sln text.";

        public string Parse(string slnText)
        {
            if (string.IsNullOrEmpty(slnText))
                ParserExThrower.ThrowSlnTextNullOrEmpty(nameof(slnText));

            try
            {
                var match = Regex.Match(slnText, "^VisualStudioVersion = ([0-9.]+)", RegexOptions.Multiline);

                var value = match.Groups[1].Value;

                if (value == string.Empty)
                    throw new InvalidDotNetSolutionException(ExMessage);

                return value;
            }
            catch (Exception ex)
            {
                throw new InvalidDotNetSolutionException(ExMessage, ex);
            }
        }
    }
}