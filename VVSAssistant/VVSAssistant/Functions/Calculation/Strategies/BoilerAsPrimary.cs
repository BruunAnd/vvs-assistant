using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VVSAssistant.Models;
using VVSAssistant.Models.DataSheets;

namespace VVSAssistant.Functions.Calculation.Strategies
{
    class BoilerAsPrimary : IEEICalculation
    {
        private float II;
        private float III;
        private float IV;
        private EEICalculationResult _result = new EEICalculationResult();
        public EEICalculationResult CalculateEEI(PackagedSolution Package)
        {
            var primarydata = Package.PrimaryHeatingUnit.DataSheet as HeatingUnitDataSheet;
            var tempControlData = Package.Appliances.SingleOrDefault(item => 
                           item.Type == ApplianceTypes.TemperatureController)?.
                           DataSheet as TemperatureControllerDataSheet;
            var secondaryBoiler = Package.Appliances.Single(item =>
                           item.Type == ApplianceTypes.Boiler && 
                           item != Package.PrimaryHeatingUnit);
            var supplementaryHeatpump = Package.Appliances.SingleOrDefault(item =>
                            item.Type == ApplianceTypes.Heatpump);

            var solarPanels = Package.Appliances.Where(item =>
                            item.Type == ApplianceTypes.SolarPanel);
            var containers = Package.Appliances.Where(item =>
                            item.Type == ApplianceTypes.Container);
            bool HasNonSolarContainer = Package.Appliances.Where(item =>
                            item.Type == ApplianceTypes.Container &&
                            item != Package.SolarContainer).Count() > 0;

            float AFUE = primarydata.AFUE;
            float tempBonus = TemperatureControllerDataSheet.ClassBonus[tempControlData.Class ?? null];
            float supplementaryBoilerAFUE = (secondaryBoiler?.DataSheet as HeatingUnitDataSheet).AFUE;
            float supplementaryBoiler = (supplementaryBoilerAFUE - primarydata.AFUE) * 0.1f;

            float solarContribution = SolarContribution(solarPanels ?? null, Package.SolarContainer ?? null, Package.PrimaryHeatingUnit);
            float heatpumpContribution = HeatpumpContribution(Package.PrimaryHeatingUnit, supplementaryHeatpump, HasNonSolarContainer);

            float contributionCorrigation = solarContribution > 0 && heatpumpContribution > 0 ?
                  Corrigation(heatpumpContribution, solarContribution) : solarContribution > 0 ? 
                                                    solarContribution : heatpumpContribution;

            var EEI = AFUE + tempBonus + supplementaryBoiler + solarContribution + 
                      heatpumpContribution - contributionCorrigation;
            _result.EEI = EEI;
            _result.PackagedSolutionAtColdTemperaturesAFUE = EEI  + (50 * II);
            _result.EffectOfTemperatureRegulatorClass = tempBonus;
            _result.SecondaryBoilerAFUE = supplementaryBoilerAFUE;
            _result.EffectOfSecondaryBoiler = supplementaryBoiler;
            _result.SolarHeatContribution = solarContribution;
            _result.EffectOfSecondaryHeatPump = heatpumpContribution;
            _result.AdjustedContribution = contributionCorrigation;
            return _result;
        }

        internal float SolarContribution(IEnumerable<Appliance> solarpanels, Appliance solarContainer, Appliance primaryBoiler)
        {
            if (solarpanels == null || solarContainer == null)
                return 0.0f;
            var primaryWattUsage = (primaryBoiler.DataSheet as HeatingUnitDataSheet).WattUsage;
            var solarContainerDatasheet = solarContainer.DataSheet as ContainerDataSheet;
            float solarPanelEfficiency = (solarpanels.First().DataSheet as SolarCollectorDataSheet).Efficency;
            float containerClassification = ContainerDataSheet.ClassificationClass[solarContainerDatasheet.Classification];
            float area = 0;
            foreach (var panel in solarpanels)
            {
                area += (panel.DataSheet as SolarCollectorDataSheet).Area;  
            }
            _result.SolarCollectorArea = area;
            _result.ContainerVolume = solarContainerDatasheet.Volume;
            _result.SolarCollectorEffectiveness = solarPanelEfficiency;
            _result.ContainerClassification = containerClassification;
            III = 294 / (11 * primaryWattUsage);
            IV = 115 / (11 * primaryWattUsage);

            return ((III * area + IV * solarContainerDatasheet.Volume)*0.45f * (solarPanelEfficiency/100)*containerClassification);
        }
        internal float HeatpumpContribution(Appliance heatpump, Appliance primaryBoiler, bool container)
        {
            var heatpumpData = heatpump?.DataSheet as HeatingUnitDataSheet;
            var boilerData = heatpump?.DataSheet as HeatingUnitDataSheet;
            float relationship;
            if (heatpumpData != null && boilerData != null)
                relationship = heatpumpData.WattUsage / (boilerData.WattUsage + heatpumpData.WattUsage);
            else
                return 0;
            II = UtilityClass.GetWeighting(relationship, container, false);
            _result.SecondaryHeatPumpAFUE = heatpumpData.AFUE;
            return (heatpumpData.AFUE - boilerData.AFUE) * II;
        }
        internal float Corrigation(float heatpumpContribution, float solarContribution)
        {
            float value = heatpumpContribution > solarContribution ? solarContribution : heatpumpContribution;
            return (value * 0.5f);
        }
    }
}
