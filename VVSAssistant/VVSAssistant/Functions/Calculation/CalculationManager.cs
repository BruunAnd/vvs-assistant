using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            List<IEEICalculation> Calculations = new List<IEEICalculation>();
            
            
            var primaryType = package.PrimaryHeatingUnit.Type;

            switch (package.PrimaryHeatingUnit.Type)
            {
                case ApplianceTypes.CHP:
                case ApplianceTypes.HeatPump:
                     Calculations.Add(new HeatPumpAsPrimary());
                    break;
                case ApplianceTypes.Boiler:
                     Calculations.Add(new BoilerAsPrimary());
                    if (IsBoilerForWater(package))
                        Calculations.Add(new BoilerForWater());
                    break;
                case ApplianceTypes.LowTempHeatPump:
                    Calculations.Add(new HeatPumpAsPrimary());
                    break;
                default:
                    return null;
            }

            return Calculations;
        }
        private bool IsBoilerForWater(PackagedSolution package)
        {

            return ((package.PrimaryHeatingUnit.DataSheet as HeatingUnitDataSheet).WaterHeatingEffiency > 0);

        } 
    }
}
