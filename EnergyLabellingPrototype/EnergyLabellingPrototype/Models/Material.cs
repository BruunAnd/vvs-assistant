using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnergyLabellingPrototype.Models
{
    public class Material : UnitPrice
    {
        private int vvsNumber;
        public int VvsNumber
        {
            get
            {
                return vvsNumber;
            }
            set
            {
                if (value != vvsNumber)
                {
                    vvsNumber = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string Name { get; set; }
    }
}
