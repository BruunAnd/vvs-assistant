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
        PackagedSolution pack1;
        HeatPumpAsPrimary HeatPumpStrategy = new HeatPumpAsPrimary();

        [SetUp]
        public void Setup()
        {
            //first PackageSolution
            pack1 = new PackagedSolution()
            {
                PrimaryHeatingUnit = new Appliance()
                {
                    Name = "Compress 5000",
                    Type = ApplianceTypes.Heatpump,
                    DataSheet = new HeatingUnitDataSheet() { AFUE = 133, WattUsage = 43, AFUEColdClima = 139, AFUEWarmClima = 136 }
                }
            };

            pack1.Appliances = new List<Appliance>() {
                new Appliance()
                {
                    Name = "Logano Plus",
                    Type = ApplianceTypes.Boiler,
                    DataSheet = new HeatingUnitDataSheet() { AFUE = 91, WattUsage = 18 }
                },

                new Appliance()
                {
                    Name = "Logasol SKN",
                    Type = ApplianceTypes.SolarPanel,
                    DataSheet = new SolarCollectorDataSheet() {Area = 2.25f, Efficency = 60 }
                },

                new Appliance()
                {
                    Name = "Logasol SKN",
                    Type = ApplianceTypes.SolarPanel,
                    DataSheet = new SolarCollectorDataSheet() {Area = 2.25f, Efficency = 60 }
                },

                new Appliance()
                {
                    Name = "BST",
                    Type = ApplianceTypes.Container,
                    DataSheet = new ContainerDataSheet() {Volume = 481, Classification = "C" }
                },
                new Appliance()
                {
                    Name = "Logalux",
                    Type = ApplianceTypes.Container,
                    DataSheet = new ContainerDataSheet() {Volume = 500, Classification = "B"}
                },
                new Appliance()
                {
                    Name = "CW400",
                    Type = ApplianceTypes.TemperatureController,
                    DataSheet = new TemperatureControllerDataSheet() {Class = "6"}
                }
            };

            pack1.SolarContainer = pack1.Appliances?.FirstOrDefault(solCon => solCon.Type == ApplianceTypes.Container && solCon.Name == "BST");
        }

        [TearDown]
        public void TearDown()
        {
            pack1 = null;
        }

        [Test]
        public void HeatPumpAsPrimStrategyReturnsResultsEEUCalculationResult_true()
        {
        Assert.AreEqual(HeatPumpStrategy.CalculateEEI(pack1).GetType(), typeof(EEICalculationResult));
        }

        [Test]
        public void HeatPumpAsPrimStrategyReturnsCorrectEEI_true()
        {
            Assert.AreEqual(HeatPumpStrategy.CalculateEEI(pack1).EEI, 137);
        }

        [Test]
        public void HeatPumpAsPrimaryReturnsCorrectRegEff_true()
        {
            Assert.AreEqual(HeatPumpStrategy.CalculateEEI(pack1).EffectOfTemperatureRegulatorClass, 3.5f);
        }

        [Test]
        public void HeatPumpAsPrimaryReturnsCorrectSecEff_true()
        {
            Assert.AreEqual(HeatPumpStrategy.CalculateEEI(pack1).EffectOfSecondaryBoiler, 0);
        }

        [Test]
        public void HeatPumpAsPrimaryReturnsCorrectSolEff_true()
        {
            Assert.AreEqual(HeatPumpStrategy.CalculateEEI(pack1).SolarHeatContribution, 0.65f);
        }

    }
}
