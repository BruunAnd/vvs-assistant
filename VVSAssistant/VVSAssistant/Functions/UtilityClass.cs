using System;

namespace VVSAssistant.Functions
{
    public static class UtilityClass
    {

        public static float[] resultsPrimHeat = new float[] { 0.0f, 0.1f, 0.2f, 0.3f, 0.4f, 0.5f, 0.6f, 0.7f };
        public static float[] PrimHeatNoContainer = new float[] { 1.0f, 0.7f, 0.45f, 0.25f, 0.15f, 0.05f, 0.02f, 0.0f };
        public static float[] PrimHeatWithContainer = new float[] { 1.0f, 0.63f, 0.30f, 0.15f, 0.06f, 0.02f, 0.0f, 0.0f };
        public static float[] PrimBoilNoContainer = new float[] { 0.30f, 0.35f, 0.55f, 0.75f, 0.85f, 0.95f, 0.98f, 1.0f };
        public static float[] PrimBoilWithContainer = new float[] { 0.0f, 0.37f, 0.70f, 0.85f, 0.94f, 0.98f, 1.0f, 1.0f };


        public static float GetWeighting(float input, bool hasContainer, bool PrimIsHeatPump)
        {
            float[] Array;

            if(hasContainer == true)
            {
                Array = PrimIsHeatPump ? PrimHeatWithContainer : PrimBoilWithContainer;
            }
            else
            {
                Array = PrimIsHeatPump ? PrimHeatNoContainer : PrimBoilNoContainer;
            }

            float output = 0.0f;
            bool _outputFound = false;
            int i = 0;

            while (_outputFound == false)
            {
                if (input > 0.7f)
                {
                    output = Array[resultsPrimHeat.Length - 1];
                    _outputFound = true;
                }                                
                else if (input > resultsPrimHeat[i])
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

        private static float LiniarInterpolation(float[] results, float[] IIvalues, int i, float input)
        {          
            return (float)Math.Round(IIvalues[i - 1] + (IIvalues[i] - IIvalues[i - 1]) / (results[i] - results[i - 1]) * (input - results[i - 1]), 2);
        }
    }
}