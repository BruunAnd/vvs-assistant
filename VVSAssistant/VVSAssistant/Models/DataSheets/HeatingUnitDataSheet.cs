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

        [DisplayName(@"Elforbrug i standbytilstand")]
        public float Psb { get; set; }

        //TODO: Maybe find some better names for these two
        //TODO: Make example text appear in the text boxes when creating a new appliance so the user won't enter invalid info.
        [Browsable(false)]
        public bool isRoomHeater { get; set; }
        [Browsable(false)]
        public bool isWaterHeater { get; set; }

        public override string ToString()
        {
            string userProfileString = "";
            string WaterHeatEffString = "";

            if (UseProfile != 0)
                userProfileString = $", forbrugsprofil: {UseProfile}";
            if (WaterHeatingEffiency != 0)
                WaterHeatEffString = $", effektivitet ved vandopvarmning: {WaterHeatingEffiency}%";


            return $"Årsvirkningsgrad: {AFUE}%, Wattforbrug: {WattUsage}kW{userProfileString}{WaterHeatEffString}";
        }
    }
    public enum UseProfileType
    {
        XXXS = 1, XXS, XS, S, M, L, XL, XXL
    }
}
