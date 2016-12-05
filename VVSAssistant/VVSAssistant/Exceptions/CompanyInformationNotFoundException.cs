using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VVSAssistant.Exceptions
{
    class CompanyInformationNotFoundException : Exception
    {
        public CompanyInformationNotFoundException() : base() { }
        public CompanyInformationNotFoundException(string message) : base(message) { }
        public CompanyInformationNotFoundException(string message, System.Exception inner) : base(message, inner) { }
    }
}
