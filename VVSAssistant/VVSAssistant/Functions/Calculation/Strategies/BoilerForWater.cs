using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VVSAssistant.Models;
using VVSAssistant.Models.DataSheets;

namespace VVSAssistant.Functions.Calculation.Strategies
{
    class BoilerForWater : IEEICalculation
    {
        private PackagedSolution _package;
        /// <summary>
        /// Calculation method for Water heating with a primary Boiler
        /// </summary>
        /// <param name="Package"></param>
        /// <returns>EEICalculationResult which contains the variables used for the calculation,
        /// the energy effiency index and the calculation method used </returns>
        public EEICalculationResult CalculateEEI(PackagedSolution Package)
        {
            _package = Package;
            var result = new EEICalculationResult();
            var data = Package.PrimaryHeatingUnit.DataSheet as WaterHeatingUnitDataSheet;
            if (PrimaryData == null || PrimaryData?.WaterHeatingEffiency == null || 
                PrimaryData?.UseProfile == null)
                return null;

            result.WaterHeatingEffciency = PrimaryData.WaterHeatingEffiency;
            result.WaterHeatingUseProfile = PrimaryData.UseProfile;

            var Qref = _Qref[result.WaterHeatingUseProfile];
            float Qaux = SolCalMethodQaux();
            float Qnonsol = SolCalMethodQnonsol();

            float parameterOne = result.WaterHeatingEffciency;
            float parameterTwo = Qnonsol != 0 ? (float)((220 * Qref) / Qnonsol) : 0;
            
            float parameterThree =  Qaux != 0 ? (float)(((Qaux * 2.5) / (220 * Qref))*100) : 0; 
            result.SolarHeatContribution = Qaux == 0 || Qnonsol == 0 ? 0:
                (1.1f * parameterOne - 10) * parameterTwo - parameterThree - parameterOne;

            result.EEI = (float)(result.SolarHeatContribution + parameterOne);

            result.PackagedSolutionAtColdTemperaturesAFUE = result.EEI - 0.2f * result.SolarHeatContribution;
            result.PackagedSolutionAtWarmTemperaturesAFUE = result.EEI + 0.4f * result.SolarHeatContribution;

            result.CalculationType = this;

            return result;
        }

        private Dictionary<UseProfileType, float> _Qref = new Dictionary<UseProfileType, float>()
        {
            {UseProfileType.M, 5.845f}, {UseProfileType.L, 11.655f}, {UseProfileType.XL, 19.070f}, {UseProfileType.XXL, 24.530f}
        };
        // Averge solar iradiance and averge temperatur per month 
        private Dictionary<string, QnonsolData> MonthlyQnonsol = new Dictionary<string, QnonsolData>()
        {
            {"Jan", new QnonsolData(70f, 2.8f) }, {"Feb", new QnonsolData(104f, 2.6f)}, {"Mar", new QnonsolData(149f,7.4f)},
            {"Apr", new QnonsolData(192f, 12.2f)}, {"May", new QnonsolData(221f, 16.3f)}, {"Jun", new QnonsolData(222f, 19.8f)},
            {"Jul", new QnonsolData(232f, 21f)}, {"Aug", new QnonsolData(217f, 22f)}, {"Sep", new QnonsolData(176f, 17f)},
            {"Oct", new QnonsolData(129f, 11.9f)}, {"Nov", new QnonsolData(80f, 5.6f)}, {"Dec", new QnonsolData(56f, 3.2f)}
        };
        // Calculates the Qaux (auxiliary electricity consumption)
        internal float SolCalMethodQaux()
        {
            // 2000 active solar hours 
            float Qaux = SolarStation == null ? 0: (float)Math.Ceiling(((SolarStation.SolPumpConsumption * 2000) +
                   (SolarStation.SolStandbyConsumption * 24 * 365)) / 1000);
            return Qaux;
        }
        internal float SolCalMethodQnonsol()
        {
            float Qnonsol = 0;
            if (SolarStation == null || SolarData == null || PrimaryData == null || SolarContainerData == null)
                return 0;
            // Monthly Qnonsol values, needs to be summed to get the full Qnonsol
            foreach (var keyvalue in MonthlyQnonsol.Keys)
            {
                var item = MonthlyQnonsol[keyvalue];
                // Averge temp surrounding the heat store, 20 if inside and Tout if outside
                float Ta = 20;

                float etaloop = (float)((1 - ((SolarData.N0 * SolarData.a1) / 100)));
                float Ac = (float)(SolarData.a1 + SolarData.a2 * 40);
                float Ul = (float)((6 + 0.3f * SolarData.Asol) / SolarData.Asol);

                float Vsol = SolarContainerData.Volume - PrimaryData.Vbu;
                float ccap = (float)Math.Pow(75 * SolarData.Asol / Vsol, 0.25f);

                item.Lwh = 30.5f * 0.6f * (_Qref[PrimaryData.UseProfile] + 1.09f);
                item.Y = SolarData.Asol * SolarData.IAM * SolarData.N0 * etaloop * item.Qsol * (0.732f / item.Lwh);
                item.Trefw = 11.6f + 1.18f * 40 + 3.86f * 10 - 1.32f * item.Tout;
                item.X = SolarData.Asol * (Ac + Ul) * etaloop * (item.Trefw - item.Tout) *
                         ccap * 0.732f / item.Lwh;

                float LsolW1 = (float)((item.Lwh * (1.029f * item.Y - 0.065f * item.X - 0.245f *
                               (float)Math.Pow(item.Y, 2) + 0.0018f *
                               (float)Math.Pow(item.X, 2) + 0.0215f *
                               (float)Math.Pow(item.Y, 3))));
                // Standing loss in W/k
                float PsbSol = (SolarContainerData.StandingLoss / 45);
                item.Qbuf = (float)(0.732f * PsbSol * ((Vsol) /
                            SolarContainerData.Volume) * (10 + (50 * LsolW1) / item.Lwh - Ta));
                float LsolW = LsolW1 - item.Qbuf;
                item.Qnonsol = (float)((item.Lwh - LsolW + PsbSol *
                               (PrimaryData.Vbu / SolarContainerData.Volume) * (60 - Ta) * 0.732f));

                Qnonsol += item.Qnonsol;
            }

            return Qnonsol;
        }
        // Calculates the Qnonsol (annual non-solar contribution)
        internal float SolCalMethodQnonsol2()
        {
            float Qnonsol = 0;
            float Vsol = PrimaryData.Vnorm > 0 ? PrimaryData.Vnorm - PrimaryData.Vbu: SolarContainerData.Volume;
            // Monthly Qnonsol values, needs to be summed to get the full Qnonsol
            foreach (var keyvalue in MonthlyQnonsol.Keys)
            {
                var item = MonthlyQnonsol[keyvalue];
                // Averge temp surrounding the heat store, 20 if inside and Tout if outside
                float Ta = 20;
                
                float etaloop = (float)((1 - ((SolarData.N0 * SolarData.a1) / 100)));
                float Ac = (float)(SolarData.a1 + SolarData.a2 * 40);
                float Ul = (float)((6 + 0.3f * SolarData.Asol) / SolarData.Asol);
                
                float ccap = (float)Math.Pow(75 * SolarData.Asol / Vsol, 0.25f);

                item.Lwh = 30.5f * 0.6f * (_Qref[PrimaryData.UseProfile] + 1.09f);
                item.Y = SolarData.Asol * SolarData.IAM * SolarData.N0 * etaloop * item.Qsol * (0.732f/item.Lwh);
                item.Trefw = 11.6f + 1.18f * 40 + 3.86f * 10 - 1.32f * item.Tout;
                item.X = SolarData.Asol * (Ac + Ul) * etaloop * (item.Trefw - item.Tout) * 
                         ccap * 0.732f / item.Lwh;

                float LsolW1 = (float)((item.Lwh * (1.029f*item.Y - 0.065f*item.X - 0.245f*
                               (float)Math.Pow(item.Y, 2) + 0.0018f * 
                               (float)Math.Pow(item.X,2)+0.0215f*
                               (float)Math.Pow(item.Y,3))));
                // Standing loss in W/k
                float PsbSol = (PrimaryData.StandingLoss/ 45);
                item.Qbuf = (float)(0.732f * PsbSol * ((Vsol) / 
                            PrimaryData.Vnorm) * (10 + (50 * LsolW1) / item.Lwh - Ta));
                float LsolW = LsolW1 - item.Qbuf;
                item.Qnonsol = (float)((item.Lwh - LsolW + PsbSol * 
                               (PrimaryData.Vbu / PrimaryData.Vnorm) * (60 - Ta) * 0.732f));         

                Qnonsol += item.Qnonsol;
            }

            return Qnonsol;
        }

        // Solar panel plants and conventional water heaters
        internal float SolicsMethode(WaterHeatingUnitDataSheet data)
        {
            return 0.0f;
        }

        #region Data Getters
        public SolarCollectorDataSheet SolarData { get
            { return _package.Appliances.FirstOrDefault(item =>
                    item.Type == ApplianceTypes.SolarPanel)?.DataSheet
                    as SolarCollectorDataSheet; } }
        public SolarStationDataSheet SolarStation { get
            { return _package?.Appliances?.FirstOrDefault(item =>
                     item.Type == ApplianceTypes.SolarStation)?.DataSheet
                     as SolarStationDataSheet ?? null; } }
        public HeatingUnitDataSheet PrimaryData { get
            { return _package?.PrimaryHeatingUnit?.DataSheet
                     as HeatingUnitDataSheet ?? null; } }
        public ContainerDataSheet SolarContainerData { get
            { return _package.SolarContainer.DataSheet as ContainerDataSheet; } }
        #endregion

        private class QnonsolData
        {
            public QnonsolData(float Qsol, float Tout)
            {
                this.Qsol = Qsol;
                this.Tout = Tout;
            }
            public float Qnonsol { get; set; }
            public float Qsol { get; set; }
            public float Ta { get; set; }
            public float Tout { get; set; }
            public float Lwh { get; set; }
            public float Trefw { get; set; }
            public float X { get; set; }
            public float Y { get; set; }
            public float Qbuf { get; set; }
        }
    }
}
