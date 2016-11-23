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
        PackagedSolution CurrentPack;
        HeatPumpAsPrimary HeatPumpStrategy = new HeatPumpAsPrimary();

        [SetUp]
        public void Setup()
        {
        }
        
        [TearDown]
        public void TearDown()
        {
            CurrentPack = null;
        }

        [Test]
        [TestCase(ApplianceTypes.Heatpump, 133, 43, 139, 136, ApplianceTypes.Boiler, 91, 18, 2.25f, 60, 2, 481, "C", 500, "B", "7", 1)]
        [TestCase(ApplianceTypes.Heatpump, 158, 10, 164, 151, ApplianceTypes.Boiler, 91, 18, 2.25f, 61, 6, 489.3f, "E", 741, "E", "6", 1)]
        [TestCase(ApplianceTypes.Heatpump, 118, 9, 100, 183, ApplianceTypes.Boiler, 94, 42, 3.19f, 72, 4, 400, "E", 300, "B", "2", 1)]
        public void pack1HeatPumpAsPrimStrategyReturnsResultsEEUCalculationResult_true(ApplianceTypes PrimType, float PrimAFUE, float PrimWatt, float PrimCold, float PrimWarm, ApplianceTypes SecType, float secAFUE, float secWatt, float SolArea, float SolEff, int SolNumber, float Container1Vol, string Container1Class,
                                           float Container2Vol, string Container2Class, string TempControlClass, int IdOfSolarContainer)
        {
        CreatePackageSolution(PrimType, PrimAFUE, PrimWatt, PrimCold, PrimWarm, SecType, secAFUE, secWatt, SolArea, SolEff, SolNumber, Container1Vol, Container1Class, Container2Vol, Container2Class, TempControlClass, IdOfSolarContainer);
        Assert.AreEqual(HeatPumpStrategy.CalculateEEI(CurrentPack).GetType(), typeof(EEICalculationResult));
        }

        [Test]
        [TestCase(ApplianceTypes.Heatpump, 133, 43, 139, 136, ApplianceTypes.Boiler, 91, 18, 2.25f, 60, 2, 481, "C", 500, "B", "7", 1, 137)]
        [TestCase(ApplianceTypes.Heatpump, 158, 10, 164, 151, ApplianceTypes.Boiler, 91, 18, 2.25f, 61, 6, 489.3f, "E", 741, "E", "6", 1, 164)]
        [TestCase(ApplianceTypes.Heatpump, 118, 9, 100, 183, ApplianceTypes.Boiler, 94, 42, 3.19f, 72, 4, 400, "E", 300, "B", "2", 1, 122)]
        public void pack1HeatPumpAsPrimStrategyReturnsCorrectEEI_true(ApplianceTypes PrimType, float PrimAFUE, float PrimWatt, float PrimCold, float PrimWarm, ApplianceTypes SecType, float secAFUE, float secWatt, float SolArea, float SolEff, int SolNumber, float Container1Vol, string Container1Class,
                                           float Container2Vol, string Container2Class, string TempControlClass, int IdOfSolarContainer, float expectedValue)
        {
            CreatePackageSolution(PrimType, PrimAFUE, PrimWatt, PrimCold, PrimWarm, SecType, secAFUE, secWatt, SolArea, SolEff, SolNumber, Container1Vol, Container1Class, Container2Vol, Container2Class, TempControlClass, IdOfSolarContainer);
            Assert.AreEqual(HeatPumpStrategy.CalculateEEI(CurrentPack).EEI, expectedValue);
            float EEI = HeatPumpStrategy.CalculateEEI(CurrentPack).EEI;
            Assert.IsTrue(EEI <= expectedValue + 1 && EEI >= expectedValue -1);
        }

        [Test]
        [TestCase(ApplianceTypes.Heatpump, 133, 43, 139, 136, ApplianceTypes.Boiler, 91, 18, 2.25f, 60, 2, 481, "C", 500, "B", "7", 1, 3.5f)]
        [TestCase(ApplianceTypes.Heatpump, 158, 10, 164, 151, ApplianceTypes.Boiler, 91, 18, 2.25f, 61, 6, 489.3f, "E", 741, "E", "6", 1, 4)]
        [TestCase(ApplianceTypes.Heatpump, 118, 9, 100, 183, ApplianceTypes.Boiler, 94, 42, 3.19f, 72, 4, 400, "E", 300, "B", "2", 1, 2)]
        public void pack1HeatPumpAsPrimaryReturnsCorrectRegEff_true(ApplianceTypes PrimType, float PrimAFUE, float PrimWatt, float PrimCold, float PrimWarm, ApplianceTypes SecType, float secAFUE, float secWatt, float SolArea, float SolEff, int SolNumber, float Container1Vol, string Container1Class,
                                           float Container2Vol, string Container2Class, string TempControlClass, int IdOfSolarContainer, float expectedValue)
        {
            CreatePackageSolution(PrimType, PrimAFUE, PrimWatt, PrimCold, PrimWarm, SecType, secAFUE, secWatt, SolArea, SolEff, SolNumber, Container1Vol, Container1Class, Container2Vol, Container2Class, TempControlClass, IdOfSolarContainer);
            float RegulatorEffect = HeatPumpStrategy.CalculateEEI(CurrentPack).EffectOfTemperatureRegulatorClass;
            Assert.IsTrue(RegulatorEffect <= expectedValue + 0.1f && RegulatorEffect >= expectedValue - 0.1f);
        }

        [Test]
        [TestCase(ApplianceTypes.Heatpump, 133, 43, 139, 136, ApplianceTypes.Boiler, 91, 18, 2.25f, 60, 2, 481, "C", 500, "B", "7", 1, 0)]
        [TestCase(ApplianceTypes.Heatpump, 158, 10, 164, 151, ApplianceTypes.Boiler, 91, 18, 2.25f, 61, 6, 489.3f, "E", 741, "E", "6", 1, 6.6f)]
        [TestCase(ApplianceTypes.Heatpump, 118, 9, 100, 183, ApplianceTypes.Boiler, 94, 42, 3.19f, 72, 4, 400, "E", 300, "B", "2", 2, 9.06f)]
        public void pack1HeatPumpAsPrimaryReturnsCorrectSecEff_true(ApplianceTypes PrimType, float PrimAFUE, float PrimWatt, float PrimCold, float PrimWarm, ApplianceTypes SecType, float secAFUE, float secWatt, float SolArea, float SolEff, int SolNumber, float Container1Vol, string Container1Class,
                                           float Container2Vol, string Container2Class, string TempControlClass, int IdOfSolarContainer, float expectedValue)
        {
            CreatePackageSolution(PrimType, PrimAFUE, PrimWatt, PrimCold, PrimWarm, SecType, secAFUE, secWatt, SolArea, SolEff, SolNumber, Container1Vol, Container1Class, Container2Vol, Container2Class, TempControlClass, IdOfSolarContainer);
            float SecondaryEffect = HeatPumpStrategy.CalculateEEI(CurrentPack).EffectOfSecondaryBoiler;
            Assert.IsTrue(SecondaryEffect <= expectedValue + 0.1f && SecondaryEffect >= expectedValue - 0.1f);
        }

        [Test]
        [TestCase(ApplianceTypes.Heatpump, 133, 43, 139, 136, ApplianceTypes.Boiler, 91, 18, 2.25f, 60, 2, 481, "C", 500, "B", "7", 1, 0.65f)]
        [TestCase(ApplianceTypes.Heatpump, 158, 10, 164, 151, ApplianceTypes.Boiler, 91, 18, 2.25f, 61, 6, 489.3f, "E", 741, "E", "6", 1, 8.14f)]
        [TestCase(ApplianceTypes.Heatpump, 118, 9, 100, 183, ApplianceTypes.Boiler, 94, 42, 3.19f, 72, 4, 400, "E", 300, "B", "2", 2, 10.69f)]
        public void pack1HeatPumpAsPrimaryReturnsCorrectSolEff_true(ApplianceTypes PrimType, float PrimAFUE, float PrimWatt, float PrimCold, float PrimWarm, ApplianceTypes SecType, float secAFUE, float secWatt, float SolArea, float SolEff, int SolNumber, float Container1Vol, string Container1Class,
                                           float Container2Vol, string Container2Class, string TempControlClass, int IdOfSolarContainer, float expectedValue)
        {
            CreatePackageSolution(PrimType, PrimAFUE, PrimWatt, PrimCold, PrimWarm, SecType, secAFUE, secWatt, SolArea, SolEff, SolNumber, Container1Vol, Container1Class, Container2Vol, Container2Class, TempControlClass, IdOfSolarContainer);
            float solarContribution = HeatPumpStrategy.CalculateEEI(CurrentPack).SolarHeatContribution;
            Assert.IsTrue(solarContribution <= expectedValue + 0.1f && solarContribution >= expectedValue - 0.1f);
        }

        private void CreatePackageSolution(ApplianceTypes PrimType, float PrimAFUE, float PrimWatt, float PrimCold, float PrimWarm, ApplianceTypes SecType, float secAFUE, float secWatt, float SolArea, float SolEff, int SolNumber, float Container1Vol, string Container1Class,
                                           float Container2Vol, string Container2Class, string TempControlClass, int IdOfSolarContainer)
        {
            CurrentPack = new PackagedSolution()
            {
                PrimaryHeatingUnit = new Appliance()
                {
                    Name = "Compress 5000",
                    Type = PrimType,
                    DataSheet = new HeatingUnitDataSheet() { AFUE = PrimAFUE, WattUsage = PrimWatt, AFUEColdClima = PrimCold, AFUEWarmClima = PrimWarm }
                }
            };

            CurrentPack.Appliances = new ApplianceList(new List<Appliance>() {
                new Appliance()
                {
                    Name = "Logano Plus",
                    Type = SecType,
                    DataSheet = new HeatingUnitDataSheet() { AFUE = secAFUE, WattUsage = secWatt }
                },

                new Appliance()
                {
                    Id = 1,
                    Name = "BST",
                    Type = ApplianceTypes.Container,
                    DataSheet = new ContainerDataSheet() {Volume = Container1Vol, Classification = Container1Class }
                },
                new Appliance()
                {
                    Id = 2,
                    Name = "Logalux",
                    Type = ApplianceTypes.Container,
                    DataSheet = new ContainerDataSheet() {Volume = Container2Vol, Classification = Container2Class}
                },
                new Appliance()
                {
                    Name = "CW400",
                    Type = ApplianceTypes.TemperatureController,
                    DataSheet = new TemperatureControllerDataSheet() {Class = TempControlClass}
                }

            });
            for (int i = 0; i < SolNumber; i++)
            {
                Appliance SolarCollector = new Appliance()
                {
                    Name = "Logasol SKN",
                    Type = ApplianceTypes.SolarPanel,
                    DataSheet = new SolarCollectorDataSheet() { Area = SolArea, Efficency = SolEff }
                };

                CurrentPack.Appliances.Add(SolarCollector);
            }

            CurrentPack.SolarContainer = CurrentPack.Appliances?.FirstOrDefault(solCon => solCon.Type == ApplianceTypes.Container && solCon.Id == IdOfSolarContainer);
        }
    }
}
