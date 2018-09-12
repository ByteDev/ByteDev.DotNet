namespace ByteDev.DotNet.IntTests.Project
{
    internal static class TestProjFiles
    {
        private const string BasePath = @"Project\TestProjs\";

        internal static class NewFormat
        {
            public const string NoPropertyGroups = BasePath + "new-nopropertygroups.xml";

            public const string Framework471 = BasePath + "new-framework471.xml";
            public const string Std20 = BasePath + "new-std20.xml";
            public const string Core21 = BasePath + "new-core21.xml";

            public const string Std15AndFramework4 = BasePath + "new-std15-and-net40.xml";

            public const string Core21Exe = BasePath + "new-core21-exe.xml";
        }

        internal static class OldFormat
        {
            public const string Framework462 = BasePath + "old-framework462.xml";
        }
    }
}