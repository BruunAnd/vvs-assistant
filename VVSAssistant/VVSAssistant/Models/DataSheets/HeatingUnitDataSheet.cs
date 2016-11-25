using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace VVSAssistant.Models.DataSheets
{
    [DisplayName(@"Datablad for varmeenhed")]
    public class HeatingUnitDataSheet : DataSheet
    {
        [DisplayName(@"Årsvirkningsgrad ved rumopvarmning")]
        public float AFUE { get; set; }
        [DisplayName(@"Wattforbrug")]
        public float WattUsage { get; set; }
        [DisplayName(@"Virkningsgrad ved lavtemperatursanvendelse")]
        public float AFUEColdClima { get; set; }
        [DisplayName(@"Virkningsgrad ved højtemperatursanvendelse")]
        public float AFUEWarmClima { get; set; }
        [DisplayName(@"Intern temperaturkontrol energiklasse")]
        public string InternalTempControl { get; set; }

        /* Boiler data for WaterPrimaryBoiler Calculation */
        [DisplayName(@"Energieffektivitet ved vandopvarmning")]
        public float WaterHeatingEffiency { get; set; }
        [DisplayName(@"Forbrugsprofil")]
        public UseProfileType UseProfile { get; set; }

        [DisplayName(@"Stilstandstab")]
        public float StandingLoss { get; set; }
        // Norminal volume
        [DisplayName(@"Vandinhold")]
        public float Vnorm { get; set; }

        // Ikke sol-relateret beholder volume
        [DisplayName(@"Ikke-solrelateret beholdervolumen")]
        public float Vbu { get; set; }
    }
    public enum UseProfileType
    {
        XXXS = 1, XXS, XS, S, M, L, XL, XXL
    }
}
