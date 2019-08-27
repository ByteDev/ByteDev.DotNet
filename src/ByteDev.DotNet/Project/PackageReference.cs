namespace ByteDev.DotNet.Project
{
    public class PackageReference
    {
        /// <summary>
        /// Package name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Package version number. Typically in: (Major).(Minor).(Patch) format. 
        /// </summary>
        public string Version { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}