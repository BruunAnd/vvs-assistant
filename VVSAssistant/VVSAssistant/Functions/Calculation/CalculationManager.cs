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

            bool OnlyContainsSolorPanels = package.Appliances.SingleOrDefault(solarPanel =>
                        solarPanel.Type == ApplianceTypes.SolarPanel) != null &&
                        package.Appliances.FirstOrDefault(item => item.Type != ApplianceTypes.SolarPanel && 
                                                          item != package.PrimaryHeatingUnit) == null;

            switch (package.PrimaryHeatingUnit.Type)
            {
                case ApplianceTypes.Heatpump:
                    return new HeatPumpAsPrimary();
                case ApplianceTypes.Boiler:
                    if (primaryType == ApplianceTypes.Boiler && OnlyContainsSolorPanels)
                        return new BoilerForWater();
                    else
                        return new BoilerAsPrimary();
                case ApplianceTypes.LowTempHeatPump:
                    return new LowTempHeatPumpAsPrimary();
                case default(ApplianceTypes):
                    if (package.Appliances.Any(item => item.Type == ApplianceTypes.Boiler))
                        return new CHPStrategy();
                    else
                        return null;
                default:
                    return null;
            }
        } 
    }
}
