using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VVSAssistant.Models;
using VVSAssistant.Models.DataSheets;

namespace VVSAssistant.Functions.Calculation.Strategies
{
    class HeatPumpAsPrimary : IEEICalculation
    {
        EEICalculationResult _results = new EEICalculationResult();
        private PackagedSolution _package;
        private PackageDataManager _packageData;
        private float _II;
        private float _III;
        private float _IV;
        private float _solarContributionFactor;



        public EEICalculationResult CalculateEEI(PackagedSolution Package)
        {
            _package = Package;
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
            _results.EEICharacters = EEICharLabelChooser.EEIChar(_package.PrimaryHeatingUnit.Type, _results.EEI, 1)[0];
            _results.ToNextLabel = EEICharLabelChooser.EEIChar(_package.PrimaryHeatingUnit.Type, _results.EEI, 1)[1];
            _results.ProceedingEEICharacter = EEICharLabelChooser.EEIChar(_package.PrimaryHeatingUnit.Type, _results.EEI, 1)[2];
            //Calculating for colder and warmer climates
            if (_package.PrimaryHeatingUnit.Type != ApplianceTypes.CHP)
            {
                _results.PackagedSolutionAtColdTemperaturesAFUE = _results.EEI-(_packageData.PrimaryUnit.AFUE - _packageData.PrimaryUnit.AFUEColdClima);
                _results.PackagedSolutionAtWarmTemperaturesAFUE = _results.EEI + (_packageData.PrimaryUnit.AFUEWarmClima - _packageData.PrimaryUnit.AFUE);
            }
            _results.CalculationType = new PackageDataManager(_package).CalculationStrategyType(_package,_results);
            return _results;
        }


        private float SecBoilerEffect()
        {
            float heatingUnitRelationship;
            if (_packageData.SupplementaryBoiler == null)
            {
                return 0;
            }
            else
            {
                heatingUnitRelationship = _packageData.PrimaryUnit.WattUsage / (_packageData.PrimaryUnit.WattUsage + _packageData.SupplementaryBoiler.WattUsage);

                _II = UtilityClass.GetWeighting(heatingUnitRelationship, _packageData.HasNonSolarContainer(), true);


                return Math.Abs((float)Math.Round((_packageData.SupplementaryBoiler.AFUE - _packageData.PrimaryUnit.AFUE) * _II, 2));
            }

        }

        private float SolarContribution()
        {
            if (_package.PrimaryHeatingUnit.Type == ApplianceTypes.CHP)
                _solarContributionFactor = 0.7f;
            else
                _solarContributionFactor = 0.45f;

                var SolarCollectorData = _packageData.SolarPanelData;
            float solarPanelArea = _packageData.SolarPanelArea(panel =>
                                    panel.isRoomHeater == true);
            float solarContainerVolume = _packageData.SolarContainerVolume(container =>
                                         !container.isWaterContainer);

            _results.ContainerVolume = solarContainerVolume / 1000;
            _results.SolarCollectorArea = solarPanelArea;
            float ans = 0;
            _III = 294 / (11 * _packageData.PrimaryUnit.WattUsage);
            _IV = 115 / (11 * _packageData.PrimaryUnit.WattUsage);

            // Assume only one type of solarpanel pr. package
            if (solarPanelArea != 0 && solarContainerVolume > 0)
            {
                ans = (_III * solarPanelArea + _IV * (solarContainerVolume / 1000)) *
                    _solarContributionFactor * (SolarCollectorData.Efficency / 100) * _packageData.SolarContainerClass;
            }
            return ans;
        }       
    }
}
