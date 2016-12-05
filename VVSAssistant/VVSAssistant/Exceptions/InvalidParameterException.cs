using System;

namespace VVSAssistant.Exceptions
{
    internal class InvalidParameterException : Exception
    {
        public InvalidParameterException()
        { }
        public InvalidParameterException(string message) : base(message) { }
        public InvalidParameterException(string message, Exception inner) : base(message, inner) { }
    }
}
