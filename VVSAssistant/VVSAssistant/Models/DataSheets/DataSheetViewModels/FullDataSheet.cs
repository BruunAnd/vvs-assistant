using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VVSAssistant.Models.DataSheets
{
    /* The sole purpose of this class is to supply the CreatePackagedSolution 
     * VM with all possible properties needed when making a new appliance. */
    class FullDataSheet : DataSheet
    {
        /* Container */
        public static Dictionary<string, float> ClassificationClassProperty = new Dictionary<string, float>()
        {
            {"A*", 0.95f }, {"A", 0.91f }, {"B", 0.86f}, {"C", 0.83f}, {"D", 0.81f}, {"E", 0.81f}, {"F", 0.81f}, {"G", 0.81f}
        };
        public float VolumeProperty { get; set; }
        public string ClassificationProperty { get; set; }
        public float StandingLossProperty { get; set; }

        public DataSheetProperty ClassificationClass { get; set; }
        public DataSheetProperty Volume { get; set; }
        public DataSheetProperty Classification { get; set; }
        public DataSheetProperty StandingLoss { get; set; }
        
        /* Heating Unit */
        public float AFUEProperty { get; set; }
        public float WattUsageProperty { get; set; }
        public float AFUEColdClimaProperty { get; set; }
        public float AFUEWarmClimaProperty { get; set; }
        public string InternalTempControlProperty { get; set; }
        public float WaterHeatingEffiencyProperty { get; set; }
        public UseProfileType UseProfileProperty { get; set; }
        public float VnormProperty { get; set; }
        public float VbuProperty { get; set; }

        public DataSheetProperty AFUE { get; set; }
        public DataSheetProperty WattUsage { get; set; }
        public DataSheetProperty AFUEColdClima { get; set; }
        public DataSheetProperty AFUEWarmClima { get; set; }
        public DataSheetProperty InternalTempControl { get; set; }
        public DataSheetProperty WaterHeatingEffiency { get; set; }
        public DataSheetProperty UseProfile { get; set; }
        public DataSheetProperty Vnorm { get; set; }
        public DataSheetProperty Vbu { get; set; }


        /* Solar Collector */
        public float AreaProperty { get; set; }
        public float EfficencyProperty { get; set; }
        public float AsolProperty { get; set; }
        public float N0Property { get; set; }
        public float a1Property { get; set; }
        public float a2Property { get; set; }
        public float IAMProperty { get; set; }

        public DataSheetProperty Area { get; set; }
        public DataSheetProperty Efficency { get; set; }
        public DataSheetProperty Asol { get; set; }
        public DataSheetProperty N0 { get; set; }
        public DataSheetProperty a1 { get; set; }
        public DataSheetProperty a2 { get; set; }
        public DataSheetProperty IAM { get; set; }

        /* Solar Station */
        public float SolPumpConsumptionProperty { get; set; }
        public float SolStandbyConsumptionProperty { get; set; }

        public DataSheetProperty SolPumpConsumption { get; set; }
        public DataSheetProperty SolStandbyConsumption { get; set; }

        /* Temperature Controller */
        public static Dictionary<string, float> ClassBonusProperty = new Dictionary<string, float>()
        {
            {"0", 0.0f }, {"1", 1.0f }, {"2", 2.0f }, {"3", 1.5f }, {"4", 2.0f }, {"5", 3.0f }, {"6", 4.0f }, {"7", 3.5f }, {"8", 5.0f }
        };
        public string ClassProperty { get; set; }

        public DataSheetProperty ClassBonus { get; set; }
        public DataSheetProperty Class { get; set; }

        public FullDataSheet()
        {
            ClassificationClass = new DataSheetProperty(ClassificationClassProperty);
            Classification = new DataSheetProperty(ClassificationProperty);
            Volume = new DataSheetProperty(VolumeProperty);
            StandingLoss = new DataSheetProperty(StandingLossProperty);

            AFUE = new DataSheetProperty(AFUEProperty);
            WattUsage = new DataSheetProperty(WattUsageProperty);
            AFUEColdClima = new DataSheetProperty(AFUEColdClimaProperty);
            AFUEWarmClima = new DataSheetProperty(AFUEWarmClimaProperty);
            InternalTempControl = new DataSheetProperty(InternalTempControlProperty);
            WaterHeatingEffiency = new DataSheetProperty(WaterHeatingEffiencyProperty);
            UseProfile = new DataSheetProperty(UseProfileProperty);
            Vnorm = new DataSheetProperty(VnormProperty);
            Vbu = new DataSheetProperty(VbuProperty);

            Area = new DataSheetProperty(AreaProperty);
            Efficency = new DataSheetProperty(EfficencyProperty);
            Asol = new DataSheetProperty(AsolProperty);
            N0 = new DataSheetProperty(N0Property);
            a1 = new DataSheetProperty(a1Property);
            a2 = new DataSheetProperty(a2Property);
            IAM = new DataSheetProperty(IAMProperty);

            SolPumpConsumption = new DataSheetProperty(SolPumpConsumptionProperty);
            SolStandbyConsumption = new DataSheetProperty(SolStandbyConsumptionProperty);

            ClassBonus = new DataSheetProperty(ClassBonusProperty);
            Class = new DataSheetProperty(ClassProperty);
        }
    }
}
