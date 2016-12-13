using System.Collections.Generic;
using VVSAssistant.Functions.Calculation.Strategies;
using VVSAssistant.Models;
using VVSAssistant.Models.DataSheets;

namespace VVSAssistant.Functions.Calculation
{
    public class CalculationManager
    {
        /// <summary>
        /// Returns a Calculation strategy based on the primary type and 
        /// packaged solution content.
        /// </summary>
        /// <param name="package"></param>
        /// <returns>Calculation strategy</returns>
        public List<IEEICalculation> SelectCalculationStrategy(PackagedSolution package)
        {
            var calculations = new List<IEEICalculation>();
                   
            switch (package.PrimaryHeatingUnitInstance?.Appliance?.Type ?? 0)
            {
                case ApplianceTypes.LowTempHeatPump:
                case ApplianceTypes.CHP:
                case ApplianceTypes.HeatPump:
                     calculations.Add(new HeatPumpAsPrimary());
                    break;
                case ApplianceTypes.Boiler:
                     calculations.Add(new BoilerAsPrimary());
                    if (IsBoilerForWater(package))
                        calculations.Add(new BoilerForWater());
                    break;
                case ApplianceTypes.TemperatureController:
                    break;
                case ApplianceTypes.SolarPanel:
                    break;
                case ApplianceTypes.Container:
                    break;
                case ApplianceTypes.SolarStation:
                    break;
                case ApplianceTypes.WaterHeater:
                    break;
                default:
                    return null;
            }
            return calculations;
        }
        private static bool IsBoilerForWater(PackagedSolution package)
        {
            return ((HeatingUnitDataSheet) package.PrimaryHeatingUnitInstance.Appliance.DataSheet).WaterHeatingEffiency > 0;
        } 
    }
}
