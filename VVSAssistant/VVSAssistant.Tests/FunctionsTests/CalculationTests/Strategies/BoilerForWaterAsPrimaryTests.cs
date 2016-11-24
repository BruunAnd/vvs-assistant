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
        public void WaterPrimaryCalculateEEI_GetNulled()
        {

        }

        [Test]
        public void SolCalMethodQaux()
        {
            var package = new PackageFactory().GetPackage(PackagedSolutionId.WaterHeatingEuroACUSBT1003);
            var calculation = new BoilerForWater();
            calculation.CalculateEEI(package);
            var result = calculation.SolCalMethodQaux();

            Assert.AreEqual(result, 114);
        }
        [Test]
        [TestCase(PackagedSolutionId.WaterHeatingEuroACUSBT1003,3074)]
        public void SolcalMethodQnonsol_CalculatesQnonsol(PackagedSolutionId packageId, float expected)
        {
            var package = new PackageFactory().GetPackage(packageId);
            var calculation = new BoilerForWater();
            calculation.CalculateEEI(package);
            float QnonsolTest = calculation.SolCalMethodQnonsol();
            
            Assert.AreEqual(expected, Math.Round(QnonsolTest));
        }

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
        [TestCase(PackagedSolutionId.WaterHeatingEuroACUSBT1003,20.63f)]
        [TestCase(PackagedSolutionId.WaterHeatingEuroACUSBT653, 21.82f)]
        [TestCase(PackagedSolutionId.WaterHeatingCondens9000SBT353, 22.42f)]
        // Test med Vbu over 0 virker ikke
        //[TestCase(PackagedSolutionId.WaterHeatingEuroSolarSBT353, 17.21f)]
        public void WaterPrimaryCalculateEEI_CorrectSolarContribution(PackagedSolutionId packageId, float expected)
        {
            var package = new PackageFactory().GetPackage(packageId);
            var calculation = new BoilerForWater();
            var result = calculation.CalculateEEI(package);
            var EEI = Math.Round(result.SolarHeatContribution, 3);

            Assert.IsTrue(expected + 0.1f >= EEI && EEI >= expected - 0.1f);
        }
        [Test]
        [TestCase(PackagedSolutionId.WaterHeatingEuroACUSBT1003, 103)]
        [TestCase(PackagedSolutionId.WaterHeatingEuroACUSBT653, 104)]
        [TestCase(PackagedSolutionId.WaterHeatingCondens9000SBT353, 104)]
        //[TestCase(PackagedSolutionId.PrimaryWaterBoilerNull,1)]
        //[TestCase(PackagedSolutionId.PrimaryWaterBoilerOSolar,1)]
        //[TestCase(PackagedSolutionId.WaterHeatingEuroSolarSBT353, 102)]
        public void WaterPrimaryCalculateEEI_CalculatesEEICompletePackagedSolution(PackagedSolutionId packageId, float expected)
        {
            var package = new PackageFactory().GetPackage(packageId);
            var calculation = new BoilerForWater();
            var result = calculation.CalculateEEI(package);
            var EEI = (float)Math.Round(result.EEI);
            Assert.IsTrue(expected+1f >= EEI && EEI >= expected-1f);
        }
        [Test]
        [TestCase(PackagedSolutionId.WaterHeatingEuroACUSBT1003,111)]
        [TestCase(PackagedSolutionId.WaterHeatingEuroACUSBT653,113)]
        [TestCase(PackagedSolutionId.WaterHeatingCondens9000SBT353, 113)]
        //[TestCase(PackagedSolutionId.WaterHeatingEuroSolarSBT353, 109)]
        public void WaterPrimaryCalculateEEI_CorrectWarmerEEI(PackagedSolutionId packageId, float expected)
        {
            var package = new PackageFactory().GetPackage(packageId);
            var calculation = new BoilerForWater();
            var result = calculation.CalculateEEI(package);
            var EEI = (float)Math.Round(result.PackagedSolutionAtWarmTemperaturesAFUE);
            Assert.IsTrue(expected + 1f >= EEI && EEI >= expected - 1f);
        }
        [Test]
        [TestCase(PackagedSolutionId.WaterHeatingEuroACUSBT1003,99)]
        [TestCase(PackagedSolutionId.WaterHeatingEuroACUSBT653,99)]
        [TestCase(PackagedSolutionId.WaterHeatingCondens9000SBT353, 100)]
        //[TestCase(PackagedSolutionId.WaterHeatingEuroSolarSBT353, 99)]
        public void WaterPrimaryCalculateEEI_CorrectColderEEI(PackagedSolutionId packageId, float expected)
        {
            var package = new PackageFactory().GetPackage(packageId);
            var calculation = new BoilerForWater();
            var result = calculation.CalculateEEI(package);
            var EEI = (float)Math.Round(result.PackagedSolutionAtColdTemperaturesAFUE);
            Assert.IsTrue(expected + 1f >= EEI && EEI >= expected - 1f);
        }
    }
}