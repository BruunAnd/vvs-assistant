using VVSAssistant.Models;

namespace VVSAssistant.Functions.Calculation
{
    public interface IEEICalculation
    {
        EEICalculationResult CalculateEEI(PackagedSolution packagedSolutionForCalculation);
    }
}
