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
    class PdfCalculationViewModel
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


        public void SetUp(PdfCalculationViewModel vm, List<EEICalculationResult> results)
        {
            EEICalculationResult result = results[0];
            switch (result.CalculationType)
            {
                case CalculationType.PrimaryBoiler: CalPageTwo(vm, result); break;
                case CalculationType.PrimaryCPH: CalPageThree(vm, result); break;
                case CalculationType.PrimaryHeatPump: CalPageOne(vm, result); break;
                case CalculationType.PrimaryLowTempHeatPump: CalPageFour(vm, result); break;
                case CalculationType.PrimaryWaterBoiler: break;
                default: return;
            }

            vm.PrimAnnualEfficiency = CheckforNull(result.PrimaryHeatingUnitAFUE);
            vm.TemperatureControleclass = CheckforNull(result.EffectOfTemperatureRegulatorClass);
            vm.SupBoilerAnnualEfficiency = CheckforNull(result.SecondaryBoilerAFUE);
            vm.SupBoilerTotal = CheckforNull(result.EffectOfSecondaryBoiler);

            vm.SolarM2 = CheckforNull(result.SolarCollectorArea);
            vm.SolarM3 = CheckforNull(result.ContainerVolume);
            vm.SolarEfficiency = CheckforNull(result.SolarCollectorEffectiveness);//
            vm.SolarClass = CheckforNull(result.ContainerClassification);//
            vm.SolarTotal = CheckforNull(result.SolarHeatContribution);

            vm.PackagedAnnualEfficiencyEEILabel = SelectValue(result.EEICharacters);
            vm.ResultOne = CheckforNull(result.PackagedSolutionAtColdTemperaturesAFUE);
        }
        private string CheckforNull(float value)
        {
            if (value == 0)
            {
                return "";
            }
            return Math.Round(value, 2).ToString();

        }

        private void CalPageOne(PdfCalculationViewModel vm, EEICalculationResult result)
        {
            vm.PageOne = "Visible";
            vm.ResultTwo = CheckforNull(result.PackagedSolutionAtWarmTemperaturesAFUE);
            vm.PackagedAnnualEfficiencyAverageClima = CheckforNull(result.EEI);
        }
        private void CalPageTwo(PdfCalculationViewModel vm, EEICalculationResult result)
        {
            vm.PageTwo = "Visible";
            vm.SupHeatingUnitAnnualEfficiency = CheckforNull(result.EffectOfSecondaryHeatPump);
            vm.SupHeatingUnitTotal = CheckforNull(result.SecondaryHeatPumpAFUE);
            vm.SolarContributionAndSupHeatingUnitTotal = CheckforNull(result.AdjustedContribution);
            vm.PackagedAnnualEfficiencyRoomHeating = CheckforNull(result.EEI);
        }
        private void CalPageThree(PdfCalculationViewModel vm, EEICalculationResult result)
        {
            vm.PageThree = "Visible";
            vm.PackagedAnnualEfficiencyRoomHeating = CheckforNull(result.EEI);
        }
        private void CalPageFour(PdfCalculationViewModel vm, EEICalculationResult result)
        {
            vm.PageFour = "Visible";
            vm.PackagedAnnualEfficiencyAverageClima = "N/A";
        }
        private void CalPageFive(PdfCalculationViewModel vm, EEICalculationResult result)
        {
            vm.PageFive = "Visible";
            vm.UseProfile = "N/A";
            vm.AnnualWaterheatingEfficiency = "N/A";
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
                case "D": value = 4; break;
                case "E": value = 3; break;
                case "F": value = 2; break;
                case "G": value = 1; break;
                default: value = 0; break;
            }
            return value;
        }
    }
}
