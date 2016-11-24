using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VVSAssistant.Models.DataSheets
{
    class SolarCollectorDataSheet : DataSheet
    {
        public float Area { get; set; }
        public float Efficency { get; set; }
        // Solfanger area
        public float Asol { get; set; }
        // Zero loss efficiency
        public float N0 { get; set; }
        // First order heat loss coll
        public float a1 { get; set; }
        // Second order heat loss coll
        public float a2 { get; set; }
        // Incidence Angle Modifier 
        public float IAM { get; set; }
    }
}
