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
    }
}
