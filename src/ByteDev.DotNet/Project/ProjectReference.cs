namespace ByteDev.DotNet.Project
{
    public class ProjectReference
    {
        public string Include { get; set; }

        public override string ToString()
        {
            return Include;
        }
    }
}