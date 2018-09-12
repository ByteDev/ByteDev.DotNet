using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace ByteDev.DotNet.Project.Parsers
{
    internal class PropertyGroupXmlParser
    {
        public static XElement GetOldStyleTargetElement(IEnumerable<XElement> propertyGroups)
        {
            const string name = "TargetFrameworkVersion";
            XNamespace nameSpace = "http://schemas.microsoft.com/developer/msbuild/2003";
            
            return propertyGroups.SingleOrDefault(pg => pg.Element(nameSpace + name) != null)?
                .Element(nameSpace + name);
        }

        public static XElement GetNewStyleTargetElement(IEnumerable<XElement> propertyGroups)
        {
            const string name = "TargetFramework";

            return propertyGroups.SingleOrDefault(pg => pg.Element(name) != null)?
                .Element(name);
        }
    }
}