using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace ByteDev.DotNet.Project
{
    internal class ProjectXmlParser
    {
        public static IEnumerable<XElement> GetPropertyGroups(XDocument xDocument)
        {
            return xDocument.Root?.Descendants().Where(d => d.Name.LocalName == "PropertyGroup");
        }
    }
}