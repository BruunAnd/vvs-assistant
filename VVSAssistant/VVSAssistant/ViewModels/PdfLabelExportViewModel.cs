using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Xml;
using VVSAssistant.Functions.Calculation;
using VVSAssistant.Models;

namespace VVSAssistant.ViewModels
{
    internal struct ArrowData
    {
        public int Location { get; set; }
        public string Letter { get; set; }
        public string Plus { get; set; }
    }

    internal class PdfLabelExportViewModel
    {
        #region Prop
        private string _labelOne;
        public string LabelOne
        {
            get
            {
                return this._labelOne != null ? _labelOne : "Collapsed";
            }
            set { _labelOne = value; }
        }
        public string CompNameText { get; set;}
        public string TapValue { get; set; }
        public string SolarIncluded { get; set; }
        public string WatertankIncluded { get; set; }
        public string TempControleIncluded { get; set; }
        public string HeaterIncluded { get; set; }

        private int _tabelOneArrow;
        public int TabelOneArrow
        {
            get { return _tabelOneArrow + 1; }
            set { _tabelOneArrow = value; }
        }
        private int _tabelTwoArrow;
        public int TabelTwoArrow
        {
            get { return _tabelTwoArrow + 1; }
            set {
                {
                    _tabelTwoArrow = value;
                }
            }
        }
        public string TabelOneArrowLetter { get; set; }
        public string TabelOneArrowPlus { get; set; }
        public string TabelTwoArrowLetter { get; set; }
        public string TabelTwoArrowPlus { get; set; }
        
        public string WaterHeatingModeLetter { get; set; }
        public string WaterHeatingModePlus { get; set; }
        public string AnnualEfficiencyLetter { get; set; }
        public string AnnualEfficiencyPlus { get; set; }

        private string _labelTwo;
        public string LabelTwo
        {
            get
            {
                return this._labelTwo != null ? _labelTwo : "Collapsed";
            }
            set { _labelTwo = value; }
        }
        public int LabelTwoTabeOneArrow { get; set; }
        public string LabelTwoTabeOneArrowLetter { get; set; }
        public string LabelTwoTabeOneArrowPlus { get; set; }
        #endregion

        public void Setup(PackagedSolution packaged)
        {
            CompNameText = CompanyInfo.CompanyName;

            switch (packaged.EnergyLabel.Count)
            {
                case 1: SetupLabelOne(packaged.EnergyLabel[0]); break;
                case 2: SetupLabelTwo(packaged.EnergyLabel); break;
                default: return;
            }

            SolarIncluded = (packaged.Appliances.Any(item => item.Type == ApplianceTypes.SolarPanel)) ? "Visible" : "Hidden";
            WatertankIncluded = (packaged.Appliances.Any(item => item.Type == ApplianceTypes.Container)) ? "Visible" : "Hidden";
            TempControleIncluded = (packaged.Appliances.Any(item => item.Type == ApplianceTypes.TemperatureController)) ? "Visible" : "Hidden";
            HeaterIncluded = (packaged.EnergyLabel[0].SecondaryBoilerAFUE > 0 || packaged.EnergyLabel[0].SecondaryHeatPumpAFUE > 0)? "Visible" : "Hidden";


        }

        private void SetupLabelOne(EEICalculationResult result)
        {
            LabelOne = "Visible";
            var arrowData = SelectArrowValue(EEICharLabelChooser.EEIChar(ApplianceTypes.Boiler, result.PrimaryHeatingUnitAFUE)[0]);
            AnnualEfficiencyLetter = arrowData.Letter;
            AnnualEfficiencyPlus = arrowData.Plus;

            arrowData = SelectArrowValue(result.EEICharacters);
            LabelTwoTabeOneArrow = arrowData.Location;
            LabelTwoTabeOneArrowLetter = arrowData.Letter;
            LabelTwoTabeOneArrowPlus = arrowData.Plus;

        }
        private void SetupLabelTwo(IReadOnlyList<EEICalculationResult> result)
        {
            LabelTwo = "Visible";
            var arrowData = SelectArrowValue(EEICharLabelChooser.EEIChar(ApplianceTypes.Boiler, result[1].PrimaryHeatingUnitAFUE)[0]);
            AnnualEfficiencyLetter = arrowData.Letter;
            AnnualEfficiencyPlus = arrowData.Plus;

            arrowData = SelectArrowValue(EEICharLabelChooser.EEIChar(ApplianceTypes.Boiler, result[0].PrimaryHeatingUnitAFUE)[0]);
            WaterHeatingModeLetter = arrowData.Letter;
            WaterHeatingModePlus = arrowData.Plus;

            TapValue = result[1].WaterHeatingUseProfile.ToString();


            arrowData = SelectArrowValue(result[0].EEICharacters);
            TabelOneArrow = arrowData.Location;
            TabelOneArrowLetter = arrowData.Letter;
            TabelOneArrowPlus = arrowData.Plus;

            arrowData = SelectArrowValue(result[1].EEICharacters);
            TabelTwoArrow = arrowData.Location;
            TabelTwoArrowLetter = arrowData.Letter;
            TabelTwoArrowPlus = arrowData.Plus;
        }

        private static ArrowData SelectArrowValue(string label)
        {
            int arrowPlace;
            string letterOnArrow;
            string plusOnArrow = string.Empty;
            switch (label)
            {
                case "A+++": letterOnArrow = "A"; plusOnArrow = "+++"; arrowPlace = 1; break;
                case "A++": letterOnArrow = "A"; plusOnArrow = "++"; arrowPlace = 2; break;
                case "A+": letterOnArrow = "A"; plusOnArrow = "+"; arrowPlace = 3; break;
                case "A": letterOnArrow = "A"; arrowPlace = 4; break;
                case "B": letterOnArrow = "B"; arrowPlace = 5; break;
                case "C": letterOnArrow = "C"; arrowPlace = 6; break;
                case "D": letterOnArrow = "D"; arrowPlace = 7; break;
                case "E": letterOnArrow = "E"; arrowPlace = 8; break;
                case "F": letterOnArrow = "F"; arrowPlace = 9; break;
                case "G": letterOnArrow = "G"; arrowPlace = 10; break;
                default: letterOnArrow = "Error"; arrowPlace = 10; break;
            }
            var arrowData = new ArrowData
            {
                Location = arrowPlace,
                Letter = letterOnArrow,
                Plus = plusOnArrow
            };
            return arrowData;
        }
    }
}

