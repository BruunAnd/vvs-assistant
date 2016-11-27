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
        private PackagedSolution _package;
        /// <summary>
        /// EEI Calculation for packaged solution with a boiler as primary heating unit
        /// </summary>
        /// <param name="Package"></param>
        /// <returns>EEICalculationResult which are the EEI and all the varibales in between</returns>
        public EEICalculationResult CalculateEEI(PackagedSolution Package)
        {
            _package = Package;
            if (PrimaryBoiler == null)
                return null;
            _result.PrimaryHeatingUnitAFUE = PrimaryBoiler.AFUE;

            _result.EffectOfTemperatureRegulatorClass = PrimaryBoiler.InternalTempControl == null ? TempControlBonus :
                                         TemperatureControllerDataSheet.ClassBonus[PrimaryBoiler.InternalTempControl];
            _result.EffectOfSecondaryBoiler = (SecondBoilerData?.AFUE - _result.PrimaryHeatingUnitAFUE) * 0.1f ?? 0;
            III = 294 / (11*PrimaryBoiler.WattUsage);
            IV = 115 / (11 * PrimaryBoiler.WattUsage);
            // Assume only one type of solarpanel pr. package
            _result.SolarHeatContribution = SolarPanels != null && SolarContainerData != null ? 
                                            (III * SolarPanelArea + IV * (SolarContainerData.Volume/1000)) * 
                                             0.9f * (SolarCollectorData.Efficency/100)*SolarContainerClass 
                                             : default(float);
            _result.EffectOfSecondaryHeatPump = -HeatpumpContribution(HasNonSolarContainer());
            _result.AdjustedContribution = _result.EffectOfSecondaryHeatPump != 0 && _result.SolarHeatContribution != 0
                                           ? AdjustedContribution(_result.EffectOfSecondaryHeatPump, _result.SolarHeatContribution)
                                           : default(float);

            _result.EEI = _result.PrimaryHeatingUnitAFUE + _result.EffectOfTemperatureRegulatorClass
                          - _result.EffectOfSecondaryBoiler + _result.SolarHeatContribution -
                          _result.EffectOfSecondaryHeatPump - _result.AdjustedContribution;
            _result.PackagedSolutionAtColdTemperaturesAFUE = II != default(float) ? _result.EEI + (50 * II) : 0;
            _result.EEICharacters = EEICharLabelChooser.EEIChar(ApplianceTypes.Boiler, _result.EEI, 1);
            return _result;
        }

        /* Calculates the Heatpump contribution using the relationship between the heatpump
         * and primary boiler, and performing linear interpolation on the table values
         * defined in the calculation documents */
         // bool container defines whether the package non solar container
        private float HeatpumpContribution(bool container)
        {
            var heatpumpData = HeatpumpDataSheet;
            if (heatpumpData == null)
                return 0f;
            var boilerData = PrimaryBoiler;
            float relationship;
            if (heatpumpData != null && boilerData != null)
                relationship = heatpumpData.WattUsage / (boilerData.WattUsage + heatpumpData.WattUsage);
            else
                return 0;
            II = UtilityClass.GetWeighting(relationship, container, false);
            _result.SecondaryHeatPumpAFUE = heatpumpData.AFUE;
            return (heatpumpData.AFUE - boilerData.AFUE) * II;
        }
        private bool HasNonSolarContainer()
        {
            return _package?.SolarContainers.Count > 0;
        }
        private float AdjustedContribution(float heatpumpContribution, float solarContribution)
        {
            float value = -heatpumpContribution > solarContribution ? solarContribution : heatpumpContribution;
            return (value * 0.5f);
        }
        #region Properties Data Getters
        internal float SolarPanelArea
        {
            get
            {
                float area = 0;
                var panels =_package.Appliances.Where(item => item.Type == ApplianceTypes.SolarPanel);
                foreach (var item in panels)
                {
                    area += (item?.DataSheet as SolarCollectorDataSheet).Area;
                }
                return area;
            }
        }
        internal float ContainerVolume
        {
            get
            {
                return 0;
            }
        }
        internal HeatingUnitDataSheet PrimaryBoiler { get {return _package?.PrimaryHeatingUnit?.
                                                      DataSheet as HeatingUnitDataSheet; } }
        internal float TempControlBonus { get { return TemperatureControllerDataSheet.ClassBonus
                                        [(_package?.Appliances.FirstOrDefault(item =>
                                        item?.Type == ApplianceTypes.TemperatureController)?
                                        .DataSheet as TemperatureControllerDataSheet)?.Class ?? "0"]; } }
        internal HeatingUnitDataSheet SecondBoilerData { get { return _package.Appliances.FirstOrDefault(item =>
                                            item.Type == ApplianceTypes.Boiler && item !=
                                            _package.PrimaryHeatingUnit)?.DataSheet as HeatingUnitDataSheet; } }
        internal SolarCollectorDataSheet SolarPanels { get { return _package?.Appliances?.FirstOrDefault(item =>
                                                      item.Type == ApplianceTypes.SolarPanel)?.DataSheet as
                                                      SolarCollectorDataSheet; } }
        internal Appliance SupplementaryHeatpump { get { return null; } }
        internal float SolarContainerClass { get { return ContainerDataSheet.ClassificationClass[
                                             (_package?.SolarContainers[0]?.DataSheet
                                             as ContainerDataSheet).Classification ?? "0"]; } }
        internal ContainerDataSheet SolarContainerData
        {
            get
            {
                return _package?.SolarContainers[0]?.DataSheet as ContainerDataSheet;
            }
        }
        internal SolarCollectorDataSheet SolarCollectorData
        {
            get
            {
                return _package?.Appliances.FirstOrDefault(item =>
                       item.Type == ApplianceTypes.SolarPanel).DataSheet
                       as SolarCollectorDataSheet;
            }
        }
        internal HeatingUnitDataSheet HeatpumpDataSheet
        {
            get
            {
                return _package?.Appliances.FirstOrDefault(item =>
                       item.Type == ApplianceTypes.HeatPump)?.DataSheet
                       as HeatingUnitDataSheet;
            }
        }
        #endregion
    }
}
