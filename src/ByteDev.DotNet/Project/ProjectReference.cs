namespace ByteDev.DotNet.Project
{
    public class ProjectReference
    {
        /// <summary>
        /// Path to the project file.
        /// </summary>
        public string FilePath { get; set; }

        public override string ToString()
        {
            return FilePath;
        }
    }
}