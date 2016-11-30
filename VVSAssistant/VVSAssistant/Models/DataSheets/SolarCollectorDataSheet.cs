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
        [Description(@"Arealet for en enkelt solfanger i kvadratmeter")]
        public float Area { get; set; }
        [DisplayName(@"Energieffektivitet")]
        [Description(@"Solfangereeffektivitet i procent")]
        public float Efficency { get; set; }
        // Zero loss efficiency
        [DisplayName(@"Effektivitet ved nulbelastning")]
        public float N0 { get; set; }
        // First order heat loss coll
        [DisplayName(@"1. Ordens koefficient")]// w/(m^2 K)
        [Description(@"Værdi i W/(m^2K)")]
        public float a1 { get; set; }
        // Second order heat loss coll
        [DisplayName(@"2. Ordens koefficient")]
        [Description(@"Værdi i W/(m^2K^2)")]
        public float a2 { get; set; }
        // Incidence Angle Modifier 
        [DisplayName(@"Indfaldsvinkel korrektionsfaktor")]
        public float IAM { get; set; }
        [Browsable(false)]
        public bool isRoomHeater { get; set; }
        [Browsable(false)]
        public bool isWaterHeater { get; set; }

        public override string ToString()
        {
            string room = "";
            string water = "";
            string N0String = "";
            string a1String = "";
            string a2String = "";
            string IAMString = "";
            if (isRoomHeater == true)
                room = ", rumopvarmning";
            if (isWaterHeater == true)
                water = ", vandopvarmning";
            if (N0 != 0)
                N0String = $", Effektivitet ved nulbelastning: {N0}";
            if (a1 != 0)
                a1String = $", 1. Ordens koefficient: {a1} W/(m\u00b2 K)";
            if (a2 != 0)
                a2String = $", 2. Ordens koefficient: {a2}";
            if (IAM != 0)
                IAMString = $", Indfaldsvinkel korrektionsfaktor: {IAM}";




            return $"Areal: {Area}m\u00b2, Energieffektivitet: {Efficency}%{room}{water}{N0String}{a1String}{a2String}{IAMString}";
        }
    }
}
