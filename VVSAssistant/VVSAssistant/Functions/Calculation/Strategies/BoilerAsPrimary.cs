using System;
using VVSAssistant.Models;
using VVSAssistant.Models.DataSheets;

namespace VVSAssistant.Functions.Calculation.Strategies
{
    internal class BoilerAsPrimary : IEEICalculation
    {
        private readonly EEICalculationResult _result = new EEICalculationResult();
        private PackagedSolution _package;
        private PackageDataManager _packageData;
        private float _ii;
        /// <summary>
        /// EEI Calculation for packaged solution with a boiler as primary heating unit
        /// </summary>
        /// <param name="package"></param>
        /// <returns>EEICalculationResult which are the EEI and all the varibales in between</returns>
        public EEICalculationResult CalculateEEI(PackagedSolution package)
        {
            _package = package;
            _packageData = new PackageDataManager(_package);
            if (PrimaryBoiler == null)
                return null;

            _result.PrimaryHeatingUnitAFUE = PrimaryBoiler.AFUE;
            _result.SecondaryBoilerAFUE = _packageData.SupplementaryBoiler?.AFUE ?? 0;
            _result.EffectOfTemperatureRegulatorClass = _packageData.TempControllerBonus;
            _result.EffectOfSecondaryBoiler = (_packageData.SupplementaryBoiler?.AFUE - _result.PrimaryHeatingUnitAFUE) * 0.1f ?? 0;
            _result.SolarHeatContribution = SolarContribution();
            _result.SecondaryHeatPumpAFUE = _packageData.SupplementaryHeatPump?.AFUE ?? 0;
            _result.EffectOfSecondaryHeatPump = -HeatpumpContribution(_packageData.HasNonSolarContainer());
            _result.AdjustedContribution = Math.Abs(Math.Abs(_result.EffectOfSecondaryHeatPump)) > 0 && Math.Abs(_result.SolarHeatContribution) > 0
                                           ? AdjustedContribution(_result.EffectOfSecondaryHeatPump, _result.SolarHeatContribution)
                                           : default(float);

            _result.EEI = (float)Math.Round(_result.PrimaryHeatingUnitAFUE + _result.EffectOfTemperatureRegulatorClass
                          - _result.EffectOfSecondaryBoiler + _result.SolarHeatContribution -
                          _result.EffectOfSecondaryHeatPump - _result.AdjustedContribution);
            _result.PackagedSolutionAtColdTemperaturesAFUE = Math.Abs(_ii - default(float)) > 0 ? _result.EEI + (50 * _ii) : 0;
            _result.EEICharacters = EEICharLabelChooser.EEIChar(ApplianceTypes.Boiler, _result.EEI, 1)[0];
            _result.ToNextLabel = EEICharLabelChooser.EEIChar(ApplianceTypes.Boiler, _result.EEI, 1)[1];
            _result.ProceedingEEICharacter = EEICharLabelChooser.EEIChar(ApplianceTypes.Boiler, _result.EEI, 1)[2];
            _result.CalculationType = _packageData.CalculationStrategyType(_package, _result);
            return _result;
        }

        /* Calculates the Solar contribution using the area of the solar panels,
         * the volume of the solar containers and the watt usage of the primary
         * Heating unit */
        private float SolarContribution()
        {
            var solarCollectorData = _packageData.SolarPanelData(item => item.IsRoomHeater == true);
            var solarPanelArea = _packageData.SolarPanelArea(panel =>
                                    panel.IsRoomHeater);
            var solarContainerVolume = _packageData.SolarContainerVolume(container =>
                                         !container.IsWaterContainer);

            _result.ContainerClassification = _packageData.SolarContainerClass;
            _result.SolarCollectorEffectiveness = solarCollectorData?.Efficency ?? 0;
            _result.ContainerVolume = solarContainerVolume / 1000;
            _result.SolarCollectorArea = solarPanelArea;
            float ans = 0;
            var iii = 294 / (11 * PrimaryBoiler.WattUsage);
            var iv = 115 / (11 * PrimaryBoiler.WattUsage);

            // Assume only one type of solarpanel pr. package
            if (Math.Abs(solarPanelArea) > 0 && solarContainerVolume > 0)
            {
                ans = (iii * solarPanelArea + iv * (solarContainerVolume / 1000)) *
                    0.9f * (solarCollectorData.Efficency / 100) * _packageData.SolarContainerClass;
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
            var relationship = heatpumpData.WattUsage / (PrimaryBoiler.WattUsage + heatpumpData.WattUsage);
            _ii = UtilityClass.GetWeighting(relationship, container, false);
            _result.SecondaryHeatPumpAFUE = heatpumpData.AFUE;
            return (heatpumpData.AFUE - PrimaryBoiler.AFUE) * _ii;
        }

        // Adjusts the contribution from the HeatPump and the solar system
        private float AdjustedContribution(float heatpumpContribution, float solarContribution)
        {
            float value = -heatpumpContribution > solarContribution ? solarContribution : heatpumpContribution;
            return (value * 0.5f);
        }

        private HeatingUnitDataSheet PrimaryBoiler => _package?.PrimaryHeatingUnitInstance?.Appliance?.DataSheet as HeatingUnitDataSheet;
    }
}
