using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VVSAssistant.Models
{
    public class OfferedAppliance : UnitPrice
    {
        public OfferedAppliance(Appliance appliance)
        {
            Quantity = 1;
            UnitCostPrice = appliance.DataSheet.Price;
            Name = appliance.Name;
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged(); }
        }
    }
}
