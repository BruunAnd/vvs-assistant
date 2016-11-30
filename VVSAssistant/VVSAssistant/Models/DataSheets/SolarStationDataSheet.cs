using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VVSAssistant.Models.DataSheets
{
    [DisplayName(@"Datablad for solstation")]
    class SolarStationDataSheet : DataSheet
    {
        [DisplayName(@"Elforbrug til pumpe")]
        [Description(@"Elforbrug til pumpe (solpump) i watt (W)")]
        public float SolPumpConsumption { get; set; }
        [DisplayName(@"Elforbrug i standbytilstand")]
        [Description(@"Elforbtug i standbytilstand (solstandby) i watt (W)")]
        public float SolStandbyConsumption { get; set; }

        public override string ToString()
        {
            return $"Elforbrug til pumpe: {SolPumpConsumption}W, Elforbrug i standbytilstand: {SolStandbyConsumption}W";
        }
    }
}
