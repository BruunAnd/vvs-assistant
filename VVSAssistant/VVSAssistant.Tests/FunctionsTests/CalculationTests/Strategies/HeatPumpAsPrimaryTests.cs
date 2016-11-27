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
    public class HeatPumpAsPrimaryStrategyTests
    {
        HeatPumpAsPrimary HeatPumpStrategy = new HeatPumpAsPrimary();

        [SetUp]
        public void Setup()
        {
        }

        [TearDown]
        public void TearDown()
        {
        }

        [Test]
        [TestCase(PackagedSolutionId.PrimaryHeatPump2Solar)]
        [TestCase(PackagedSolutionId.PrimaryHeatPump6Solars)]
        [TestCase(PackagedSolutionId.PrimaryHeatPump4Solars)]
        [TestCase(PackagedSolutionId.PrimaryHeatPumpNoSolars)]
        [TestCase(PackagedSolutionId.PrimaryCHP4Solars)]
        public void HeatPumpAsPrimStrategyReturnsResultsEEUCalculationResult_true(PackagedSolutionId Id)
        {
            var package = new PackageFactory().GetPackage(Id);
            Assert.AreEqual(HeatPumpStrategy.CalculateEEI(package).GetType(), typeof(EEICalculationResult));
        }


        [Test]
        [TestCase(PackagedSolutionId.PrimaryHeatPump2Solar, 137)]
        [TestCase(PackagedSolutionId.PrimaryHeatPump6Solars, 164)]
        [TestCase(PackagedSolutionId.PrimaryHeatPump4Solars, 122)]
        [TestCase(PackagedSolutionId.PrimaryHeatPumpNoSolars, 155)]
        [TestCase(PackagedSolutionId.PrimaryCHP4Solars, 142)]
        public void HeatPumpAsPrimStrategyReturnsCorrectEEI_true(PackagedSolutionId Id, float expectedValue)
        {
            var package = new PackageFactory().GetPackage(Id);
            float EEI = HeatPumpStrategy.CalculateEEI(package).EEI;
            //Assert.AreEqual(EEI, expectedValue);
            Assert.IsTrue(EEI <= expectedValue + 1 && EEI >= expectedValue - 1);
        }


        [Test]
        [TestCase(PackagedSolutionId.PrimaryHeatPump2Solar, 3.5f)]
        [TestCase(PackagedSolutionId.PrimaryHeatPump6Solars, 4)]
        [TestCase(PackagedSolutionId.PrimaryHeatPump4Solars, 2)]
        [TestCase(PackagedSolutionId.PrimaryHeatPumpNoSolars, 4)]
        [TestCase(PackagedSolutionId.PrimaryCHP4Solars, 1.5f)]
        public void HeatPumpAsPrimStrategyReturnsCorrectRegEff_true(PackagedSolutionId Id, float expectedValue)
        {
            var package = new PackageFactory().GetPackage(Id);
            float RegulatorEffect = HeatPumpStrategy.CalculateEEI(package).EffectOfTemperatureRegulatorClass;
            Assert.AreEqual(RegulatorEffect, expectedValue);
            //Assert.IsTrue(RegulatorEffect <= expectedValue + 0.1f && RegulatorEffect >= expectedValue - 0.1f);
        }

        [Test]
        [TestCase(PackagedSolutionId.PrimaryHeatPump2Solar, 0)]
        [TestCase(PackagedSolutionId.PrimaryHeatPump6Solars, 6.6f)]
        [TestCase(PackagedSolutionId.PrimaryHeatPump4Solars, 9.06f)]
        [TestCase(PackagedSolutionId.PrimaryHeatPumpNoSolars, 6.6f)]
        [TestCase(PackagedSolutionId.PrimaryCHP4Solars, 3.15f)]
        public void HeatPumpAsPrimStrategyReturnsCorrectSecEff_true(PackagedSolutionId Id, float expectedValue)
        {
            var package = new PackageFactory().GetPackage(Id);
            float SecondaryEffect = HeatPumpStrategy.CalculateEEI(package).EffectOfSecondaryBoiler;
            //Assert.AreEqual(SecondaryEffect, expectedValue);
            Assert.IsTrue(SecondaryEffect <= expectedValue + 0.1f && SecondaryEffect >= expectedValue - 0.1f);
        }

        [Test]
        [TestCase(PackagedSolutionId.PrimaryHeatPump2Solar, 0.65f)]
        [TestCase(PackagedSolutionId.PrimaryHeatPump6Solars, 8.14f)]
        [TestCase(PackagedSolutionId.PrimaryHeatPump4Solars, 10.69f)]
        [TestCase(PackagedSolutionId.PrimaryHeatPumpNoSolars, 0)]
        [TestCase(PackagedSolutionId.PrimaryCHP4Solars, 3.84f)]
        public void HeatPumpAsPrimStrategyReturnsCorrectSolEff_true(PackagedSolutionId Id, float expectedValue)
        {
            var package = new PackageFactory().GetPackage(Id);
            float solarContribution = HeatPumpStrategy.CalculateEEI(package).SolarHeatContribution;
            Assert.AreEqual(solarContribution, expectedValue);
            //Assert.IsTrue(solarContribution <= expectedValue + 0.1f && solarContribution >= expectedValue - 0.1f);
        }


        [Test]
        [TestCase(PackagedSolutionId.PrimaryHeatPump2Solar, 139)]
        [TestCase(PackagedSolutionId.PrimaryHeatPump6Solars, 170)]
        [TestCase(PackagedSolutionId.PrimaryHeatPump4Solars, 104)]
        [TestCase(PackagedSolutionId.PrimaryHeatPumpNoSolars, 161)]
        public void HeatPumpAsPrimStrategyReturnsCorrectColdAFUE_true(PackagedSolutionId Id, float expectedValue)
        {
            var package = new PackageFactory().GetPackage(Id);
            float ColdAFUE = HeatPumpStrategy.CalculateEEI(package).PackagedSolutionAtColdTemperaturesAFUE;
            //Assert.AreEqual(ColdAFUE, expectedValue);
            Assert.IsTrue(ColdAFUE <= expectedValue + 1 && ColdAFUE >= expectedValue - 1);
        }

        [Test]
        [TestCase(PackagedSolutionId.PrimaryHeatPump2Solar, 136)]
        [TestCase(PackagedSolutionId.PrimaryHeatPump6Solars, 157)]
        [TestCase(PackagedSolutionId.PrimaryHeatPump4Solars, 187)]
        [TestCase(PackagedSolutionId.PrimaryHeatPumpNoSolars, 148)]
        public void HeatPumpAsPrimStrategyReturnsCorrectWarmAFUE_true(PackagedSolutionId Id, float expectedValue)
        {
            var package = new PackageFactory().GetPackage(Id);
            float WarmAFUE = HeatPumpStrategy.CalculateEEI(package).PackagedSolutionAtWarmTemperaturesAFUE;
            //Assert.AreEqual(WarmAFUE, expectedValue);
            Assert.IsTrue(WarmAFUE <= expectedValue + 1 && WarmAFUE >= expectedValue - 1);
        }

    }

 }