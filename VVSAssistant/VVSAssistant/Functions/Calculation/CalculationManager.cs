using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VVSAssistant.Functions.Calculation.Interfaces;
using VVSAssistant.Functions.Calculation.Strategies;
using VVSAssistant.Models;

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
        public IEEICalculation SelectCalculationStreategy(PackagedSolution package)
        {
            var primaryType = package.PrimaryHeatingUnit.Type;

            switch (package.PrimaryHeatingUnit.Type)
            {
                case ApplianceTypes.HeatPump:
                    return new HeatPumpAsPrimary();
                case ApplianceTypes.Boiler:
                    if (IsBoilerForWater(package))
                        return new BoilerForWater();
                    else
                        return new BoilerAsPrimary();
                case ApplianceTypes.LowTempHeatPump:
                    return new HeatPumpAsPrimary();
                case default(ApplianceTypes):
                    if (package.Appliances.Any(item => item.Type == ApplianceTypes.Boiler))
                        return new HeatPumpAsPrimary();
                    else
                        return null;
                default:
                    return null;
            }
        }
        private bool IsBoilerForWater(PackagedSolution package)
        {
            var primaryType = package.PrimaryHeatingUnit.Type;

            bool ContainsSolarPanel = package.Appliances.SingleOrDefault(solarPanel =>
                                       solarPanel.Type == ApplianceTypes.SolarPanel) != null;

            bool DoesNotContainsOtherTypes = package.Appliances.FirstOrDefault(item =>
                                       item.Type != ApplianceTypes.SolarPanel &&
                                       item != package.PrimaryHeatingUnit && 
                                       item.Type != ApplianceTypes.Container) == null;

            bool OnlyContainsSolarPanels = ContainsSolarPanel && DoesNotContainsOtherTypes;

            return primaryType == ApplianceTypes.Boiler && OnlyContainsSolarPanels;
        } 
    }
}
