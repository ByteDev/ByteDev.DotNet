namespace ByteDev.DotNet.Solution.Parsers
{
    internal static class GlobalSectionTypeConverter
    {
        public static GlobalSectionType ConvertToType(string value)
        {
            switch (value)
            {
                case "preSolution":
                    return GlobalSectionType.PreSolution;
                case "postSolution":
                    return GlobalSectionType.PostSolution;
                default:
                    return GlobalSectionType.Unknown;
            }
        }
    }
}