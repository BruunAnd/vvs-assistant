using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VVSAssistant.Models;

namespace VVSAssistant.Functions.Calculation.Strategies
{

    // !!!!!!!! Alle strategies skal gøre internal igen !!!!!!!!!!
    public class BoilerForWater : IEEICalculation
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

            throw new NotImplementedException();
        }
    }
}
