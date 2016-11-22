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
        public EEICalculationResult CalculateEEI(PackagedSolution Package)
        {
            var primarydata = Package.PrimaryHeatingUnit.DataSheet as HeatingUnitDataSheet;
            var tempControlData = Package.Appliances.SingleOrDefault(item => 
                           item.Type == ApplianceTypes.TemperatureController)?.
                           DataSheet as TemperatureControllerDataSheet;
            var supplementaryHeatpump = Package.Appliances.SingleOrDefault(item =>
                            item.Type == ApplianceTypes.Heatpump)?.DataSheet as HeatingUnitDataSheet;

            var solarPanels = Package.Appliances.Where(item =>
                            item.Type == ApplianceTypes.SolarPanel);
            var containers = Package.Appliances.Where(item =>
                            item.Type == ApplianceTypes.Container);

            float solarContribution = 0;
            float heatpumpContribution = 0;
            // Solarcontribution is calculated here
            while(false)
            {
                solarContribution = 0;
            }
            if(heatpumpContribution > 0 && solarContribution > 0)
            {

            }
            var EEI = 0;
            var result = new EEICalculationResult();
            result.EEI = EEI;
            result.PackagedSolutionAtColdTemperaturesAFUE = EEI * 0;
            return result;
        }
    }
}
