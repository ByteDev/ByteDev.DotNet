namespace ByteDev.DotNet.Solution.Converters
{
    internal static class ProjectSectionTypeConverter
    {
        public static ProjectSectionType ConvertToType(string value)
        {
            switch (value)
            {
                case "preProject":
                    return ProjectSectionType.PreProject;
                case "postProject":
                    return ProjectSectionType.PostProject;
                default:
                    return ProjectSectionType.Unknown;
            }
        }
    }
}