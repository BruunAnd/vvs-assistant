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

namespace VVSAssistant.Tests.FunctionsTests.CalculationTests
{
    [TestFixture]
    public class CalculationManagerTests
    {
        [Test]
        public void SelectCalculationStrategy_ReturnsAEEICalculationType_True()
        {
            var package = new PackagedSolution() { PrimaryHeatingUnit = new Appliance() { Type = ApplianceTypes.Boiler } };
            var calcManager = new CalculationManager();

            Assert.IsTrue(typeof(IEEICalculation).IsAssignableFrom(calcManager.SelectCalculationStreategy(package).GetType()));
        }

        // Integrations Test
        [Test]
        [TestCase(ApplianceTypes.Boiler, null, typeof(BoilerAsPrimary))]
        [TestCase(ApplianceTypes.Heatpump, null, typeof(HeatPumpAsPrimary))]
        [TestCase(ApplianceTypes.LowTempHeatPump, null, typeof(LowTempHeatPumpAsPrimary))]
        //[TestCase(ApplianceTypes.Boiler, ApplianceTypes.SolarPanel, typeof(BoilerForWater))]
        [TestCase(null, ApplianceTypes.Boiler, typeof(CHPStrategy))]
        public void SelectCalculationStrategy_SelectsCorrectStrategy(ApplianceTypes type,
            ApplianceTypes secondApplianceType, Type EEICalculation)
        {
            // Arrange
            var package = new PackagedSolution();
            var calcManager = new CalculationManager();

            //Act
            package.PrimaryHeatingUnit = new Appliance() { Type = type };
            package.Appliances.Add(new Appliance() { Type = secondApplianceType});

            Assert.AreEqual(calcManager.SelectCalculationStreategy(package).GetType(), EEICalculation);
        }
    }
    internal class CalculationManagerStub
    {
        private CalculationManager _calcManager;
        private PackagedSolution _package;
        public CalculationManagerStub(CalculationManager calcManager, PackagedSolution package)
        {
            _calcManager = calcManager;
            _package = package;
        }
    }
}
