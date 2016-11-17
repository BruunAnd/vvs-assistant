using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VVSAssistant.Models
{
    public class Appliance
    {
        public Appliance()
        {
        }

        public string Name { get; set; }

        public string Type { get; set; }

        public string Description { get; set; }


        public System.DateTime CreationDate { get; set; }
        public ApplianceTypes Type { get; set; }
    }
}
