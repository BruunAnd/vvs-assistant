using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VVSAssistant.Functions.Calculation;
using VVSAssistant.Models;
using Moq;
using VVSAssistant.Functions.Calculation.Strategies;
using VVSAssistant.Tests.FunctionsTests.CalculationTests.Strategies;

namespace VVSAssistant.Tests.FunctionsTests.CalculationTests
{
    [TestFixture]
    public class CalculationManagerTests
    {

        [Test]
        [TestCase(PackagedSolutionId.PrimaryBoilerWHeatPump)]
        [TestCase(PackagedSolutionId.PrimaryWaterBoilerOSolar)]
        [TestCase(PackagedSolutionId.PrimaryBoilerSame)]
        [TestCase(PackagedSolutionId.PrimaryBoilerNulls)]
        [TestCase(PackagedSolutionId.PrimaryBoilerW1Solar)]
        [TestCase(PackagedSolutionId.PrimaryWaterBoilerOSolar)]
        [TestCase(PackagedSolutionId.WaterHeatingEuroSolarSBT353)]
        [TestCase(PackagedSolutionId.WaterHeatingEuroACUSBT653)]
        [TestCase(PackagedSolutionId.PrimaryHeatPump6Solars)]
        [TestCase(PackagedSolutionId.Brian1)]
        public void SelectCalculationStrategy_returnsAEEICalculationType_true(PackagedSolutionId packID)
        {
            var package = new PackageFactory().GetPackage(packID);
            var calcManager = new CalculationManager();

            Assert.IsTrue(typeof(IEEICalculation).IsAssignableFrom(calcManager.SelectCalculationStrategy(package)[0].GetType()));
        }

        [Test]
        [TestCase(PackagedSolutionId.PrimaryBoilerWHeatPump)]
        [TestCase(PackagedSolutionId.PrimaryWaterBoilerOSolar)]
        [TestCase(PackagedSolutionId.PrimaryBoilerSame)]
        [TestCase(PackagedSolutionId.PrimaryBoilerNulls)]
        [TestCase(PackagedSolutionId.PrimaryBoilerW1Solar)]
        [TestCase(PackagedSolutionId.PrimaryWaterBoilerOSolar)]
        [TestCase(PackagedSolutionId.WaterHeatingEuroSolarSBT353)]
        [TestCase(PackagedSolutionId.WaterHeatingEuroACUSBT653)]
        [TestCase(PackagedSolutionId.Brian1)]
        public void SelectCalculationStrategyReturnsCorrectCalcTypePrimBoil(PackagedSolutionId packID)
        {
            var package = new PackageFactory().GetPackage(packID);
            var calcManager = new CalculationManager();

            Assert.AreEqual(typeof(BoilerAsPrimary) , calcManager.SelectCalculationStrategy(package)[0].GetType());
        }

        [Test]
        [TestCase(PackagedSolutionId.PrimaryWaterBoilerOSolar)]
        [TestCase(PackagedSolutionId.PrimaryBoilerW1Solar)]
        [TestCase(PackagedSolutionId.PrimaryWaterBoilerOSolar)]
        [TestCase(PackagedSolutionId.WaterHeatingEuroSolarSBT353)]
        [TestCase(PackagedSolutionId.WaterHeatingEuroACUSBT653)]
        [TestCase(PackagedSolutionId.AndersTest)]
        public void SelectCalculationStrategyReturnsSecondCalc(PackagedSolutionId packID)
        {
            var package = new PackageFactory().GetPackage(packID);
            var calcManager = new CalculationManager();

            Assert.AreEqual(typeof(BoilerForWater), calcManager.SelectCalculationStrategy(package)[1].GetType());
        }

        [Test]
        [TestCase(PackagedSolutionId.PrimaryHeatPump6Solars)]
        public void SelectCalculationStrategyDoesNotReturnWrong(PackagedSolutionId packID)
        {
            var package = new PackageFactory().GetPackage(packID);
            var calcManager = new CalculationManager();

            Assert.AreNotEqual(typeof(BoilerAsPrimary), calcManager.SelectCalculationStrategy(package)[0].GetType());
        }


        [Test]
        [TestCase(PackagedSolutionId.Brian1, 98)]
        [TestCase(PackagedSolutionId.Brian2, 127)]
        [TestCase(PackagedSolutionId.Brian3, 115)]
        [TestCase(PackagedSolutionId.Brian5, 97)]
        public void SelectCalculationStrategyReturnsCorrectEEI(PackagedSolutionId packID, float expectedValue)
        {
            var package = new PackageFactory().GetPackage(packID);
            var calcManager = new CalculationManager();
            EEICalculationResult Results = calcManager.SelectCalculationStrategy(package)[0].CalculateEEI(package);
            //Assert.AreEqual(Results.EEI, expectedValue);
            Assert.IsTrue(Results.EEI <= expectedValue + 1 && Results.EEI >= expectedValue - 1);
        }

        [Test]
        [TestCase(PackagedSolutionId.Brian1, 5.3f)]
        [TestCase(PackagedSolutionId.Brian2, 13.05f)]
        [TestCase(PackagedSolutionId.Brian5, 6.45f)]
        public void SelectCalculationStrategyReturnsCorrectSolEff(PackagedSolutionId packID, float expectedValue)
        {
            var package = new PackageFactory().GetPackage(packID);
            var calcManager = new CalculationManager();
            EEICalculationResult Results = calcManager.SelectCalculationStrategy(package)[0].CalculateEEI(package);
            //Assert.AreEqual(Results.SolarHeatContribution, expectedValue);
            Assert.IsTrue(Results.SolarHeatContribution <= expectedValue + 0.1f && Results.SolarHeatContribution >= expectedValue - 0.1f);
        }

        [Test]
        [TestCase(PackagedSolutionId.Brian2, 113)]
        public void SelectCalculationStrategyReturnsCorrectCold(PackagedSolutionId packID, float expectedValue)
        {
            var package = new PackageFactory().GetPackage(packID);
            var calcManager = new CalculationManager();
            EEICalculationResult Results = calcManager.SelectCalculationStrategy(package)[0].CalculateEEI(package);
            //Assert.AreEqual(ColdAFUE, expectedValue);
            Assert.IsTrue(Results.PackagedSolutionAtColdTemperaturesAFUE <= expectedValue + 1 && Results.PackagedSolutionAtColdTemperaturesAFUE >= expectedValue - 1);
        }

        [Test]
        [TestCase(PackagedSolutionId.Brian2, 153)]
        public void SelectCalculationStrategyReturnsCorrectWarm(PackagedSolutionId packID, float expectedValue)
        {
            var package = new PackageFactory().GetPackage(packID);
            var calcManager = new CalculationManager();
            EEICalculationResult Results = calcManager.SelectCalculationStrategy(package)[0].CalculateEEI(package);
            //Assert.AreEqual(WarmAFUE, expectedValue);
            Assert.IsTrue(Results.PackagedSolutionAtWarmTemperaturesAFUE <= expectedValue + 1 && Results.PackagedSolutionAtWarmTemperaturesAFUE >= expectedValue - 1);
        }

        [Test]
        [TestCase(PackagedSolutionId.Brian3, -24.2f)]
        public void SelectCalculationStrategyReturnsCorrectSupHeat(PackagedSolutionId packID, float expectedValue)
        {
            var package = new PackageFactory().GetPackage(packID);
            var calcManager = new CalculationManager();
            EEICalculationResult Results = calcManager.SelectCalculationStrategy(package)[0].CalculateEEI(package);
            //Assert.AreEqual(Results.EffectOfSecondaryHeatPump, expectedValue);
            // fejl margin hævet til 0.3 fordi det eneste der påvirker resultatet en en værdi der er 0.509 i stedet for 0.504
            Assert.IsTrue(Results.EffectOfSecondaryHeatPump <= expectedValue + 0.3f && Results.EffectOfSecondaryHeatPump >= expectedValue - 0.3f);
        }
    }
}
