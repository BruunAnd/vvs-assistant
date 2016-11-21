using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VVSAssistant.Functions.Calculation
{
    // Calculation dat goes here
    public class EEICalculationResult
    {
        public IEEICalculation CalculationType { get; set; }
        public float PrimaryHeatingUnitAFUE { get; set; }
        public float EffectOfTemperatureRegulatorClass { get; set; }
        public float SecondaryBoilerAFUE { get; set; }
        public float EffectOfSecondaryBoiler { get; set; }
        public float SecondaryHeatPumpAFUE { get; set; }
        public float EffectOfSecondaryHeatPump { get; set; }
        public float SolarCollectorArea { get; set; }
        public float ContainerVolume { get; set; }
        public float SolarCollectorEffectiveness { get; set; }
        public float ContainerClassification { get; set; }
        public float PackagedSolutionAtNormalTemperaturesAFUE { get; set; }
        public float PackagedSolutionAtColdTemperaturesAFUE { get; set; }
        public float PackagedSolutionAtWarmTemperaturesAFUE { get; set; }
        public float LowHeatAFUE { get; set; }
        public float WaterHeatingUseProfile { get; set; }
    }
}
