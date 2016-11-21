using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VVSAssistant.Exceptions
{
    class InvalidParameterException : Exception
    {
        public InvalidParameterException() : base() { }
        public InvalidParameterException(string message) : base(message) { }
        public InvalidParameterException(string message, System.Exception inner) : base(message, inner) { }
    }
}
