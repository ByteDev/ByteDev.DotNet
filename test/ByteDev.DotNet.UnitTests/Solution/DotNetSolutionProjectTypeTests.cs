using ByteDev.DotNet.Solution;
using NUnit.Framework;

namespace ByteDev.DotNet.UnitTests.Solution
{
    [TestFixture]
    public class DotNetSolutionProjectTypeTests
    {
        [TestFixture]
        public class Constructor : DotNetSolutionProjectTypeTests
        {
            [Test]
            public void WhenCreated_ThenSetProperties()
            {
                const string description = "Solution Folder";

                var sut = new DotNetSolutionProjectType(ProjectTypeIds.SolutionFolder, description);

                Assert.That(sut.Id, Is.EqualTo(ProjectTypeIds.SolutionFolder));
                Assert.That(sut.Description, Is.EqualTo(description));
            }
        }

        [TestFixture]
        public class IsSolutionFolder : DotNetSolutionProjectTypeTests
        {
            [Test]
            public void WhenIsSolutionFolder_ThenReturnTrue()
            {
                var sut = new DotNetSolutionProjectType(ProjectTypeIds.SolutionFolder, "Solution Folder");

                Assert.That(sut.IsSolutionFolder, Is.True);
            }

            [Test]
            public void WhenIsNotSolutionFolder_ThenReturnFalse()
            {
                var sut = new DotNetSolutionProjectType(ProjectTypeIds.CSharp, "C#");

                Assert.That(sut.IsSolutionFolder, Is.False);
            }
        }

        [TestFixture]
        public class OverrideEquals : DotNetSolutionProjectTypeTests
        {
            [Test]
            public void WhenOtherIsNull_ThenReturnFalse()
            {
                var sut = new DotNetSolutionProjectType(ProjectTypeIds.SolutionFolder, "Solution Folder");

                var result = sut.Equals(null);

                Assert.That(result, Is.False);
            }

            [Test]
            public void WhenTwoHaveSameReference_ThenReturnTrue()
            {
                var sut = new DotNetSolutionProjectType(ProjectTypeIds.SolutionFolder, "Solution Folder");

                var other = sut;

                var result = sut.Equals(other);

                Assert.That(result, Is.True);
            }

            [Test]
            public void WhenEqualById_ThenReturnTrue()
            {
                var other = new DotNetSolutionProjectType(ProjectTypeIds.SolutionFolder, "Some other");

                var sut = new DotNetSolutionProjectType(ProjectTypeIds.SolutionFolder, "Solution Folder");

                var result = sut.Equals(other);

                Assert.That(result, Is.True);
            }

            [Test]
            public void WhenNotEqualById_ThenReturnFalse()
            {
                var other = new DotNetSolutionProjectType(ProjectTypeIds.CSharp, "Some other");

                var sut = new DotNetSolutionProjectType(ProjectTypeIds.SolutionFolder, "Solution Folder");

                var result = sut.Equals(other);

                Assert.That(result, Is.False);
            }
        }

        [TestFixture]
        public class OverrideGetHashCode : DotNetSolutionProjectTypeTests
        {
            [Test]
            public void WhenEqualIdAndDescription_ThenReturnSameHashCode()
            {
                var other = new DotNetSolutionProjectType(ProjectTypeIds.SolutionFolder, "Solution Folder");

                var sut = new DotNetSolutionProjectType(other.Id, other.Description);

                var result = sut.GetHashCode();

                Assert.That(result, Is.EqualTo(other.GetHashCode()));
            }

            [Test]
            public void WhenNotEqualId_ThenReturnDifferentHashCode()
            {
                var other = new DotNetSolutionProjectType(ProjectTypeIds.SolutionFolder, "Solution Folder");

                var sut = new DotNetSolutionProjectType(ProjectTypeIds.CSharp, other.Description);

                var result = sut.GetHashCode();

                Assert.That(result, Is.Not.EqualTo(other.GetHashCode()));
            }

            [Test]
            public void WhenNotEqualDescription_ThenReturnDifferentHashCode()
            {
                var other = new DotNetSolutionProjectType(ProjectTypeIds.SolutionFolder, "Solution Folder");

                var sut = new DotNetSolutionProjectType(other.Id, "Something else");

                var result = sut.GetHashCode();

                Assert.That(result, Is.Not.EqualTo(other.GetHashCode()));
            }
        }
    }
}