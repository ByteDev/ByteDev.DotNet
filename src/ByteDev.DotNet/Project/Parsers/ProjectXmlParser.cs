using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace ByteDev.DotNet.Project.Parsers
{
    internal class ProjectXmlParser
    {
        public static IEnumerable<XElement> GetPropertyGroups(XDocument xDocument)
        {
            return xDocument.Root?.Descendants().Where(d => d.Name.LocalName == "PropertyGroup");
        }

        public static IEnumerable<XElement> GetItemGroups(XDocument xDocument)
        {
            return xDocument.Root?.Descendants().Where(d => d.Name.LocalName == "ItemGroup");
        }
    }
}