using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VVSAssistant.Models;
using VVSAssistant.Models.DataSheets;

namespace VVSAssistant.Functions.Calculation
{
    static class EEICharLabelChooser
    {
        static List<string> Labels = new List<string> { "G", "F", "E", "D", "C", "B", "A", "A+", "A++", "A+++" };
        static List<float> PrimPumpOrBoilOrCHP = new List<float> { 30, 34, 36, 75, 82, 90, 98, 125, 150};
        static List<float> LowHeatPump = new List<float> { 55, 59, 61, 100, 107, 115, 123, 150, 175};
        static List<float> UserTypeM = new List<float> { 27, 30, 33, 36, 39, 65, 100, 130, 163 };
        static List<float> UserTypeL = new List<float> { 27, 30, 34, 37, 50, 75, 115, 150, 188 };
        static List<float> UserTypeXL = new List<float> { 27, 30, 35, 38, 55, 80, 123, 160, 200 };
        static List<float> UserTypeXXL = new List<float> { 28, 32, 36, 40, 60, 85, 131, 170, 213 };

        static public List<string> EEIChar(ApplianceTypes PrimaryHeat, float CalcEII, float uncertainty = 0)
        {
            switch (PrimaryHeat)
            {
                case ApplianceTypes.HeatPump:
                case ApplianceTypes.Boiler:
                case ApplianceTypes.CHP:
                    return new List<string> { Labels[getLabel(PrimPumpOrBoilOrCHP, CalcEII, uncertainty)],  getToNext(PrimPumpOrBoilOrCHP, CalcEII, getLabel(PrimPumpOrBoilOrCHP, CalcEII, uncertainty)) };
                case ApplianceTypes.LowTempHeatPump:
                    return new List<string> { Labels[getLabel(LowHeatPump, CalcEII, uncertainty)], getToNext(PrimPumpOrBoilOrCHP, CalcEII, getLabel(LowHeatPump, CalcEII, uncertainty)) };
                default:
                    return null;
            }

        }

        static public List<string> EEIChar(UseProfileType TypeOfUser, float CalcEEI, float uncertainty = 0)
        {
            switch (TypeOfUser)
            {
                case UseProfileType.M:
                    return new List<string> { Labels[getLabel(UserTypeM, CalcEEI, uncertainty)], getToNext(UserTypeM, CalcEEI, getLabel(UserTypeM, CalcEEI, uncertainty))};
                case UseProfileType.L:
                    return new List<string> { Labels[getLabel(UserTypeL, CalcEEI, uncertainty)], getToNext(UserTypeL, CalcEEI, getLabel(UserTypeL, CalcEEI, uncertainty)) };
                case UseProfileType.XL:
                    return new List<string> { Labels[getLabel(UserTypeXL, CalcEEI, uncertainty)], getToNext(UserTypeXL, CalcEEI, getLabel(UserTypeXL, CalcEEI, uncertainty)) };
                case UseProfileType.XXL:
                    return new List<string> { Labels[getLabel(UserTypeXXL, CalcEEI, uncertainty)], getToNext(UserTypeXXL, CalcEEI, getLabel(UserTypeXXL, CalcEEI, uncertainty)) };
                default:
                    return null;
            }

        }

        static private int getLabel(List<float> correctType, float EEI, float uncertainty = 0)
        {
            if (EEI > correctType[8])
                return 9;

            int index = 0;
            while (EEI >= correctType[index] + uncertainty)
            {
                index++;
            }
            return index;
        }

        static private string getToNext(List<float> list, float calcEEI, int index)
        {
            if (calcEEI > list[8])
                return "0%";
            else
                return (list[index + 1] - calcEEI).ToString() + "%";
        }
    }
}
