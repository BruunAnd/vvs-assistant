using System;
using System.ComponentModel;

namespace VVSAssistant.Models.DataSheets
{
    [DisplayName(@"Datablad for varmeenhed")]
    public class HeatingUnitDataSheet : DataSheet
    {
        [DisplayName(@"Årsvirkningsgrad ved rumopvarmning")]
        [Description(@"Årsvirkningsgrad i procent")]
        public float AFUE { get; set; }
        [DisplayName(@"Wattforbrug")]
        [Description(@"Varmeenhedens Wattforbrug (Prated) i kW")]
        public float WattUsage { get; set; }
        [DisplayName(@"Virkningsgrad ved lavtemperatursanvendelse")]
        [Description(@"Lavtemperaturs virkninsgrad i procent")]
        public float AFUEColdClima { get; set; }
        [DisplayName(@"Virkningsgrad ved højtemperatursanvendelse")]
        [Description(@"Højtemperaturs virkninsgrad i procent")]
        public float AFUEWarmClima { get; set; }
        [DisplayName(@"Intern temperaturkontrol energiklasse")]
        [Description(@"Energiklassen for en indbygget temperatur kontroller i varme enheden, 
                       intastes som en værdi mellem klasse 1 og 8")]
        public string InternalTempControl { get; set; }

        /* Boiler data for WaterPrimaryBoiler Calculation */
        [DisplayName(@"Energieffektivitet ved vandopvarmning")]
        [Description(@"Kedlens effektivitet ved brugsvandsopvarmning i procent")]
        public float WaterHeatingEffiency { get; set; }
        [DisplayName(@"Forbrugsprofil")]
        [Description(@"Kedlens forbrugsprofil er en værdi mellem M og XXL")]
        public UseProfileType UseProfile { get; set; }

        [DisplayName(@"Stilstandstab")]
        [Description(@"Stilsstandstab (S) for kedlens vandbeholder i Watt (W)")]
        public float StandingLoss { get; set; }
        // Norminal volume
        [DisplayName(@"Vandinhold")] //L
        [Description(@"Kedlens vandbeholder i liter")]
        public float Vnorm { get; set; }

        // Ikke sol-relateret beholder volume
        [DisplayName(@"Ikke-solrelateret beholdervolumen")] //L
        [Description(@"Kedlens eller vandvarmerens ikke-solrelateret beholdervolumen (Vbu) i liter")]
        public float Vbu { get; set; }

        [DisplayName(@"Elforbrug i standbytilstand")] //W
        [Description(@"Kedlens Elforbrug i standbytilstand (Psb) i Watt (W)")]
        public float Psb { get; set; }
        
        public override string ToString()
        {
            var room= "";
            var water = "";
            var userProfileString = "";
            var waterHeatEffString = "";
            var coldwarmString = "";
            var tempconString = "";
            var lossString = "";
            var vnormString = "";
            var vbuString = "";
            var psbString = "";
            
            if (UseProfile != 0)
                userProfileString = $", Forbrugsprofil: {UseProfile}";
            if (Math.Abs(WaterHeatingEffiency) > 0)
                waterHeatEffString = $", Effektivitet ved vandopvarmning: {WaterHeatingEffiency}%";
            if (Math.Abs(AFUEColdClima) > 0)
                coldwarmString = $", Årvirkninsgrad i koldere klimazoner: {AFUEColdClima}%, Årvirkninsgrad i varmere klimazoner: {AFUEWarmClima}%";
            if (InternalTempControl != null)
                tempconString = $", Temperaturregulatorklasse: {InternalTempControl}";
            if (Math.Abs(StandingLoss) > 0)
                lossString = $", Stilstandstab: {StandingLoss}W";
            if (Math.Abs(Vnorm) > 0)
                vnormString = $", Vandindhold: {Vnorm}L";
            if (Math.Abs(Vbu) > 0)
                vbuString = $", Ikke-solrelateret beholdervolumen: {Vbu}L";
            if (Math.Abs(Psb) > 0)
                psbString = $", Elforbrug i standbytilstand {Psb}W";

            return $"Årsvirkningsgrad: {AFUE}%, Wattforbrug: {WattUsage}kW{room}{water}{coldwarmString}{tempconString}{userProfileString}{waterHeatEffString}{lossString}{vnormString}{vbuString}{psbString}";
        }
    }
    public enum UseProfileType
    {
        XXXS = 1, XXS, XS, S, M, L, XL, XXL
    }
}
