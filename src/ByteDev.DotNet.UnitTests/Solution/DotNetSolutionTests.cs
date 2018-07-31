using System;
using ByteDev.DotNet.Solution;
using NUnit.Framework;

namespace ByteDev.DotNet.UnitTests.Solution
{
    [TestFixture]
    public class DotNetSolutionTests
    {
        [Test]
        public void WhenSlnTextIsNull_ThenThrowException()
        {
            Assert.Throws<ArgumentException>(() => new DotNetSolution(null));
        }

        [Test]
        public void WhenSlnTextIsEmpty_ThenThrowException()
        {
            Assert.Throws<ArgumentException>(() => new DotNetSolution(string.Empty));
        }
    }
}