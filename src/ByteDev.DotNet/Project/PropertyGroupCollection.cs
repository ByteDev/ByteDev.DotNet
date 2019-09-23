using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using ByteDev.DotNet.Project.Parsers;

namespace ByteDev.DotNet.Project
{
    internal class PropertyGroupCollection
    {
        public IList<XElement> PropertyGroupElements { get; }

        public PropertyGroupCollection(XDocument xDocument)
        {
            PropertyGroupElements = GetPropertyGroups(xDocument);
        }

        public string GetElementValue(string elementName)
        {
            if(string.IsNullOrEmpty(elementName))
                throw new ArgumentException("Element name was null or empty.", nameof(elementName));

            return PropertyGroupElements
                .SingleOrDefault(pg => pg.Element(elementName) != null)?
                .Element(elementName)?
                .Value;
        }

        public bool GetElementBoolValue(string elementName, bool defaultValue = false)
        {
            var value = GetElementValue(elementName);

            if (string.IsNullOrEmpty(value))
                return defaultValue;

            return Convert.ToBoolean(value);
        }

        private static IList<XElement> GetPropertyGroups(XDocument xDocument)
        {
            var propertyGroups = ProjectXmlParser.GetPropertyGroups(xDocument)?.ToList();

            if (propertyGroups == null || !propertyGroups.Any())
                throw new InvalidDotNetProjectException("Project document contains no PropertyGroup elements.");

            return propertyGroups;
        }
    }
}