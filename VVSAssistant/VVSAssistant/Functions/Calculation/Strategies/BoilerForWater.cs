using System;
using System.Collections.Generic;
using System.Linq;
using VVSAssistant.Models;
using VVSAssistant.Models.DataSheets;

namespace VVSAssistant.Functions.Calculation.Strategies
{
    internal class BoilerForWater : IEEICalculation
    {
        private PackagedSolution _package;
        private PackageDataManager _packageData;
        private readonly EEICalculationResult _result = new EEICalculationResult();
        /// <summary>
        /// Calculation method for Water heating with a primary Boiler
        /// </summary>
        /// <param name="package"></param>
        /// <returns>EEICalculationResult which contains the variables used for the calculation,
        /// the energy effiency index and the calculation method used </returns>
        public EEICalculationResult CalculateEEI(PackagedSolution package)
        {
            _package = package;
            _packageData = new PackageDataManager(_package);
            if (PrimaryData == null)
                return null;
            var data = WaterHeater ?? PrimaryData;
            _result.WaterHeatingEffciency = data.WaterHeatingEffiency;
            _result.WaterHeatingUseProfile = data.UseProfile;

            var qref = _qref[_result.WaterHeatingUseProfile];
            var qaux = SolCalMethodQaux();
            var qnonsol = SolCalMethodQnonsol();

            var I = _result.WaterHeatingEffciency;
            var II = Math.Abs(qnonsol) > 0 ? (220 * qref) / qnonsol : 0;

            var III = Math.Abs(qaux) > 0 ? (float)(((qaux * 2.5) / (220 * qref)) * 100) : 0;
            _result.SolarHeatContribution = qaux.Equals(0) || qnonsol.Equals(0) ? 0 :
                (1.1f * I - 10) * II - III - I;

            _result.EEI = (float)Math.Round(_result.SolarHeatContribution + I);

            _result.PackagedSolutionAtColdTemperaturesAFUE = _result.EEI - 0.2f * _result.SolarHeatContribution;
            _result.PackagedSolutionAtWarmTemperaturesAFUE = _result.EEI + 0.4f * _result.SolarHeatContribution;
            _result.CalculationType = _packageData.CalculationStrategyType(_package, _result);
            _result.EEICharacters = EEICharLabelChooser.EEIChar(_result.WaterHeatingUseProfile, _result.EEI, 1.5f)[0];
            _result.ToNextLabel = EEICharLabelChooser.EEIChar(_result.WaterHeatingUseProfile, _result.EEI, 1.5f)[1];
            _result.ProceedingEEICharacter = EEICharLabelChooser.EEIChar(_result.WaterHeatingUseProfile, _result.EEI, 1.5f)[2];
            return _result;
        }

        private readonly Dictionary<UseProfileType, float> _qref = new Dictionary<UseProfileType, float>
        {
            {UseProfileType.M, 5.845f}, {UseProfileType.L, 11.655f}, {UseProfileType.XL, 19.070f}, {UseProfileType.XXL, 24.530f}
        };
        // Averge solar iradiance and averge temperatur per month 
        private readonly Dictionary<string, QnonsolData> _monthlyQnonsol = new Dictionary<string, QnonsolData>
        {
            {"Jan", new QnonsolData(70f, 2.8f) }, {"Feb", new QnonsolData(104f, 2.6f)}, {"Mar", new QnonsolData(149f,7.4f)},
            {"Apr", new QnonsolData(192f, 12.2f)}, {"May", new QnonsolData(221f, 16.3f)}, {"Jun", new QnonsolData(222f, 19.8f)},
            {"Jul", new QnonsolData(232f, 21f)}, {"Aug", new QnonsolData(217f, 22f)}, {"Sep", new QnonsolData(176f, 17f)},
            {"Oct", new QnonsolData(129f, 11.9f)}, {"Nov", new QnonsolData(80f, 5.6f)}, {"Dec", new QnonsolData(56f, 3.2f)}
        };
        // Calculates the Qaux (auxiliary electricity consumption)
        private float SolCalMethodQaux()
        {
            var solpumpConconsumption = PumpConsumption;
            var solstandbyConsumption = StandbyConsumption;
            // 2000 active solar hours 
            var qaux = solpumpConconsumption <= 0 || solstandbyConsumption <= 0 ?
                         0 :
                         (float)Math.Ceiling(((solpumpConconsumption * 2000) +
                         (solstandbyConsumption * 24 * 365)) / 1000);
            return qaux;
        }
        // Calculates the Qnonsol (annual non-solar contribution)
        private float SolCalMethodQnonsol()
        {
            float qnonsol = 0;
            var area = _packageData.SolarPanelArea(x => x.IsUsedForWaterHeating);
            // Vbu is used in the getter properties, and used as zero for the rest of the calculation
            // since no documentation properly speciefies how to use the Vbu value elsewhere.
            float vbu = 0;
            var vnorm = VnormPackage;
            var psbsol = PsbsolPackage;
            var solarData = _packageData.SolarPanelData(item => item.IsUsedForWaterHeating == true);
            if (vnorm <= 0 || psbsol <= 0 || solarData == null || area < 0.1f)
                return 0;
            // Monthly Qnonsol values, needs to be summed to get the full Qnonsol
            foreach (var keyvalue in _monthlyQnonsol.Keys)
            {
                var item = _monthlyQnonsol[keyvalue];
                // Averge temp surrounding the heat store, 20 if inside and Tout if outside
                float Ta = 20;

                var etaloop = 1 - ((solarData.N0 * solarData.a1) / 100);
                var Ac = solarData.a1 + solarData.a2 * 40;
                var Ul = (6 + 0.3f * area) / area;

                var Vsol = vnorm * (1 - 1 * (vbu / vnorm));
                //Vnorm = Vnorm - Vbu;
                var ccap = (float)Math.Pow(75 * area / Vsol, 0.25f);

                item.Lwh = 30.5f * 0.6f * (_qref[_result.WaterHeatingUseProfile] + 1.09f);
                item.Y = area * solarData.IAM * solarData.N0 * etaloop * item.Qsol * (0.732f / item.Lwh);
                item.Trefw = 11.6f + 1.18f * 40 + 3.86f * 10 - 1.32f * item.Tout;
                item.X = area * (Ac + Ul) * etaloop * (item.Trefw - item.Tout) *
                         ccap * 0.732f / item.Lwh;

                var LsolW1 = item.Lwh * (1.029f * item.Y - 0.065f * item.X - 0.245f *
                                           (float)Math.Pow(item.Y, 2) + 0.0018f *
                                           (float)Math.Pow(item.X, 2) + 0.0215f *
                                           (float)Math.Pow(item.Y, 3));
                // Standing loss in W/k
                item.Qbuf = 0.732f * psbsol * ((Vsol) /
                                               vnorm) * (10 + (50 * LsolW1) / item.Lwh - Ta);
                var LsolW = LsolW1 - item.Qbuf;
                item.Qnonsol = item.Lwh - LsolW + psbsol *
                               (vbu / vnorm) * (60 - Ta) * 0.732f;

                qnonsol += item.Qnonsol;
            }

            return qnonsol;
        }

        public HeatingUnitDataSheet PrimaryData => _package?.PrimaryHeatingUnitInstance?.Appliance?.DataSheet as HeatingUnitDataSheet;

        public HeatingUnitDataSheet WaterHeater => _package.ApplianceInstances.FirstOrDefault(item =>
                item.Appliance.Type == ApplianceTypes.WaterHeater)?.Appliance?.DataSheet as HeatingUnitDataSheet;

        private float PumpConsumption
        {
            get
            {
                float ans = 0;
                var stationData = _packageData.SolarStationData;
                ans += PrimaryData.Vnorm > 0 ? PrimaryData.WattUsage : 0;
                ans += stationData?.SolPumpConsumption ?? 0;
                return ans;
            }
        }
        private float StandbyConsumption
        {
            get
            {
                float ans = 0;
                var stationData = _packageData.SolarStationData;
                ans += PrimaryData.Vnorm > 0 ? PrimaryData.Psb : 0;
                ans += stationData?.SolStandbyConsumption ?? 0;
                return ans;
            }
        }
        private float VnormPackage
        {
            get
            {
                float ans = 0;
                ans += PrimaryData.Vnorm > 0 ? PrimaryData.Vnorm : 0;
                ans -= PrimaryData.Vnorm > 0 ? PrimaryData.Vbu : 0;
                ans += ans.Equals(0) && WaterHeater != null ? WaterHeater.Vnorm : 0;
                ans -= ans.Equals(0) && WaterHeater != null ? WaterHeater.Vbu : 0;
                ans += _packageData.SolarContainerVolume(container =>
                    container.IsBivalent && container.IsWaterContainer);
                return ans;
            }
        }
        private float PsbsolPackage
        {
            get
            {
                float ans = 0;
                ans += PrimaryData.Vnorm > 0 ? PrimaryData.StandingLoss / 45 : 0;
                ans += ans.Equals(0) && WaterHeater != null ? WaterHeater.StandingLoss / 45 : 0;
                var solarContains = _packageData.SolarContainers(container =>
                    container.IsWaterContainer && container.IsBivalent);
                ans += solarContains.Sum(item => (item?.Appliance?.DataSheet as ContainerDataSheet)?.StandingLoss / 45 ?? 0);
                return ans;
            }
        }

        private class QnonsolData
        {
            public QnonsolData(float qsol, float tout)
            {
                Qsol = qsol;
                Tout = tout;
            }

            public float Qnonsol { get; set; }
            public float Qsol { get; }
            public float Tout { get; }
            public float Lwh { get; set; }
            public float Trefw { get; set; }
            public float X { get; set; }
            public float Y { get; set; }
            public float Qbuf { get; set; }
        }
    }
}
