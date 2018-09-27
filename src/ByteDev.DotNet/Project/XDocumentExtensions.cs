using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace ByteDev.DotNet.Project
{
    public static class XDocumentExtensions
    {
        public static IEnumerable<XElement> GetPropertyGroups(this XDocument source)
        {
            var propertyGroups = ProjectXmlParser.GetPropertyGroups(source)?.ToList();

            if (propertyGroups == null || !propertyGroups.Any())
                throw new InvalidDotNetProjectException("Project document contains no PropertyGroup elements.");

            return propertyGroups;
        }
    }
}