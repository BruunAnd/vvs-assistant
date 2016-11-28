using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final.Model
{
    class PackagedSolution
    {
        private int _id;
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public virtual List<Appliance> Appliances { get; set; }

        public virtual EnergyLabel EnergyLabel { get; set; }


    }
}
