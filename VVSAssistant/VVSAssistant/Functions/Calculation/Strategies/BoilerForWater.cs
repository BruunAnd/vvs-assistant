using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VVSAssistant.Models;
using VVSAssistant.Models.DataSheets;

namespace VVSAssistant.Functions.Calculation.Strategies
{

    // !!!!!!!! Alle strategies skal gøre internal igen !!!!!!!!!!
    class BoilerForWater : IEEICalculation
    {
        /// <summary>
        /// Calculation method for Water heating with a primary Boiler
        /// </summary>
        /// <param name="Package"></param>
        /// <returns>EEICalculationResult which contains the variables used for the calculation,
        /// the energy effiency index and the calculation method used </returns>
        public EEICalculationResult CalculateEEI(PackagedSolution Package)
        {
            var result = new EEICalculationResult();
            var data = Package.PrimaryHeatingUnit.DataSheet as WaterHeatingUnitDataSheet;
            result.WaterHeatingUseProfile = data.UseProfile;
            result.WaterHeatingEffciency = data.WaterHeatingEffiency;
            result.SolarHeatContribution = (1.1f * result.WaterHeatingEffciency - 10.0f) * 666 - 333 - result.WaterHeatingEffciency;
            
            throw new NotImplementedException();
        }
    }
}
