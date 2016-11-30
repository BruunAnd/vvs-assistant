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
        [DisplayName(@"Vandinhold")] //L
        public float Vnorm { get; set; }

        // Ikke sol-relateret beholder volume
        [DisplayName(@"Ikke-solrelateret beholdervolumen")] //L
        public float Vbu { get; set; }

        [DisplayName(@"Elforbrug i standbytilstand")] //W
        public float Psb { get; set; }

        //TODO: Maybe find some better names for these two
        //TODO: Make example text appear in the text boxes when creating a new appliance so the user won't enter invalid info.
        [Browsable(false)]
        public bool isRoomHeater { get; set; }
        [Browsable(false)]
        public bool isWaterHeater { get; set; }

        public override string ToString()
        {
            string room= "";
            string water = "";
            string userProfileString = "";
            string WaterHeatEffString = "";
            string coldwarmString = "";
            string tempconString = "";
            string lossString = "";
            string vnormString = "";
            string vbuString = "";
            string psbString = "";

            if (isRoomHeater == true)
                room = ", Rumopvarmning";
            if (isWaterHeater == true)
                water = ", Vandopvarmning";
            if (UseProfile != 0)
                userProfileString = $", Forbrugsprofil: {UseProfile}";
            if (WaterHeatingEffiency != 0)
                WaterHeatEffString = $", Effektivitet ved vandopvarmning: {WaterHeatingEffiency}%";
            if (AFUEColdClima != 0)
                coldwarmString = $", Årvirkninsgrad i koldere klimazoner: {AFUEColdClima}%, Årvirkninsgrad i varmere klimazoner: {AFUEWarmClima}%";
            if (InternalTempControl != null)
                tempconString = $", Temperaturregulatorklasse: {InternalTempControl}";
            if (StandingLoss != 0)
                lossString = $", Stilstandstab: {StandingLoss}W";
            if (Vnorm != 0)
                vnormString = $", Vandindhold: {Vnorm}L";
            if (Vbu != 0)
                vbuString = $", Ikke-solrelateret beholdervolumen: {Vbu}L";
            if (Psb != 0)
                psbString = $", Elforbrug i standbytilstand {Psb}W";

            return $"Årsvirkningsgrad: {AFUE}%, Wattforbrug: {WattUsage}kW{room}{water}{coldwarmString}{tempconString}{userProfileString}{WaterHeatEffString}{lossString}{vnormString}{vbuString}{psbString}";
        }
    }
    public enum UseProfileType
    {
        XXXS = 1, XXS, XS, S, M, L, XL, XXL
    }
}
