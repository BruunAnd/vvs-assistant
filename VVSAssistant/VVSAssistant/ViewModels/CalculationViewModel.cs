using System;
using System.Collections.Generic;
using System.Globalization;
using VVSAssistant.Functions.Calculation;

namespace VVSAssistant.ViewModels
{
    internal class CalculationViewModel
    {
        #region Prop
        private string _pageOne;
        public string PageOne
        {
            get
            {
                return _pageOne ?? "Collapsed";
            }
            set { _pageOne = value; }
        }
        private string _pageTwo;
        public string PageTwo
        {
            get
            {
                return _pageTwo ?? "Collapsed";
            }
            set { _pageTwo = value; }
        }

        private string _pageThree;
        public string PageThree
        {
            get
            {
                return _pageThree ?? "Collapsed";
                
            }
            set { _pageThree = value; }
        }
        private string _pageFour;

        public string PageFour
        {
            get { return _pageFour ?? "Collapsed"; }
            set { _pageFour = value; }
        }

        private string _pageFive;

        public string PageFive
        {
            get { return _pageFive ?? "Collapsed"; }
            set { _pageFive = value; }
        }

        public List<EEICalculationResult> Results { get; set; }

        public string PackagedAnnualEfficiencyAverageClima { get; set; } 

        public string ResultTwo { get; set; }

        public string SupHeatingUnitAnnualEfficiency { get; set; }
        public string SupHeatingUnitTotal { get; set; }

        public string SolarContributionAndSupHeatingUnitTotal { get; set; }

        public string PackagedAnnualEfficiencyRoomHeating { get; set; }


        public string UseProfile { get; set; }
        public string AnnualWaterheatingEfficiency { get; set; }


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
        #endregion

        public CalculationViewModel(List<EEICalculationResult> results)
        {
            Results = results;
            var result = Results[0];

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
            SupHeatingUnitAnnualEfficiency = CheckIfZero(result.SecondaryHeatPumpAFUE);
            SupHeatingUnitTotal = CheckIfZero(result.EffectOfSecondaryHeatPump);
            SolarContributionAndSupHeatingUnitTotal = CheckIfZero(result.AdjustedContribution);

            PackagedAnnualEfficiencyAverageClima = CheckIfZero(result.EEI);

            ResultOne = CheckIfZero(result.PackagedSolutionAtColdTemperaturesAFUE);
        }
        public void Setup()
        {
            var result = Results[0];
            switch (result.CalculationType)
            {
                case CalculationType.PrimaryBoiler: PageTwo = "Visible"; break;
                case CalculationType.PrimaryCHP: PageThree = "Visible"; break;
                case CalculationType.PrimaryHeatPump: PageOne = "Visible"; SetupPageOneSecResult(result); break;
                case CalculationType.PrimaryLowTempHeatPump: PageFour = "Visible"; SetupPageOneSecResult(result); break;
                default: return;
            }
        }

        public void SetupSpecialPage()
        {
                PageFive = "Visible";
                UseProfile = Results[1].WaterHeatingUseProfile.ToString();
                AnnualWaterheatingEfficiency = CheckIfZero(Results[1].WaterHeatingEffciency);
                SolarTotal = CheckIfZero(Results[1].SolarHeatContribution);
                PackagedAnnualEfficiencyAverageClima = CheckIfZero(Results[1].EEI);
                PackagedAnnualEfficiencyEEILabelWater = SelectValue(Results[1].EEICharacters);
                PackagedWaterUseprofile = SelectValue(Results[1].WaterHeatingUseProfile.ToString());
        }
        private string CheckIfZero(float value)
        {

            return Math.Abs(value) <= 0 ? "" : Math.Round(value, 2).ToString(CultureInfo.CurrentCulture);
        }

        private void SetupPageOneSecResult(EEICalculationResult result)
        {
            ResultTwo = CheckIfZero(result.PackagedSolutionAtWarmTemperaturesAFUE);
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
