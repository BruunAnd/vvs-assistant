using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final.Model
{
    class Client
    {
        private DateTime _creationDate;

        public DateTime CreationDate
        {
            get { return _creationDate; }
            set { _creationDate = value; }
        }

        public virtual ClientInformation ClientInformation { get; set; }
    }
}
