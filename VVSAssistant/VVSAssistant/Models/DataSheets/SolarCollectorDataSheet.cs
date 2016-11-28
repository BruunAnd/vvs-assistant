using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VVSAssistant.Models.DataSheets
{
    [DisplayName(@"Datablad for solpanel")]
    class SolarCollectorDataSheet : DataSheet
    {
        [DisplayName(@"Areal")]
        public float Area { get; set; }
        [DisplayName(@"Energieffektivitet")]
        public float Efficency { get; set; }
        // Solfanger area
        public float Asol { get; set; }
        // Zero loss efficiency
        [DisplayName(@"Effektivitet ved nulbelastning")]
        public float N0 { get; set; }
        // First order heat loss coll
        [DisplayName(@"1. Ordens koefficient")]
        public float a1 { get; set; }
        // Second order heat loss coll
        [DisplayName(@"2. Ordens koefficient")]
        public float a2 { get; set; }
        // Incidence Angle Modifier 
        [DisplayName(@"Indfaldsvinkel korrektionsfaktor")]
        public float IAM { get; set; }
        [Browsable(false)]
        public bool isRoomHeater { get; set; }
        [Browsable(false)]
        public bool isWaterHeater { get; set; }
    }
}
