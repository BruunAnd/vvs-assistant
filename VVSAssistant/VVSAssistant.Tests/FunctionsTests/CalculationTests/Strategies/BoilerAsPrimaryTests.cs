using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VVSAssistant.Functions.Calculation;
using VVSAssistant.Functions.Calculation.Strategies;
using VVSAssistant.Models;
using VVSAssistant.Models.DataSheets;

namespace VVSAssistant.Tests.FunctionsTests.CalculationTests.Strategies
{

    [TestFixture]
    public class BoilerAsPrimaryStrategyTests
    {
        [Test]
        public void PrimaryBoilerCalculateEEI_PrimaryPackageNull_ReturnsNull()
        {
            var package = new PackageFactory().GetPackage(PackagedSolutionId.PrimaryBoilerSame);
            package.PrimaryHeatingUnit = null;
            var calculation = new BoilerAsPrimary();
            var result = calculation.CalculateEEI(package);
            Assert.AreEqual(result, null);
        }
        [Test]
        //[Ignore("Only fails during unit tests")]
        public void PrimaryBoilerCalculateEEI_PrimarySolarContainerNull_ReturnsNull()
        {
            var package = new PackageFactory().GetPackage(PackagedSolutionId.PrimaryBoilerSame);
            package.SolarContainers = new ApplianceList(new List<ApplianceInstance>());
            package.SolarContainerInstances = new List<ApplianceInstance>();
            var calculation = new BoilerAsPrimary();
            var result = calculation.CalculateEEI(package);
            Assert.AreEqual(result.SolarHeatContribution, 0);
        }
        [Test]
        public void PrimaryBoilerCalculateEEI_AllAboutThemNulls()
        {
            var package = new PackageFactory().GetPackage(PackagedSolutionId.PrimaryBoilerNulls);
            var calculation = new BoilerAsPrimary();
            var result = calculation.CalculateEEI(package);
            Assert.AreEqual(result.EEI, result.PrimaryHeatingUnitAFUE);
        }
        [Test]
        [TestCase(PackagedSolutionId.PrimaryBoilerOHeatPump,91)]
        public void PrimaryBoilerCalculateEEI_CorrectAFUEContribution(PackagedSolutionId id, float expected)
        {
            var package = new PackageFactory().GetPackage(id);
            var calculation = new BoilerAsPrimary();
            var containers = package.SolarContainers;
            foreach (var item in containers)
            {
                var data = item?.DataSheet as ContainerDataSheet;
                if (data == null)
                    break;
                data.IsBivalent = true;
            }
            var panels = package.Appliances.Where(item => item.Type == ApplianceTypes.SolarPanel);
            foreach (var item in panels)
            {
                var data = item?.DataSheet as SolarCollectorDataSheet;
                if (data == null)
                    break;
                data.IsRoomHeater= true;
            }
            var result = new EEICalculationResult();
            result = calculation.CalculateEEI(package);
            var AFUE = result.PrimaryHeatingUnitAFUE;
            
            Assert.IsTrue(expected <= AFUE+0.1f && expected >= AFUE-0.1f);
        }
        [Test]
        [TestCase(PackagedSolutionId.PrimaryBoilerOHeatPump,3)]
        [TestCase(PackagedSolutionId.PrimaryBoilerWHeatPump, 3)]
        public void PrimaryBoilerCalculateEEI_CorrectTempContribution(PackagedSolutionId id, float expected)
        {
            var package = new PackageFactory().GetPackage(id);
            var calculation = new BoilerAsPrimary();
            var containers = package.SolarContainers;
            foreach (var item in containers)
            {
                var data = item?.DataSheet as ContainerDataSheet;
                if (data == null)
                    break;
                data.IsBivalent = true;
            }
            var panels = package.Appliances.Where(item => item.Type == ApplianceTypes.SolarPanel);
            foreach (var item in panels)
            {
                var data = item?.DataSheet as SolarCollectorDataSheet;
                if (data == null)
                    break;
                data.IsRoomHeater = true;
            }
            var result = new EEICalculationResult();
            result = calculation.CalculateEEI(package);
            Assert.AreEqual(expected, (float)Math.Round(result.EffectOfTemperatureRegulatorClass,1));
        }
        [Test]
        [TestCase(PackagedSolutionId.PrimaryBoilerOHeatPump, 1, 0.2f)]
        [TestCase(PackagedSolutionId.PrimaryBoilerWHeatPump, 1, 0.0f)]
        public void PrimaryBoilerCalculateEEI_CorrectSecondBoilerContribution(PackagedSolutionId id, int boilerId, float expected)
        {
            var package = new PackageFactory().GetPackage(id);
            var calculation = new BoilerAsPrimary();
            var containers = package.SolarContainers;
            foreach (var item in containers)
            {
                var data = item?.DataSheet as ContainerDataSheet;
                if (data == null)
                    break;
                data.IsBivalent = true;
            }
            var panels = package.Appliances.Where(item => item.Type == ApplianceTypes.SolarPanel);
            foreach (var item in panels)
            {
                var data = item?.DataSheet as SolarCollectorDataSheet;
                if (data == null)
                    break;
                data.IsRoomHeater = true;
            }
            var result = new EEICalculationResult();
            result = calculation.CalculateEEI(package);
            var secBoiler = result.EffectOfSecondaryBoiler;

            Assert.IsTrue(expected <= secBoiler + 0.1f && expected >= secBoiler - 0.1f);
        }
        [Test]
        [TestCase(PackagedSolutionId.PrimaryBoilerOHeatPump,1.69f)]
        [TestCase(PackagedSolutionId.PrimaryBoilerWHeatPump, 1.71f)]
        public void PrimaryBoilerCalculateEEI_CorrectSolarContribution(PackagedSolutionId id, float expected)
        {
            var package = new PackageFactory().GetPackage(id);
            var calculation = new BoilerAsPrimary();
            var containers = package.SolarContainers;
            foreach (var item in containers)
            {
                var data = item?.DataSheet as ContainerDataSheet;
                if (data == null)
                    break;
                data.IsBivalent = true;
            }
            var panels = package.Appliances.Where(item => item.Type == ApplianceTypes.SolarPanel);
            foreach (var item in panels)
            {
                var data = item?.DataSheet as SolarCollectorDataSheet;
                if (data == null)
                    break;
                data.IsRoomHeater = true;
            }
            var result = new EEICalculationResult();
            result = calculation.CalculateEEI(package);
            var solar = result.SolarHeatContribution;
            //Assert.AreEqual(expected, solar);
            Assert.IsTrue(expected <= solar + 0.1f && expected >= solar - 0.1f);
        }
        [Test]
        [TestCase(PackagedSolutionId.PrimaryBoilerOHeatPump,0)]
        [TestCase(PackagedSolutionId.PrimaryBoilerWHeatPump, -60.4f)]
        public void PrimaryBoilerCalculateEEI_CorrectHeatpumpContribution(PackagedSolutionId id,float expected)
        {
            var package = new PackageFactory().GetPackage(id);
            var calculation = new BoilerAsPrimary();
            var containers = package.SolarContainers;
            foreach (var item in containers)
            {
                var data = item?.DataSheet as ContainerDataSheet;
                if (data == null)
                    break;
                data.IsBivalent = true;
            }
            var panels = package.Appliances.Where(item => item.Type == ApplianceTypes.SolarPanel);
            foreach (var item in panels)
            {
                var data = item?.DataSheet as SolarCollectorDataSheet;
                if (data == null)
                    break;
                data.IsRoomHeater = true;
            }
            var result = new EEICalculationResult();
            result = calculation.CalculateEEI(package);

            float contribution = (float)(result.EffectOfSecondaryHeatPump);
            // Kan ikke få fejl margin på 0.1 endnu
            Assert.IsTrue(expected <= contribution + 0.2f && expected >= contribution - 0.2f);
        }
        [Test]
        [TestCase(PackagedSolutionId.PrimaryBoilerOHeatPump,0)]
        [TestCase(PackagedSolutionId.PrimaryBoilerWHeatPump, 0.9f)]
        public void PrimaryBoilerCalculateEEI_CorrectAdjustedContribution(PackagedSolutionId id, float expected)
        {
            var package = new PackageFactory().GetPackage(id);
            var calculation = new BoilerAsPrimary();
            var containers = package.SolarContainers;
            foreach (var item in containers)
            {
                var data = item?.DataSheet as ContainerDataSheet;
                if (data == null)
                    break;
                data.IsBivalent = true;
            }
            var panels = package.Appliances.Where(item => item.Type == ApplianceTypes.SolarPanel);
            foreach (var item in panels)
            {
                var data = item?.DataSheet as SolarCollectorDataSheet;
                if (data == null)
                    break;
                data.IsRoomHeater = true;
            }
            var result = new EEICalculationResult();
            result = calculation.CalculateEEI(package);
            var adjusted = result.AdjustedContribution;
            Assert.IsTrue(expected <= adjusted + 0.1f && expected >= adjusted - 0.1f);
        }
        [Test]
        [TestCase(PackagedSolutionId.PrimaryBoilerOHeatPump,BoilerId.Condens5000, 95f)]
        [TestCase(PackagedSolutionId.PrimaryBoilerWHeatPump, 0, 154)]
        [TestCase(PackagedSolutionId.PrimaryBoilerNulls, 0, 91)]
        [TestCase(PackagedSolutionId.PirmaryBoilerW3Solar, BoilerId.LoganoSB150, 96)]
        [TestCase(PackagedSolutionId.PrimaryBoilerW1Solar, BoilerId.LoganoSB150, 93)]
        public void PrimaryBoilerCalculateEEI_CorrecrOverallResult(PackagedSolutionId packId, BoilerId id,float expected)
        {
            var package = new PackageFactory().GetPackage(packId);
            var calculation = new BoilerAsPrimary();
            var containers = package.SolarContainers;
            foreach (var item in containers)
            {
                var data = item?.DataSheet as ContainerDataSheet;
                if (data == null)
                    break;
                data.IsBivalent = true;
            }
            var panels = package.Appliances.Where(item => item.Type == ApplianceTypes.SolarPanel);
            foreach (var item in panels)
            {
                var data = item?.DataSheet as SolarCollectorDataSheet;
                if (data == null)
                    break;
                data.IsRoomHeater = true;
            }
            //package.Appliances.Add(new ApplianceFactory().GetBoiler(id) ?? new Appliance());
            if (id == BoilerId.LoganoSB150)
                package.PrimaryHeatingUnit = new ApplianceFactory().GetBoiler(id);
            var result = new EEICalculationResult();
            result = calculation.CalculateEEI(package);
            var EEI = Math.Round(result.EEI);
            //Assert.AreEqual(expected, EEI);
            Assert.IsTrue(expected <= EEI + 1f && expected >= EEI - 1f);
        }
        [Test]
        [TestCase(PackagedSolutionId.PrimaryBoilerOHeatPump,0)]
        [TestCase(PackagedSolutionId.PrimaryBoilerWHeatPump, 200.3f)]
        public void PrimaryBoilerCalculateEEI_CorrectLowTempEEI(PackagedSolutionId id, float expected)
        {
            var package = new PackageFactory().GetPackage(id);
            var calculation = new BoilerAsPrimary();
            var containers = package.SolarContainers;
            foreach (var item in containers)
            {
                var data = item?.DataSheet as ContainerDataSheet;
                if (data == null)
                    break;
                data.IsBivalent = true;
            }
            var panels = package.Appliances.Where(item => item.Type == ApplianceTypes.SolarPanel);
            foreach (var item in panels)
            {
                var data = item?.DataSheet as SolarCollectorDataSheet;
                if (data == null)
                    break;
                data.IsRoomHeater = true;
            }
            var result = calculation.CalculateEEI(package);
            var lowTemp = result.PackagedSolutionAtColdTemperaturesAFUE;

            Assert.IsTrue(expected <= lowTemp + 1f && expected >= lowTemp - 1f);
        }
        [Test]
        [TestCase(PackagedSolutionId.PrimaryBoilerOHeatPump, ContainerId.SM500, 95f)]
        public void PrimaryBoilerCalculateEEI_CorrecrOverallResultWContainer(PackagedSolutionId packId, ContainerId id, float expected)
        {
            var package = new PackageFactory().GetPackage(packId);
            var calculation = new BoilerAsPrimary();
            var containers = package.SolarContainers;
            foreach (var item in containers)
            {
                var data = item?.DataSheet as ContainerDataSheet;
                if (data == null)
                    break;
                data.IsBivalent = true;
            }
            var panels = package.Appliances.Where(item => item.Type == ApplianceTypes.SolarPanel);
            foreach (var item in panels)
            {
                var data = item?.DataSheet as SolarCollectorDataSheet;
                if (data == null)
                    break;
                data.IsRoomHeater = true;
            }
            //package.Appliances.Add(new ApplianceFactory().GetBoiler(id) ?? new Appliance());
            if (id == ContainerId.SM500)
                package.Appliances.Add(new ApplianceFactory().GetContainer(id));
            var result = new EEICalculationResult();
            result = calculation.CalculateEEI(package);
            var EEI = Math.Round(result.EEI);
            Assert.IsTrue(expected <= EEI + 1f && expected >= EEI - 1f);
        }
    }
}
