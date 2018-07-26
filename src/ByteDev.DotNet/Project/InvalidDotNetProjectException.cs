using System;
using System.Runtime.Serialization;

namespace ByteDev.DotNet.Project
{
    [Serializable]
    public class InvalidDotNetProjectException : Exception
    {
        public InvalidDotNetProjectException()
        {
        }

        public InvalidDotNetProjectException(string message) : base(message)
        {
        }

        public InvalidDotNetProjectException(string message, Exception inner) : base(message, inner)
        {
        }

        protected InvalidDotNetProjectException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}