using System;
using System.Runtime.Serialization;

namespace ByteDev.DotNet.Solution
{
    [Serializable]
    public class InvalidDotNetSolutionException : Exception
    {
        public InvalidDotNetSolutionException()
        {
        }

        public InvalidDotNetSolutionException(string message) : base(message)
        {
        }

        public InvalidDotNetSolutionException(string message, Exception inner) : base(message, inner)
        {
        }

        protected InvalidDotNetSolutionException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}