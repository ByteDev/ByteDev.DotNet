using ByteDev.DotNet.Project;
using NUnit.Framework;

namespace ByteDev.DotNet.UnitTests.Project
{
    public class VersionNumberFormatterTests
    {
        [TestFixture]
        public class Format : VersionNumberFormatterTests
        {
            [TestCase(null, "")]
            [TestCase("", "")]
            [TestCase("4", "4")]
            [TestCase("47", "4.7")]
            [TestCase("471", "4.7.1")]
            [TestCase("4710", "4.7.10")]
            [TestCase("47102", "4.7.102")]
            [TestCase("4.7.10", "4.7.10")]
            [TestCase("4.7.1.2", "4.7.1.2")]
            public void WhenProvidedInput_ThenReturnFormattedVersion(string unformatted, string expected)
            {
                var result = VersionNumberFormatter.Format(unformatted);

                Assert.That(result, Is.EqualTo(expected));
            }
        }        
    }
}