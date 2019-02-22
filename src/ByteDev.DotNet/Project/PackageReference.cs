namespace ByteDev.DotNet.Project
{
    public class PackageReference
    {
        public string Name { get; set; }

        public string Version { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}