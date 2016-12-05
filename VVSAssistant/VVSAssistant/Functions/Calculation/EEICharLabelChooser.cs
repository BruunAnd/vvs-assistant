using System.Collections.Generic;
using VVSAssistant.Models;
using VVSAssistant.Models.DataSheets;

namespace VVSAssistant.Functions.Calculation
{
    internal static class EEICharLabelChooser
    {
        private static readonly List<string> Labels = new List<string> { "G", "F", "E", "D", "C", "B", "A", "A+", "A++", "A+++" };
        private static readonly List<float> PrimPumpOrBoilOrChp = new List<float> { 30, 34, 36, 75, 82, 90, 98, 125, 150};
        public static readonly List<float> LowHeatPump = new List<float> { 55, 59, 61, 100, 107, 115, 123, 150, 175};
        public static readonly List<float> UserTypeM = new List<float> { 27, 30, 33, 36, 39, 65, 100, 130, 163 };
        public static readonly List<float> UserTypeL = new List<float> { 27, 30, 34, 37, 50, 75, 115, 150, 188 };
        public static readonly List<float> UserTypeXl = new List<float> { 27, 30, 35, 38, 55, 80, 123, 160, 200 };
        public static readonly List<float> UserTypeXxl = new List<float> { 28, 32, 36, 40, 60, 85, 131, 170, 213 };

        public static List<string> EEIChar(ApplianceTypes PrimaryHeat, float CalcEII, float uncertainty = 0)
        {
            switch (PrimaryHeat)
            {
                case ApplianceTypes.HeatPump:
                case ApplianceTypes.Boiler:
                case ApplianceTypes.CHP:
                    var heatBoilChPindex = GetLabel(PrimPumpOrBoilOrChp, CalcEII, uncertainty);
                    return new List<string>
                    {
                        Labels[heatBoilChPindex],
                        GetToNext(PrimPumpOrBoilOrChp, CalcEII, heatBoilChPindex),
                        GetNextLabel(heatBoilChPindex)
                    };
                case ApplianceTypes.LowTempHeatPump:
                    var lowTempIndex = GetLabel(LowHeatPump, CalcEII, uncertainty);
                    return new List<string>
                    {
                        Labels[lowTempIndex],
                        GetToNext(PrimPumpOrBoilOrChp, CalcEII, lowTempIndex),
                        GetNextLabel(lowTempIndex)
                    };
                default:
                    return null;
            }
        }

        public static List<string> EEIChar(UseProfileType TypeOfUser, float CalcEEI, float uncertainty = 0)
        {
            switch (TypeOfUser)
            {
                case UseProfileType.M:
                    var mediumIndex = GetLabel(UserTypeM, CalcEEI, uncertainty);
                    return new List<string> { Labels[mediumIndex], GetToNext(UserTypeM, CalcEEI, mediumIndex), GetNextLabel(mediumIndex)};
                case UseProfileType.L:
                    var largeIndex = GetLabel(UserTypeL, CalcEEI, uncertainty);
                    return new List<string> { Labels[largeIndex], GetToNext(UserTypeL, CalcEEI, largeIndex), GetNextLabel(largeIndex) };
                case UseProfileType.XL:
                    var xLargeIndex = GetLabel(UserTypeXl, CalcEEI, uncertainty);
                    return new List<string> { Labels[xLargeIndex], GetToNext(UserTypeXl, CalcEEI, xLargeIndex), GetNextLabel(xLargeIndex) };
                case UseProfileType.XXL:
                    var xXLargeIndex = GetLabel(UserTypeXxl, CalcEEI, uncertainty);
                    return new List<string> { Labels[xXLargeIndex], GetToNext(UserTypeXxl, CalcEEI, xXLargeIndex), GetNextLabel(xXLargeIndex) };
                default:
                    return null;
            }

        }

        private static int GetLabel(IReadOnlyList<float> correctType, float EEI, float uncertainty = 0)
        {
            if (EEI > correctType[8])
                return 9;

            var index = 0;
            while (EEI >= correctType[index] + uncertainty)
            {
                index++;
            }
            return index;
        }

        private static string GetToNext(IReadOnlyList<float> list, float calcEEI, int index)
        {
            if (calcEEI > list[8])
                return "0%";

            return (list[index]- calcEEI) + "%";
        }

        private static string GetNextLabel(int index)
        {
            return index < Labels.Count -1 ? Labels[index + 1] : null;
        }
    }
}
