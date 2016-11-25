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
        EEICalculationResult Results;
        private HeatingUnitDataSheet PrimaryUnit;
        private HeatingUnitDataSheet SecondaryBoiler;
        private float II;
        private float III;
        private float IV;
        private float SolarContributionFactor;



        public EEICalculationResult CalculateEEI(PackagedSolution PackagedSolution)
        {
            Results = new EEICalculationResult();

            //setting starting AFUE value
            PrimaryUnit = PackagedSolution.PrimaryHeatingUnit.DataSheet as HeatingUnitDataSheet;
            Results.PrimaryHeatingUnitAFUE = PrimaryUnit.AFUE;

            //finding effect of temperatur regulator
            IEnumerable<Appliance> TempControllers = PackagedSolution.Appliances.Where(TempControl => TempControl.Type == ApplianceTypes.TemperatureController);

           
           
            if (PrimaryUnit.InternalTempControl != null)
            {
                Results.EffectOfTemperatureRegulatorClass = TemperatureControllerDataSheet.ClassBonus[PrimaryUnit.InternalTempControl];
            }
            else if (TempControllers.Count() > 0)
            {
                Results.EffectOfTemperatureRegulatorClass = TemperatureControllerDataSheet.ClassBonus[(TempControllers.FirstOrDefault()?.DataSheet as TemperatureControllerDataSheet).Class];
            }

            //finding a solarCollector
            IEnumerable<Appliance> Solars = PackagedSolution.Appliances.Where(Solar => Solar.Type == ApplianceTypes.SolarPanel);
            

            //Calculating effect of secondary boiler
            IEnumerable<Appliance> SecBoilers = PackagedSolution.Appliances.Where(SecBoiler => SecBoiler.Type == ApplianceTypes.Boiler);

            if (SecBoilers.Count() > 0)
            {
                SecondaryBoiler = SecBoilers.FirstOrDefault()?.DataSheet as HeatingUnitDataSheet;
                Results.SecondaryBoilerAFUE = SecondaryBoiler.AFUE;

                float heatingUnitRelationship = PrimaryUnit.WattUsage / (PrimaryUnit.WattUsage + SecondaryBoiler.WattUsage);

                II = UtilityClass.GetWeighting(heatingUnitRelationship, HasNonSolarContainer(PackagedSolution, Solars), true);


                Results.EffectOfSecondaryBoiler = (float)Math.Round(Math.Abs(((SecondaryBoiler.AFUE - PrimaryUnit.AFUE) * II) * 100)) / 100;
            }


            //Calculating effect of solarcollector
            if (Solars.Count() != 0)
            {
                if (PackagedSolution.PrimaryHeatingUnit.Type == ApplianceTypes.CHP)
                    SolarContributionFactor = 0.7f;
                else
                    SolarContributionFactor = 0.45f;

                III = 294 / (11 * PrimaryUnit.WattUsage);
                IV = 115 / (11 * PrimaryUnit.WattUsage);

                float AreaOfSolars = 0;
                foreach (Appliance solar in Solars)
                {
                    AreaOfSolars = (solar.DataSheet as SolarCollectorDataSheet).Area + AreaOfSolars;
                }
                Results.SolarCollectorArea = AreaOfSolars;
                Results.ContainerVolume = ((PackagedSolution.SolarContainer.DataSheet as ContainerDataSheet).Volume) / 1000;
                Results.SolarCollectorEffectiveness = (Solars.FirstOrDefault()?.DataSheet as SolarCollectorDataSheet).Efficency;
                Results.ContainerClassification = ContainerDataSheet.ClassificationClass[(PackagedSolution.SolarContainer.DataSheet as ContainerDataSheet).Classification];

                Results.SolarHeatContribution = (float)Math.Round((III * Results.SolarCollectorArea + IV * Results.ContainerVolume) * SolarContributionFactor * (Results.SolarCollectorEffectiveness / 100) * Results.ContainerClassification, 2);
            }
            Results.EEI = (float)Math.Round(Results.PrimaryHeatingUnitAFUE + Results.EffectOfTemperatureRegulatorClass - Results.EffectOfSecondaryBoiler + Results.SolarHeatContribution);
            Results.EEICharacters = EEICharLabelChooser.EEIChar(PackagedSolution.PrimaryHeatingUnit.Type, Results.EEI, 1);

            //Calculating for colder and warmer climates
            if (PackagedSolution.PrimaryHeatingUnit.Type != ApplianceTypes.CHP)
            {
                Results.PackagedSolutionAtColdTemperaturesAFUE = (PackagedSolution.PrimaryHeatingUnit.DataSheet as HeatingUnitDataSheet).AFUEColdClima;
                Results.PackagedSolutionAtWarmTemperaturesAFUE = (PackagedSolution.PrimaryHeatingUnit.DataSheet as HeatingUnitDataSheet).AFUEWarmClima;
            }

            return Results;
        }

        private bool HasNonSolarContainer(PackagedSolution _package, IEnumerable<Appliance> Solars)
        {
            IEnumerable<Appliance> Containers = _package.Appliances.Where(Container => Container.Type == ApplianceTypes.Container);

            if (Solars.Count() <= 0 && Containers.Count() > 0)
            {
                return true;
            }
            else if (_package.Appliances.Where(Container => Container.Type == ApplianceTypes.Container && _package.SolarContainer != Container).Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
            //return _package?.Appliances.Any(item => item?.Type == ApplianceTypes.Container
            //                                && item != _package.SolarContainer) ?? false;
        }
    }
}
