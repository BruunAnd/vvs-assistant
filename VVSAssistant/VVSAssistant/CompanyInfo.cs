using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VVSAssistant
{
    public static class CompanyInfo
    {
        private static string _address;
        public static string Address { get { return _address; } set { _address = value; } }

        private static string _telephone;
        public static string Telephone { get { return _telephone; } set { _telephone = value; } }

        private static string _CVR;
        public static string CVR { get { return _CVR; } set { _CVR = value; } }

        private static string _email;
        public static string Email { get { return _email; } set { _email = value; } }

        private static string _website;
        public static string Website { get { return _website; } set { _website = value; } }

        private static string _companyName;
        public static string CompanyName { get { return _companyName; } set { _companyName = value; } }
    }
}
