using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace ByteDev.DotNet.Project.Parsers
{
    internal static class PropertyGroupXmlParser
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
            const string singleTargetName = "TargetFramework";
            const string multiTargetName = "TargetFrameworks";
            
            XElement element = propertyGroups.SingleOrDefault(pg => pg.Element(singleTargetName) != null)?
                .Element(singleTargetName);

            if (element != null)
                return element;

            return propertyGroups.SingleOrDefault(pg => pg.Element(multiTargetName) != null)?
                .Element(multiTargetName);
        }
    }
}