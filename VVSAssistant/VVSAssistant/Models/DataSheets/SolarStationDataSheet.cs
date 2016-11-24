using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VVSAssistant.Models.DataSheets
{
    class SolarStationDataSheet : DataSheet
    {
        public float SolPumpConsumption { get; set; }
        public float SolStandbyConsumption { get; set; }
    }
}
