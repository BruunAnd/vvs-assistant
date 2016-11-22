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
        WeightingBetweenPrimaryAndSecondaryChooser WeightChooser = new WeightingBetweenPrimaryAndSecondaryChooser();
        EEICalculationResult Results;
        private HeatingUnitDataSheet PrimaryUnit;
        private HeatingUnitDataSheet SecondaryBoiler;
        private float _effectOfTemperatureRegulator;
        private float II;
        private float _effectOfSecBoiler;

        public EEICalculationResult CalculateEEI(PackagedSolution PackagedSolution)
        {
            Results = new EEICalculationResult();

            //setting starting AFUE value
            PrimaryUnit = PackagedSolution.PrimaryHeatingUnit.DataSheet as HeatingUnitDataSheet;
            Results.PrimaryHeatingUnitAFUE = PrimaryUnit.AFUE;


            //finding effect of temperatur regulator
            IEnumerable<Appliance> TempControllers = PackagedSolution.Appliances.Where(TempControl => TempControl.Type == ApplianceTypes.TemperatureController);
            _effectOfTemperatureRegulator = TemperatureControllerDataSheet.ClassBonus[(TempControllers.FirstOrDefault().DataSheet as TemperatureControllerDataSheet).Class];
            Results.EffectOfTemperatureRegulatorClass = _effectOfTemperatureRegulator;

            //finding a solarCollector
            IEnumerable<Appliance> Solars = PackagedSolution.Appliances.Where(Solar => Solar.Type == ApplianceTypes.SolarPanel);


            //Calculating effect of secondary boiler
            IEnumerable<Appliance> SecBoilers = PackagedSolution.Appliances.Where(SecBoiler => SecBoiler.Type == ApplianceTypes.Boiler);
            SecondaryBoiler = SecBoilers.FirstOrDefault()?.DataSheet as HeatingUnitDataSheet;
            Results.SecondaryBoilerAFUE = SecondaryBoiler.AFUE;

            float heatingUnitRelationship = PrimaryUnit.WattUsage / (PrimaryUnit.WattUsage + SecondaryBoiler.WattUsage);

            IEnumerable<Appliance> Containers = PackagedSolution.Appliances.Where(Container => Container.Type == ApplianceTypes.Container);
            if (Containers.Count() > Solars.Count())
            {
                II = WeightChooser.GetWeightingPrimHeat(heatingUnitRelationship, true);
            }
            else
            {
                II = WeightChooser.GetWeightingPrimHeat(heatingUnitRelationship, false);
            }

            _effectOfSecBoiler = (SecondaryBoiler.AFUE - PrimaryUnit.AFUE) * II;
            Results.EffectOfSecondaryBoiler = _effectOfSecBoiler;









            return Results;
        }
    }
}
