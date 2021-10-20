using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using ByteDev.DotNet.Solution;
using NUnit.Framework;

namespace ByteDev.DotNet.UnitTests.Solution
{
    [TestFixture]
    public class InvalidDotNetSolutionExceptionTests
    {
        private const string ExMessage = "some message";

        [Test]
        public void WhenNoArgs_ThenSetMessageToDefault()
        {
            var sut = new InvalidDotNetSolutionException();

            Assert.That(sut.Message, Is.EqualTo("The .NET solution was invalid."));
        }

        [Test]
        public void WhenMessageSpecified_ThenSetMessage()
        {
            var sut = new InvalidDotNetSolutionException(ExMessage);

            Assert.That(sut.Message, Is.EqualTo(ExMessage));
        }

        [Test]
        public void WhenMessageAndInnerExSpecified_ThenSetMessageAndInnerEx()
        {
            var innerException = new Exception();

            var sut = new InvalidDotNetSolutionException(ExMessage, innerException);

            Assert.That(sut.Message, Is.EqualTo(ExMessage));
            Assert.That(sut.InnerException, Is.SameAs(innerException));
        }

        [Test]
        public void WhenSerialized_ThenDeserializeCorrectly()
        {
            var sut = new InvalidDotNetSolutionException(ExMessage);

            var formatter = new BinaryFormatter();
            
            using (var stream = new MemoryStream())
            {
                formatter.Serialize(stream, sut);

                stream.Seek(0, 0);

                var result = (InvalidDotNetSolutionException)formatter.Deserialize(stream);

                Assert.That(result.ToString(), Is.EqualTo(sut.ToString()));
            }
        }
    }
}