using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VVSAssistant.Models.DataSheets
{
    /* Class only for Boiler Water strategy, since it requires data no other strategy requires,
     * Might aswell combine the data here */
    class WaterHeatingUnitDataSheet : DataSheet
    {
        /* OBS skal refaktorers ud i de andre datasheets OBS */

        /* Boiler data VAND VARMER */
        public float WaterHeatingEffiency { get; set; }
        public UseProfileType UseProfile { get; set; }
        // Ikke sol-relateret beholder volume
        public float Vbu { get; set; }

        //SOLCAL method Parameters

        /*Solar Panel Data*/
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

        public float Qaux { get; set; }
        public float Qnonsol { get; set; }
        
        /* Container Data */
        public float Volume { get; set; }

        public float StandingLoss { get; set; }

        /* Solar Station */
        public float SolPumpConsumption { get; set; }
        public float SolStandbyConsumption { get; set; }

    }
}
