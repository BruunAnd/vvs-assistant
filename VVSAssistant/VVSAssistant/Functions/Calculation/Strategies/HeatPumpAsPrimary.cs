using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VVSAssistant.Models;
using VVSAssistant.Models.DataSheets;

namespace VVSAssistant.Functions.Calculation.Strategies
{
    public class HeatPumpAsPrimary : IEEICalculation
    {
        EEICalculationResult Results;
        private float _startAFUE;
        private float _effectOfTemperatureRegulator;
        private float _AFUEOfSecBoiler;

        public EEICalculationResult CalculateEEI(PackagedSolution PackagedSolution)
        {
            Results = new EEICalculationResult();

            _startAFUE = (PackagedSolution.PrimaryHeatingUnit.DataSheet as HeatingUnitDataSheet).AFUE;
            Results.PrimaryHeatingUnitAFUE = _startAFUE;

            IEnumerable<Appliance> TempControllers = PackagedSolution.Appliances.Where(TempControl => TempControl.Type == ApplianceTypes.TemperatureController);
            _effectOfTemperatureRegulator = TemperatureControllerDataSheet.ClassBonus[(TempControllers.FirstOrDefault().DataSheet as TemperatureControllerDataSheet).Class];
            Results.EffectOfTemperatureRegulatorClass = _effectOfTemperatureRegulator;

            IEnumerable<Appliance> SecBoilers = PackagedSolution.Appliances.Where(SecBoiler => SecBoiler.Type == ApplianceTypes.Boiler);
            _AFUEOfSecBoiler = (SecBoilers.FirstOrDefault().DataSheet as HeatingUnitDataSheet).AFUE;
            Results.SecondaryBoilerAFUE = _AFUEOfSecBoiler;







            return Results;
        }
    }
}
