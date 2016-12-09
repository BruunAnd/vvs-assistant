using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VVSAssistant.Tests.FunctionsTests.CalculationTests.Strategies
{
    using VVSAssistant.Functions.Calculation.Strategies;
    using Models.DataSheets;
    using Models;
    using Functions.Calculation;

    [TestFixture]
    public class BoilerForWaterAsPrimaryTests
    {
        [Test]
        [TestCase(PackagedSolutionId.WaterHeatingEuroACUSBT1003,82)]
        [TestCase(PackagedSolutionId.WaterHeatingEuroACUSBT653, 82)]
        public void WaterPrimaryCalculateEEI_CorrectEnergiEfficiency(PackagedSolutionId packageId, float expected)
        {
            var package = new PackageFactory().GetPackage(packageId);
            var calculation = new BoilerForWater();
            var result = calculation.CalculateEEI(package);
            var EEI = result.WaterHeatingEffciency;
            Assert.IsTrue(expected + 0.1f >= EEI && EEI >= expected - 0.1f);
        }
        [Test]
        [TestCase(PackagedSolutionId.WaterHeatingEuroACUSBT1003,0.1f,20.63f)]
        [TestCase(PackagedSolutionId.WaterHeatingEuroACUSBT653, 0.1f,21.82f)]
        [TestCase(PackagedSolutionId.WaterHeatingCondens9000SBT353,0.1f, 22.42f)]
        [TestCase(PackagedSolutionId.PrimaryPurUnitSolarWater, 1f,21.45f)]
        [TestCase(PackagedSolutionId.PrimaryCondens1Container, 1f, 14.67f)]
        [TestCase(PackagedSolutionId.PrimaryCondens3Container, 1f, 13.65f)]
        [TestCase(PackagedSolutionId.WaterHeatingEuroSolarSBT353, 1f,17.21f)]
        public void WaterPrimaryCalculateEEI_CorrectSolarContribution(PackagedSolutionId packageId, float errorMargin, float expected)
        {
            var package = new PackageFactory().GetPackage(packageId);
            var containers = package.SolarContainers;
            foreach (var item in containers)
            {
                var data = item?.DataSheet as ContainerDataSheet;
                if (data == null)
                    break;
                data.IsBivalent = true;
                data.IsWaterContainer = true;
            }
            var panels = package.Appliances.Where(item => item.Type == ApplianceTypes.SolarPanel);
            foreach (var item in panels)
            {
                var data = item?.DataSheet as SolarCollectorDataSheet;
                if (data == null)
                    break;
                data.IsWaterHeater = true;
            }
            var calculation = new BoilerForWater();
            var result = calculation.CalculateEEI(package);
            var EEI = Math.Round(result.SolarHeatContribution, 3);
            Assert.IsTrue(expected + errorMargin >= EEI && EEI >= expected - errorMargin);
        }
        [Test]
        [TestCase(PackagedSolutionId.WaterHeatingEuroACUSBT1003, 1,103)]
        [TestCase(PackagedSolutionId.WaterHeatingEuroACUSBT653, 1,104)]
        [TestCase(PackagedSolutionId.WaterHeatingCondens9000SBT353,1, 104)]
        [TestCase(PackagedSolutionId.PrimaryWaterBoilerNull,1,82)]
        [TestCase(PackagedSolutionId.PrimaryWaterBoilerOSolar,1,82)]
        [TestCase(PackagedSolutionId.PrimaryPurUnitSolarWater, 1f, 106)]
        [TestCase(PackagedSolutionId.PrimaryPurUnitSolarWaterWStation, 1f, 100)]
        [TestCase(PackagedSolutionId.PrimaryCondens1Container, 1f, 93)]
        [TestCase(PackagedSolutionId.PrimaryCondens3Container, 1f, 92)]
        [TestCase(PackagedSolutionId.WaterHeaterTest, 1f, 116)]
        [TestCase(PackagedSolutionId.Europur1SolarOnly,1,107)]
        //[TestCase(PackagedSolutionId.EuroPurWStationWPanels, 1, 206)]
        public void WaterPrimaryCalculateEEI_CalculatesEEICompletePackagedSolution(PackagedSolutionId packageId, float errormargin,float expected)
        {
            var package = new PackageFactory().GetPackage(packageId);
            var containers = package.SolarContainers;
            foreach (var item in containers)
            {
                var data = item?.DataSheet as ContainerDataSheet;
                if (data == null)
                    break;
                data.IsBivalent = true;
                data.IsWaterContainer = true;
            }
            var panels = package.Appliances.Where(item => item.Type == ApplianceTypes.SolarPanel);
            foreach (var item in panels)
            {
                var data = item?.DataSheet as SolarCollectorDataSheet;
                if (data == null)
                    break;
                data.IsWaterHeater = true;
            }
            var calculation = new BoilerForWater();
            var result = calculation.CalculateEEI(package);
            var EEI = (float)Math.Round(result.EEI);
            //Assert.AreEqual(EEI, expected);
            Assert.IsTrue(expected+errormargin >= EEI && EEI >= expected-errormargin);
        }
        [Test]
        [TestCase(PackagedSolutionId.WaterHeatingEuroACUSBT1003,111)]
        [TestCase(PackagedSolutionId.WaterHeatingEuroACUSBT653,113)]
        [TestCase(PackagedSolutionId.WaterHeatingCondens9000SBT353, 113)]
        [TestCase(PackagedSolutionId.WaterHeatingEuroSolarSBT353, 109)]
        public void WaterPrimaryCalculateEEI_CorrectWarmerEEI(PackagedSolutionId packageId, float expected)
        {
            var package = new PackageFactory().GetPackage(packageId);
            var calculation = new BoilerForWater();
            var containers = package.SolarContainers;
            foreach (var item in containers)
            {
                var data = item?.DataSheet as ContainerDataSheet;
                if (data == null)
                    break;
                data.IsBivalent = true;
                data.IsWaterContainer = true;
            }
            var panels = package.Appliances.Where(item => item.Type == ApplianceTypes.SolarPanel);
            foreach (var item in panels)
            {
                var data = item?.DataSheet as SolarCollectorDataSheet;
                if (data == null)
                    break;
                data.IsWaterHeater = true;
            }
            var result = calculation.CalculateEEI(package);
            var EEI = (float)Math.Round(result.PackagedSolutionAtWarmTemperaturesAFUE);
            Assert.IsTrue(expected + 1f >= EEI && EEI >= expected - 1f);
        }
        [Test]
        [TestCase(PackagedSolutionId.WaterHeatingEuroACUSBT1003,99)]
        [TestCase(PackagedSolutionId.WaterHeatingEuroACUSBT653,99)]
        [TestCase(PackagedSolutionId.WaterHeatingCondens9000SBT353, 100)]
        [TestCase(PackagedSolutionId.WaterHeatingEuroSolarSBT353, 99)]
        public void WaterPrimaryCalculateEEI_CorrectColderEEI(PackagedSolutionId packageId, float expected)
        {
            var package = new PackageFactory().GetPackage(packageId);
            var containers = package.SolarContainers;
            foreach (var item in containers)
            {
                var data = item?.DataSheet as ContainerDataSheet;
                if (data == null)
                    break;
                data.IsBivalent = true;
                data.IsWaterContainer = true;
            }
            var panels = package.Appliances.Where(item => item.Type == ApplianceTypes.SolarPanel);
            foreach (var item in panels)
            {
                var data = item?.DataSheet as SolarCollectorDataSheet;
                if (data == null)
                    break;
                data.IsWaterHeater = true;
            }
            var calculation = new BoilerForWater();
            var result = calculation.CalculateEEI(package);
            var EEI = (float)Math.Round(result.PackagedSolutionAtColdTemperaturesAFUE);
            Assert.IsTrue(expected + 1f >= EEI && EEI >= expected - 1f);
        }

        [Test]
        [TestCase(82)]
        public void Tired(float expected)
        {
            var package = new PackageFactory().GetPackage(PackagedSolutionId.PrimaryWaterBoilerOSolar);
            var containers = package.SolarContainers;
            foreach (var item in containers)
            {
                var data = item?.DataSheet as ContainerDataSheet;
                if (data == null)
                    break;
                data.IsBivalent = true;
                data.IsWaterContainer = true;
            }
            package.Appliances.Add(new ApplianceFactory().GetSolarPanel(SolarPanelId.LogasolSKNWater));
            var solar = package.Appliances.First(item => item.Type == ApplianceTypes.SolarPanel);
            (solar.DataSheet as SolarCollectorDataSheet).IsRoomHeater = true;
            (solar.DataSheet as SolarCollectorDataSheet).IsWaterHeater = false;
            var calculation = new BoilerForWater();
            var result = calculation.CalculateEEI(package);
            var EEI = (float)Math.Round(result.PackagedSolutionAtColdTemperaturesAFUE);
            Assert.IsTrue(expected + 1f >= EEI && EEI >= expected - 1f);
        } 
    }
}