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
    struct ArrowData
    {
        public int location { get; set; }
        public string Letter { get; set; }
        public string Plus { get; set; }
    }
    class PdfLabelExportViewModel
    {
        public string LabelOne { get; set; }
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


        public string LabelTwo { get; set; }
        public int LabelTwoTabeOneArrow { get; set; }
        public string LabelTwoTabeOneArrowLetter { get; set; }
        public string LabelTwoTabeOneArrowPlus { get; set; }


        public void Setup(PackagedSolution packaged, List<EEICalculationResult> result)
        {
            CompNameText = "Hjørring VVS";
            switch (result.Count)
            {
                case 1: SetUpLabelOne(packaged, result[0]); break;
                case 2: SetUpLabelTwo(packaged, result); break;
                default: return;
            }
        }


        private void SetUpLabelTwo(PackagedSolution packaged, List<EEICalculationResult> result)
        {
            ArrowData data;
            LabelTwo = "Visible";
            
            SolarIncluded = (packaged.Appliances.Any(item => item.Type == ApplianceTypes.SolarPanel)) ? "Visible" : "Hidden";
            WatertankIncluded = (packaged.Appliances.Any(item => item.Type == ApplianceTypes.Container)) ? "Visible" : "Hidden";
            TempControleIncluded = (packaged.Appliances.Any(item => item.Type == ApplianceTypes.TemperatureController)) ? "Visible" : "Hidden";
            HeaterIncluded = (packaged.Appliances.Any(item => item.Type == ApplianceTypes.CHP)) ? "Visible" : "Hidden";//Skal kigges på

            TapValue = result[1].WaterHeatingUseProfile.ToString();

            data = SelectArrowValue(result[0].EEICharacters);
            TabelOneArrow = data.location;
            TabelOneArrowLetter = data.Letter;
            TabelOneArrowPlus = data.Plus;

            data = SelectArrowValue(result[1].EEICharacters);
            TabelTwoArrow = data.location;
            TabelTwoArrowLetter = data.Letter;
            TabelTwoArrowPlus = data.Plus;
            
            data = SelectArrowValue(EEICharLabelChooser.EEIChar(ApplianceTypes.Boiler, result[0].PrimaryHeatingUnitAFUE)[0]);
            WaterHeatingModeLetter = data.Letter;
            WaterHeatingModePlus = data.Plus;

            data = SelectArrowValue(EEICharLabelChooser.EEIChar(ApplianceTypes.Boiler, result[1].PrimaryHeatingUnitAFUE)[0]);
            AnnualEfficiencyLetter = data.Letter;
            AnnualEfficiencyPlus = data.Plus;
        }
        private void SetUpLabelOne(PackagedSolution packaged, EEICalculationResult result)
        {
            ArrowData data;
            LabelOne = "Visible";

            SolarIncluded = (packaged.Appliances.Any(item => item.Type == ApplianceTypes.SolarPanel)) ? "Visible" : "Hidden";
            WatertankIncluded = (packaged.Appliances.Any(item => item.Type == ApplianceTypes.Container)) ? "Visible" : "Hidden";
            TempControleIncluded = (packaged.Appliances.Any(item => item.Type == ApplianceTypes.TemperatureController)) ? "Visible" : "Hidden";
            HeaterIncluded = (packaged.Appliances.Any(item => item.Type == ApplianceTypes.CHP)) ? "Visible" : "Hidden";//Skal kigges på
            //HeaterIncluded = (result.))
            
            data = SelectArrowValue(result.EEICharacters);
            LabelTwoTabeOneArrow = data.location;
            LabelTwoTabeOneArrowLetter = data.Letter;
            LabelTwoTabeOneArrowPlus = data.Plus;
            data = SelectArrowValue(EEICharLabelChooser.EEIChar(ApplianceTypes.Boiler, result.PrimaryHeatingUnitAFUE)[0]);
            AnnualEfficiencyLetter = data.Letter;
            AnnualEfficiencyPlus = data.Plus;
        }
        private ArrowData SelectArrowValue(string label)
        {
            int arrowPlace;
            string letterOnArrow;
            string PlusOnArrow = "";
            switch (label)
            {
                case "A+++": letterOnArrow = "A"; PlusOnArrow = "+++"; arrowPlace = 1; break;
                case "A++": letterOnArrow = "A"; PlusOnArrow = "++"; arrowPlace = 2; break;
                case "A+": letterOnArrow = "A"; PlusOnArrow = "+"; arrowPlace = 3; break;
                case "A": letterOnArrow = "A"; arrowPlace = 4; break;
                case "B": letterOnArrow = "B"; arrowPlace = 5; break;
                case "C": letterOnArrow = "C"; arrowPlace = 6; break;
                case "D": letterOnArrow = "D"; arrowPlace = 7; break;
                case "E": letterOnArrow = "E"; arrowPlace = 8; break;
                case "F": letterOnArrow = "F"; arrowPlace = 9; break;
                case "G": letterOnArrow = "G"; arrowPlace = 10; break;
                default: letterOnArrow = "Error"; arrowPlace = 10; break;
            }
            ArrowData ad = new ArrowData();
            ad.location = arrowPlace;
            ad.Letter = letterOnArrow;
            ad.Plus = PlusOnArrow;
            return ad;
        }
    }
    }

