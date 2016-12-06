using System;
using System.Collections.Generic;

namespace VVSAssistant.Functions
{
    public static class UtilityClass
    {

        public static float[] ResultsPrimHeat = new float[] { 0.0f, 0.1f, 0.2f, 0.3f, 0.4f, 0.5f, 0.6f, 0.7f };
        public static float[] PrimHeatNoContainer = new float[] { 1.0f, 0.7f, 0.45f, 0.25f, 0.15f, 0.05f, 0.02f, 0.0f };
        public static float[] PrimHeatWithContainer = new float[] { 1.0f, 0.63f, 0.30f, 0.15f, 0.06f, 0.02f, 0.0f, 0.0f };
        public static float[] PrimBoilNoContainer = new float[] { 0.30f, 0.35f, 0.55f, 0.75f, 0.85f, 0.95f, 0.98f, 1.0f };
        public static float[] PrimBoilWithContainer = new float[] { 0.0f, 0.37f, 0.70f, 0.85f, 0.94f, 0.98f, 1.0f, 1.0f };


        public static float GetWeighting(float input, bool hasContainer, bool primIsHeatPump)
        {
            float[] array;

            if(hasContainer)
            {
                array = primIsHeatPump ? PrimHeatWithContainer : PrimBoilWithContainer;
            }
            else
            {
                array = primIsHeatPump ? PrimHeatNoContainer : PrimBoilNoContainer;
            }

            float output;
            var i = 0;

            while (true)
            {
                if (input > 0.7f)
                {
                    output = array[ResultsPrimHeat.Length - 1];
                    break;
                }                                
                else if (input > ResultsPrimHeat[i])
                {
                    i++;
                }
                else if(Math.Abs(input - ResultsPrimHeat[i]) < 0)
                {
                    output = array[i];
                    break;
                }
                else
                {
                    output = LiniarInterpolation(ResultsPrimHeat, array, i, input);
                    break;
                }
            }
            return output;
        }

        private static float LiniarInterpolation(IReadOnlyList<float> results, IReadOnlyList<float> ivalues, int i, float input)
        {          
            return (float)Math.Round(ivalues[i - 1] + (ivalues[i] - ivalues[i - 1]) / (results[i] - results[i - 1]) * (input - results[i - 1]), 2);
        }
    }
}