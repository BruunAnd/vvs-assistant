using System;
using System.Collections.Generic;

namespace VVSAssistant.Models.DataSheets
{
    public class HeatingUnitDataSheet : DataSheet
    {
        public float AFUE { get; set; }
        public float WattUsage { get; set; }
        public float AFUEColdClima { get; set; }
        public float AFUEWarmClima { get; set; }
        public string InternalTempControl { get; set; }

        /* Boiler data for WaterPrimaryBoiler Calculation */
        public float WaterHeatingEffiency { get; set; }
        public UseProfileType UseProfile { get; set; }
        // Ikke sol-relateret beholder volume
        public float Vbu { get; set; }
    }
    public enum UseProfileType
    {
        XXXS = 1, XXS, XS, S, M, L, XL, XXL
    }
}
