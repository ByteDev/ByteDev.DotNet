using System;

namespace ByteDev.DotNet.Solution
{
    internal static class ProjectTypeIds
    {
        public static readonly Guid SolutionFolder = new Guid("2150E333-8FDC-42A3-9474-1A3956D46DE8");

        public static readonly Guid CSharp = new Guid("FAE04EC0-301F-11D3-BF4B-00C04F79EFBC");
        public static readonly Guid CSharpNewFormat = new Guid("9A19103F-16F7-4668-BE54-9A1E7A4F7556");

        public static readonly Guid FSharp = new Guid("F2A71F9B-5D33-465A-A702-920D77279786");
        public static readonly Guid JSharp = new Guid("E6FDF86B-F3D1-11D4-8576-0002A516ECE8");
        public static readonly Guid CPlusPlus = new Guid("8BC9CEB8-8B4A-11D0-8D11-00A0C91BC942");
        public static readonly Guid VbDotNet = new Guid("F184B08F-C81C-45F6-A57F-5ABD9991F28F");


        // https://www.codeproject.com/Reference/720512/List-of-Visual-Studio-Project-Type-GUIDs
    }
}