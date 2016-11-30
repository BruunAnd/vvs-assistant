using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using VVSAssistant.Functions.Calculation;

namespace VVSAssistant.ViewModels
{
    internal class PdfCalculationViewModel
    {

        private string _pageOne;
        public string PageOne
        {
            get
            {
                return this._pageOne != null ? _pageOne : "Collapsed";
            }
            set { _pageOne = value; }
        }
        private string _pageTwo;
        public string PageTwo
        {
            get
            {
                return this._pageTwo != null ? _pageTwo : "Collapsed";
            }
            set { _pageTwo = value; }
        }

        private string _pageThree;
        public string PageThree
        {
            get
            {
                return this._pageThree != null ? _pageThree : "Collapsed";
                
            }
            set { _pageThree = value; }
        }
        private string _pageFour;

        public string PageFour
        {
            get { return this._pageFour != null ? _pageFour : "Collapsed"; }
            set { _pageFour = value; }
        }

        private string _pageFive;

        public string PageFive
        {
            get { return this._pageFive != null ? _pageFive : "Collapsed"; }
            set { _pageFive = value; }
        }


        //Page 1

        public string PackagedAnnualEfficiencyAverageClima { get; set; } // Page 4 og 1

        public string ResultTwo { get; set; }
        //page 2
        public string SupHeatingUnitAnnualEfficiency { get; set; }
        public string SupHeatingUnitTotal { get; set; }

        public string SolarContributionAndSupHeatingUnitTotal { get; set; }

        public string PackagedAnnualEfficiencyRoomHeating { get; set; } // page 3 og 2


        //Page 5

        public string UseProfile { get; set; }
        public string AnnualWaterheatingEfficiency { get; set; }

        //Fælles
        public string PrimAnnualEfficiency { get; set; }

        public string TemperatureControleclass { get; set; }

        public string SupBoilerAnnualEfficiency { get; set; }
        public string SupBoilerTotal { get; set; }

        public string SolarM2 { get; set; }
        public string SolarM3 { get; set; }
        public string SolarEfficiency { get; set; }
        public string SolarClass { get; set; }
        public string SolarTotal { get; set; }

        public int PackagedAnnualEfficiencyEEILabel { get; set; }
        public string ResultOne { get; set; }

        private int _packagedAnnualEfficiencyEEILabelWater;
        public int PackagedAnnualEfficiencyEEILabelWater
        {
            get { return _packagedAnnualEfficiencyEEILabelWater + 1; }
            set { _packagedAnnualEfficiencyEEILabelWater = value; }
        }

        private int _packagedWaterUseprofile;
        public int PackagedWaterUseprofile
        {
            get { return _packagedWaterUseprofile + 2; }
            set { _packagedWaterUseprofile = value; }
        }

        public void Setup(List<EEICalculationResult> results)
        {
            var result = results[0];
            switch (result.CalculationType)
            {
                case CalculationType.PrimaryBoiler: PageTwo = "Visible"; ; break;
                case CalculationType.PrimaryCPH: PageThree = "Visible"; break;
                case CalculationType.PrimaryHeatPump: PageOne = "Visible"; SetupPageOneSecResult(result); break;
                case CalculationType.PrimaryLowTempHeatPump: PageFour = "Visible"; SetupPageOneSecResult(result); break;
                default: return;
            }

            BasicSetup(result);
        }

        public void SetupSpecialPage(List<EEICalculationResult> results)
        {
            BasicSetup(results[0]);
            SetupPageFiveSpecialInfo(results);

        }
        private string CheckIfZero(float value)
        {
            if (value <= 0)
            {
                return "";
            }
            return Math.Round(value, 2).ToString();
        }
        private void BasicSetup(EEICalculationResult result)
        {
            PrimAnnualEfficiency = CheckIfZero(result.PrimaryHeatingUnitAFUE);
            TemperatureControleclass = CheckIfZero(result.EffectOfTemperatureRegulatorClass);
            SupBoilerAnnualEfficiency = CheckIfZero(result.SecondaryBoilerAFUE);
            SupBoilerTotal = CheckIfZero(result.EffectOfSecondaryBoiler);

            SolarM2 = CheckIfZero(result.SolarCollectorArea);
            SolarM3 = CheckIfZero(result.ContainerVolume);
            SolarEfficiency = CheckIfZero(result.SolarCollectorEffectiveness);
            SolarClass = CheckIfZero(result.ContainerClassification);
            SolarTotal = CheckIfZero(result.SolarHeatContribution);

            PackagedAnnualEfficiencyEEILabel = SelectValue(result.EEICharacters);

            PackagedAnnualEfficiencyRoomHeating = CheckIfZero(result.EEI);
            SupHeatingUnitAnnualEfficiency = CheckIfZero(result.EffectOfSecondaryHeatPump);
            SupHeatingUnitTotal = CheckIfZero(result.SecondaryHeatPumpAFUE);
            SolarContributionAndSupHeatingUnitTotal = CheckIfZero(result.AdjustedContribution);

            PackagedAnnualEfficiencyAverageClima = CheckIfZero(result.EEI);

            ResultOne = CheckIfZero(result.PackagedSolutionAtColdTemperaturesAFUE);
        }

        private void SetupPageOneSecResult(EEICalculationResult result)
        {   
            ResultTwo = CheckIfZero(result.PackagedSolutionAtWarmTemperaturesAFUE);
        }
        private void SetupPageFiveSpecialInfo(List<EEICalculationResult> results)
        {
            PageFive = "Visible";
            UseProfile = results[1].WaterHeatingUseProfile.ToString();
            AnnualWaterheatingEfficiency = CheckIfZero(results[1].WaterHeatingEffciency);
            SolarTotal = CheckIfZero(results[1].SolarHeatContribution);
            PackagedAnnualEfficiencyAverageClima = CheckIfZero(results[1].EEI);
            PackagedAnnualEfficiencyEEILabelWater = SelectValue(results[1].EEICharacters);
            PackagedWaterUseprofile = SelectValue(results[1].WaterHeatingUseProfile.ToString());
        }
        private int SelectValue(string label)
        {
            int value;
            switch (label)
            {
                case "A+++": value = 10; break;
                case "A++": value = 9; break;
                case "A+": value = 8; break;
                case "A": value = 7; break;
                case "B": value = 6; break;
                case "C": value = 5; break;
                case "XXL":
                case "D": value = 4; break;
                case "XL":
                case "E": value = 3; break;
                case "L":
                case "F": value = 2; break;
                case "M":
                case "G": value = 1; break;
                default: value = 1; break;
            }
            return value;
        }
    }
}
