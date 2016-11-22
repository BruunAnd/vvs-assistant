using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VVSAssistant.Models;
using VVSAssistant.Models.DataSheets;

namespace VVSAssistant.Functions.Calculation.Strategies
{
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
            var Qref = _Qref[result.WaterHeatingUseProfile];
            int parameterTwo = (int)(220 - Qref);
            int parameterThree = (int)((_Qaux * 2.5) / (220 * Qref)); 
            result.SolarHeatContribution = (1.1f * result.WaterHeatingEffciency - 10.0f) * parameterTwo - parameterThree - result.WaterHeatingEffciency;

            result.EEI = result.SolarHeatContribution + result.WaterHeatingEffciency;

            result.PackagedSolutionAtColdTemperaturesAFUE = result.EEI - 0.2f * result.SolarHeatContribution;
            result.PackagedSolutionAtWarmTemperaturesAFUE = result.EEI + 0.4f * result.SolarHeatContribution;

            throw new NotImplementedException();
        }
        private int _Qaux;
        private int _Qnonsol;

        private Dictionary<UseProfileType, float> _Qref = new Dictionary<UseProfileType, float>()
        {
            {UseProfileType.M, 5.845f}, {UseProfileType.L, 11.655f}, {UseProfileType.XL, 19.070f}, {UseProfileType.XXL, 24.530f}
        };
        
        // Calculates the Qaux (auxiliary electricity consumption) and Qnonsol (annual non-solar contribution)
        private void SolCalMethod(WaterHeatingUnitDataSheet data)
        {

        }
    }
}
