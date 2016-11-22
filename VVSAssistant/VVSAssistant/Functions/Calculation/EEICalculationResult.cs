using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VVSAssistant.Models.DataSheets;

namespace VVSAssistant.Functions.Calculation
{
    // Calculation dat goes here
    public class EEICalculationResult
    {
        public IEEICalculation CalculationType { get; set; }
        public float EEI { get; set; }
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
        public float PackagedSolutionAtColdTemperaturesAFUE { get; set; }
        public float PackagedSolutionAtWarmTemperaturesAFUE { get; set; }
        public float LowHeatAFUE { get; set; }
        public UseProfileType WaterHeatingUseProfile { get; set; }
        public float WaterHeatingEffciency { get; set; }
        public float SolarHeatContribution { get; set; }
        public float AdjustedContribution { get; set; }
    }
}
