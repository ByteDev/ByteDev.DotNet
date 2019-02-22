namespace ByteDev.DotNet.Project
{
    public class ProjectReference
    {
        public string FilePath { get; set; }

        public override string ToString()
        {
            return FilePath;
        }
    }
}