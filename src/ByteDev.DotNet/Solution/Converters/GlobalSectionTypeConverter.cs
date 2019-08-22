namespace ByteDev.DotNet.Solution.Converters
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