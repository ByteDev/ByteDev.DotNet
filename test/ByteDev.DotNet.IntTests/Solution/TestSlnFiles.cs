namespace ByteDev.DotNet.IntTests.Solution
{
    internal static class TestSlnFiles
    {
        private const string BasePath = @"Solution\TestSlns\";

        public const string V12 = BasePath + "sln-v12.txt";
        public const string V12NoGlobal = BasePath + "sln-v12-noglobal.txt";
        public const string V12NoGlobalSections = BasePath + "sln-v12-nogs.txt";
        public const string V12NoProjs = BasePath + "sln-v12-noprojs.txt";

        public const string NoFormatVersion = BasePath + "sln-no-formatversion.txt";
        public const string NoMajorVersion = BasePath + "sln-no-majorversion.txt";
        public const string NoVsVersion = BasePath + "sln-no-vsversion.txt";
        public const string NoMinVsVersion = BasePath + "sln-no-minvsversion.txt";

        public const string MajorVersion16 = BasePath + "sln-majorver16.txt";
    }
}