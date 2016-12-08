using System;
using VVSAssistant.Models;

namespace VVSAssistant.Functions.Calculation.Strategies
{
    internal class HeatPumpAsPrimary : IEEICalculation
    {
        private readonly EEICalculationResult _results = new EEICalculationResult();
        private PackagedSolution _package;
        private PackageDataManager _packageData;
        private float _II;
        private float _III;
        private float _IV;
        private float _solarContributionFactor;



        public EEICalculationResult CalculateEEI(PackagedSolution package)
        {
            _package = package;
            _packageData = new PackageDataManager(_package);
            //setting starting AFUE value
            _results.PrimaryHeatingUnitAFUE = _packageData.PrimaryUnit.AFUE;

            //finding effect of temperatur regulator
            _results.EffectOfTemperatureRegulatorClass = _packageData.TempControllerBonus;
            //Calculating effect of secondary boiler
            _results.SecondaryBoilerAFUE = _packageData.SupplementaryBoiler?.AFUE ?? 0;
            _results.EffectOfSecondaryBoiler = SecBoilerEffect();

            //Calculating effect of solarcollector
            _results.SolarHeatContribution = SolarContribution();

            //finalizing the EEI Calculation
            _results.EEI = (float)Math.Round(_results.PrimaryHeatingUnitAFUE + _results.EffectOfTemperatureRegulatorClass - _results.EffectOfSecondaryBoiler + _results.SolarHeatContribution);
            _results.EEICharacters = EEICharLabelChooser.EEIChar(PrimaryUnitType, _results.EEI, 1)[0];
            _results.ToNextLabel = EEICharLabelChooser.EEIChar(PrimaryUnitType, _results.EEI, 1)[1];
            _results.ProceedingEEICharacter = EEICharLabelChooser.EEIChar(PrimaryUnitType, _results.EEI, 1)[2];
            //Calculating for colder and warmer climates
            if (PrimaryUnitType != ApplianceTypes.CHP)
            {
                _results.PackagedSolutionAtColdTemperaturesAFUE = _results.EEI-(_packageData.PrimaryUnit.AFUE - _packageData.PrimaryUnit.AFUEColdClima);
                _results.PackagedSolutionAtWarmTemperaturesAFUE = _results.EEI + (_packageData.PrimaryUnit.AFUEWarmClima - _packageData.PrimaryUnit.AFUE);
            }
            _results.CalculationType = new PackageDataManager(_package).CalculationStrategyType(_package,_results);
            return _results;
        }


        private float SecBoilerEffect()
        {
            if (_packageData.SupplementaryBoiler == null)
            {
                return 0;
            }
            else
            {
                var heatingUnitRelationship = _packageData.PrimaryUnit.WattUsage / (_packageData.PrimaryUnit.WattUsage + _packageData.SupplementaryBoiler.WattUsage);

                _II = UtilityClass.GetWeighting(heatingUnitRelationship, _packageData.HasNonSolarContainer(), true);


                return Math.Abs((float)Math.Round((_packageData.SupplementaryBoiler.AFUE - _packageData.PrimaryUnit.AFUE) * _II, 2));
            }

        }

        private float SolarContribution()
        {
            _solarContributionFactor = PrimaryUnitType == ApplianceTypes.CHP ? 0.7f : 0.45f;

                var solarCollectorData = _packageData.SolarPanelData;
            var solarPanelArea = _packageData.SolarPanelArea(panel =>
                                    panel.IsRoomHeater);
            var solarContainerVolume = _packageData.SolarContainerVolume(container =>
                                         !container.IsWaterContainer);

            _results.ContainerVolume = solarContainerVolume / 1000;
            _results.SolarCollectorArea = solarPanelArea;
            _results.SolarCollectorEffectiveness = _packageData.SolarPanelData.Efficency;
            _results.ContainerClassification = _packageData.SolarContainerClass;

            float ans = 0;
            _III = 294 / (11 * _packageData.PrimaryUnit.WattUsage);
            _IV = 115 / (11 * _packageData.PrimaryUnit.WattUsage);

            // Assume only one type of solarpanel pr. package
            if (Math.Abs(solarPanelArea) > 0 && solarContainerVolume > 0)
            {
                ans = (_III * solarPanelArea + _IV * (solarContainerVolume / 1000)) *
                    _solarContributionFactor * (solarCollectorData.Efficency / 100) * _packageData.SolarContainerClass;
            }
            return ans;
        }
        private ApplianceTypes PrimaryUnitType => _package.PrimaryHeatingUnit.Type;
    }
}
