namespace ByteDev.DotNet.Project
{
    internal class VersionNumberFormatter
    {
        /// <summary>
        /// Returns a formatted version number of the
        /// form: [Major].[Minor].[Patch].
        /// If the value contains any "." then no formatting will be applied.
        /// </summary>
        public static string Format(string value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            if (value.Contains("."))
                return value;

            var formatted = string.Join(".", value.ToCharArray());

            var dotCount = 0;
            var buffer = formatted.ToCharArray();

            for (var i = 0; i < buffer.Length; i++)
            {
                if (buffer[i] == '.')
                {
                    dotCount++;

                    if (dotCount > 2)
                        buffer[i] = ' ';
                }
            }

            return string.Join(string.Empty, buffer).Replace(" ", string.Empty);
        }
    }
}