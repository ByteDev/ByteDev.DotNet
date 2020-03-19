using ByteDev.DotNet.Solution;
using NUnit.Framework;

namespace ByteDev.DotNet.Shared.PackageTests
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void WhenSlnExists_ThenRead()
        {
            var sln = DotNetSolution.Load(@"Test.sln");

            Assert.That(sln.MajorVersion, Is.EqualTo(16));
        }
    }
}