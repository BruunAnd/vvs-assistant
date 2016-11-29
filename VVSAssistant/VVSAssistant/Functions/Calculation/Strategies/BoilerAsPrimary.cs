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
        private EEICalculationResult _result = new EEICalculationResult();
        private PackagedSolution _package;
        private PackageDataManager _packageData;
        float II;
        /// <summary>
        /// EEI Calculation for packaged solution with a boiler as primary heating unit
        /// </summary>
        /// <param name="Package"></param>
        /// <returns>EEICalculationResult which are the EEI and all the varibales in between</returns>
        public EEICalculationResult CalculateEEI(PackagedSolution Package)
        {
            _package = Package;
            _packageData = new PackageDataManager(_package);
            if (PrimaryBoiler == null)
                return null;

            _result.PrimaryHeatingUnitAFUE = PrimaryBoiler.AFUE;
            _result.SecondaryBoilerAFUE = _packageData.SupplementaryBoiler?.AFUE ?? 0;
            _result.EffectOfTemperatureRegulatorClass = _packageData.TempControllerBonus;
            _result.EffectOfSecondaryBoiler = _packageData.SupplementaryBoiler != null ?
                    (_packageData.SupplementaryBoiler.AFUE - _result.PrimaryHeatingUnitAFUE) * 0.1f :
                    0;
            _result.SolarHeatContribution = SolarContribution();
            _result.EffectOfSecondaryHeatPump = -HeatpumpContribution(_packageData.HasNonSolarContainer());
            _result.AdjustedContribution = _result.EffectOfSecondaryHeatPump != 0 && _result.SolarHeatContribution != 0
                                           ? AdjustedContribution(_result.EffectOfSecondaryHeatPump, _result.SolarHeatContribution)
                                           : default(float);

            _result.EEI = _result.PrimaryHeatingUnitAFUE + _result.EffectOfTemperatureRegulatorClass
                          - _result.EffectOfSecondaryBoiler + _result.SolarHeatContribution -
                          _result.EffectOfSecondaryHeatPump - _result.AdjustedContribution;
            _result.PackagedSolutionAtColdTemperaturesAFUE = II != default(float) ? _result.EEI + (50 * II) : 0;
            _result.EEICharacters = EEICharLabelChooser.EEIChar(ApplianceTypes.Boiler, _result.EEI, 1)[0];
            _result.ToNextLabel = EEICharLabelChooser.EEIChar(ApplianceTypes.Boiler, _result.EEI, 1)[1];
            _result.CalculationType = _packageData.CalculationStrategyType(_package, _result);
            return _result;
        }

        /* Calculates the Solar contribution using the area of the solar panels,
         * the volume of the solar containers and the watt usage of the primary
         * Heating unit */
        private float SolarContribution()
        {
            var SolarCollectorData = _packageData.SolarPanelData;
            float solarPanelArea = _packageData.SolarPanelArea(panel =>
                                    panel.isRoomHeater == true);
            float solarContainerVolume = _packageData.SolarContainerVolume(container =>
                                         !container.isWaterContainer);

            _result.ContainerVolume = solarContainerVolume / 1000;
            _result.SolarCollectorArea = solarPanelArea;
            float ans = 0;
            float III = 294 / (11 * PrimaryBoiler.WattUsage);
            float IV = 115 / (11 * PrimaryBoiler.WattUsage);

            // Assume only one type of solarpanel pr. package
            if (solarPanelArea != 0 && solarContainerVolume > 0)
            {
                ans = (III * solarPanelArea + IV * (solarContainerVolume / 1000)) *
                    0.9f * (SolarCollectorData.Efficency / 100) * _packageData.SolarContainerClass;
            }
            return ans;
        }

        /* Calculates the Heatpump contribution using the relationship between the heatpump
         * and primary boiler, and performing linear interpolation on the table values
         * defined in the calculation documents */
         // bool container defines whether the package non solar container
        private float HeatpumpContribution(bool container)
        {
            var heatpumpData = _packageData.SupplementaryHeatPump;
            if (heatpumpData == null)
                return 0f;
            float relationship;
            if (heatpumpData != null)
                relationship = heatpumpData.WattUsage / (PrimaryBoiler.WattUsage + heatpumpData.WattUsage);
            else
                return 0;
            II = UtilityClass.GetWeighting(relationship, container, false);
            _result.SecondaryHeatPumpAFUE = heatpumpData.AFUE;
            return (heatpumpData.AFUE - PrimaryBoiler.AFUE) * II;
        }
        // Adjustes the contribution from the HeatPump and the solar system
        private float AdjustedContribution(float heatpumpContribution, float solarContribution)
        {
            float value = -heatpumpContribution > solarContribution ? solarContribution : heatpumpContribution;
            return (value * 0.5f);
        }
        private HeatingUnitDataSheet PrimaryBoiler { get {return _package?.PrimaryHeatingUnit?.
                                                      DataSheet as HeatingUnitDataSheet; } }
    }
}
