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
        public void SelectCalculationStrategyReturnsCorrectCalcTypePrimBoil(PackagedSolutionId packID)
        {
            var package = new PackageFactory().GetPackage(packID);
            var calcManager = new CalculationManager();

            Assert.AreEqual(typeof(BoilerAsPrimary) , calcManager.SelectCalculationStrategy(package)[0].GetType());
        }

        [Test]
        //[TestCase(PackagedSolutionId.PrimaryBoilerWHeatPump)]
        [TestCase(PackagedSolutionId.PrimaryWaterBoilerOSolar)]
        //[TestCase(PackagedSolutionId.PrimaryBoilerSame)]
        //[TestCase(PackagedSolutionId.PrimaryBoilerNulls)]
        [TestCase(PackagedSolutionId.PrimaryBoilerW1Solar)]
        [TestCase(PackagedSolutionId.PrimaryWaterBoilerOSolar)]
        [TestCase(PackagedSolutionId.WaterHeatingEuroSolarSBT353)]
        [TestCase(PackagedSolutionId.WaterHeatingEuroACUSBT653)]
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

        //[Test]
        //public void SelectCalculationStrategy_ReturnsAEEICalculationType_True()
        //{
        //    var package = new PackagedSolution() { PrimaryHeatingUnit = new Appliance() { Type = ApplianceTypes.Boiler } };
        //    var calcManager = new CalculationManager();

        //    Assert.IsTrue(typeof(IEEICalculation).IsAssignableFrom(calcManager.SelectCalculationStreategies(package).GetType()));
        //}

        //// Integration Test between Appliance and CalculationSelector
        //[Test]
        //[TestCase(ApplianceTypes.Boiler, null, typeof(BoilerAsPrimary))]
        //[TestCase(ApplianceTypes.HeatPump, null, typeof(HeatPumpAsPrimary))]
        //[TestCase(ApplianceTypes.LowTempHeatPump, null, typeof(HeatPumpAsPrimary))]
        //[TestCase(ApplianceTypes.Boiler, ApplianceTypes.SolarPanel, typeof(BoilerForWater))]
        //public void SelectCalculationStrategy_SelectsCorrectStrategy(ApplianceTypes type,
        //    ApplianceTypes secondApplianceType, Type EEICalculation)
        //{
        //    // Arrange
        //    var package = new PackagedSolution();
        //    var calcManager = new CalculationManager();

        //    //Act
        //    package.PrimaryHeatingUnit = new Appliance() { Type = type };
        //    package.Appliances.Add(new Appliance() { Type = secondApplianceType});

        //    //Assert
        //    Assert.AreEqual(calcManager.SelectCalculationStreategies(package).GetType(), EEICalculation);
        //}
    }
}
