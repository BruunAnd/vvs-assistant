using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VVSAssistant.Models
{
    
    public class PackageSolution
    {
        public PackageSolution()
        {
            Appliances = new ObservableCollection<Appliance>();
        }

        public string Name { get; set; }

        public ObservableCollection<Appliance> Appliances
        {
            get; private set;
        }
    }
}
