namespace ByteDev.DotNet.IntTests.Project
{
    internal static class TestProjFiles
    {
        internal static class NewFormat
        {
            public static string NoPropertyGroups { get; } = GetPath("new-nopropertygroups.xml");
            public static string Std20 { get; } = GetPath("new-std20.xml");
            public static string Core21 { get; } = GetPath("new-core21.xml");
        }

        internal static class OldFormat
        {
            public static string Framework462 { get; } = GetPath("old-framework462.xml");
        }

        private static string GetPath(string fileName)
        {
            return @"Project\TestProjs\" + fileName;
        }
    }
}