using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VVSAssistant.Functions.Calculation
{
    public class WeightingBetweenPrimaryAndSecondaryChooser
    {
        //below did not work

        //public static Dictionary<float, float> PrimHeatPumpNoContainer = new Dictionary<float, float>()
        //{
        //    {0.0f, 1.0f }, {0.1f, 0.70f }, {0.2f, 0.45f }, {0.3f, 0.25f }, {0.4f, 0.15f }, {0.5f, 0.05f }, {0.6f, 0.02f }, {0.7f, 0.0f }
        //};
        //public static Dictionary<float, float> PrimHeatPumpWithContainer = new Dictionary<float, float>()
        //{
        //    {0.0f, 1.0f }, {0.1f, 0.63f }, {0.2f, 0.30f }, {0.3f, 0.15f }, {0.4f, 0.06f }, {0.5f, 0.02f }, {0.6f, 0.0f }, {0.7f, 0.0f }
        //};

        ////for later use with primary boiler.
        //public static Dictionary<float, float> PrimBoilerNoContainer = new Dictionary<float, float>();
        //public static Dictionary<float, float> PrimBoilerWithContainer = new Dictionary<float, float>();

        public static float[] resultsPrimHeat = new float[] { 0.0f, 0.1f, 0.2f, 0.3f, 0.4f, 0.5f, 0.6f, 0.7f };
        public static float[] PrimHeatNoContainer = new float[] { 1.0f, 0.7f, 0.45f, 0.25f, 0.15f, 0.05f, 0.02f, 0.0f };
        public static float[] PrimHeatWithContainer = new float[] { 1.0f, 0.63f, 0.30f, 0.15f, 0.06f, 0.02f, 0.00f, 0.0f };


        public float GetWeightingPrimHeat(float input, bool hasContainer)
        {
            float[] Array;

            if(hasContainer == true)
            {
                Array = PrimHeatWithContainer;
            }
            else
            {
                Array = PrimHeatNoContainer;
            }

            float output = 0.0f;
            bool _outputFound = false;
            int i = 0;

            while (_outputFound == false)
            {
                if (input < resultsPrimHeat[i])
                {
                    i++;
                }
                else if(input == resultsPrimHeat[i])
                {
                    output = Array[i];
                    _outputFound = true;
                }
                else
                {
                    output = LiniarInterpolation(resultsPrimHeat, Array, i, input);
                    _outputFound = true;
                }
            }
            return output;
        }

        private float LiniarInterpolation(float[] results, float[] IIvalues, int i, float input)
        {
            return IIvalues[i - 1] + (IIvalues[i] - IIvalues[i - 1]) / (results[i] - results[i - 1]) * (input - results[i - 1]);
        }

    }
}