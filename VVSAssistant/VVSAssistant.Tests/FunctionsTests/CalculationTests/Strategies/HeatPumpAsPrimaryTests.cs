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
        const float SENTINAL = -1;

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
        [TestCase(PackagedSolutionId.PrimaryHeatPump2Solar)]
        [TestCase(PackagedSolutionId.PrimaryHeatPump6Solars)]
        [TestCase(PackagedSolutionId.PrimaryHeatPump4Solars)]
        [TestCase(PackagedSolutionId.PrimaryHeatPumpNoSolars)]
        [TestCase(PackagedSolutionId.PrimaryCHP4Solars)]
        //[TestCase(ApplianceTypes.HeatPump, 133, 43, 139, 136, ApplianceTypes.Boiler, 91, 18, 2.25f, 60, 2, 481, "C", 500, "B", "7", 1)]
        //[TestCase(ApplianceTypes.HeatPump, 158, 10, 164, 151, ApplianceTypes.Boiler, 91, 18, 2.25f, 61, 6, 489.3f, "E", 741, "E", "6", 1)]
        //[TestCase(ApplianceTypes.HeatPump, 118, 9, 100, 183, ApplianceTypes.Boiler, 94, 42, 3.19f, 72, 4, 400, "B", 300, "B", "2", 1)]
        //[TestCase(ApplianceTypes.HeatPump, 158, 10, 164, 151, ApplianceTypes.Boiler, 91, 18, SENTINAL, SENTINAL, 0, 489.3f, "E", SENTINAL, null, "6", -1)]
        //[TestCase(null, SENTINAL, SENTINAL, SENTINAL, SENTINAL, null, SENTINAL, SENTINAL, SENTINAL, SENTINAL, 0 , SENTINAL, null, SENTINAL, null, null, -1)]
        //[TestCase(ApplianceTypes.CHP, 140, 39, SENTINAL, SENTINAL, ApplianceTypes.Boiler, 94, 42, 3.19f, 72, 4, 400, "B", SENTINAL, null, "3", 1)]

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
            Assert.IsTrue(EEI <= expectedValue + 1 && EEI >= expectedValue - 1);           
        }

        //[Test]
        //[TestCase(ApplianceTypes.HeatPump, 133, 43, 139, 136, ApplianceTypes.Boiler, 91, 18, 2.25f, 60, 2, 481, "C", 500, "B", "7", 1, 137)]
        //[TestCase(ApplianceTypes.HeatPump, 158, 10, 164, 151, ApplianceTypes.Boiler, 91, 18, 2.25f, 61, 6, 489.3f, "E", 741, "E", "6", 1, 164)]
        //[TestCase(ApplianceTypes.HeatPump, 118, 9, 100, 183, ApplianceTypes.Boiler, 94, 42, 3.19f, 72, 4, 400, "B", 300, "B", "2", 1, 122)]
        //[TestCase(ApplianceTypes.HeatPump, 158, 10, 164, 151, ApplianceTypes.Boiler, 91, 18, SENTINAL, SENTINAL, 0, 489.3f, "E", SENTINAL, null, "6", -1, 155)]
        //[TestCase(ApplianceTypes.CHP, 140, 39, SENTINAL, SENTINAL, ApplianceTypes.Boiler, 94, 42, 3.19f, 72, 4, 400, "B", SENTINAL, null, "3", 1, 142)]
        //public void pack1HeatPumpAsPrimStrategyReturnsCorrectEEI_true(ApplianceTypes PrimType, float PrimAFUE, float PrimWatt, float PrimCold, float PrimWarm, ApplianceTypes SecType, float secAFUE, float secWatt, float SolArea, float SolEff, int SolNumber, float Container1Vol, string Container1Class,
        //                                   float Container2Vol, string Container2Class, string TempControlClass, int IdOfSolarContainer, float expectedValue)
        //{
        //    CreatePackageSolution(PrimType, PrimAFUE, PrimWatt, PrimCold, PrimWarm, SecType, secAFUE, secWatt, SolArea, SolEff, SolNumber, Container1Vol, Container1Class, Container2Vol, Container2Class, TempControlClass, IdOfSolarContainer);
        //    float EEI = HeatPumpStrategy.CalculateEEI(CurrentPack).EEI;
        //    Assert.IsTrue(EEI <= expectedValue + 1 && EEI >= expectedValue -1);
        //}


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

        //[Test]
        //[TestCase(ApplianceTypes.HeatPump, 133, 43, 139, 136, ApplianceTypes.Boiler, 91, 18, 2.25f, 60, 2, 481, "C", 500, "B", "7", 1, 3.5f)]
        //[TestCase(ApplianceTypes.HeatPump, 158, 10, 164, 151, ApplianceTypes.Boiler, 91, 18, 2.25f, 61, 6, 489.3f, "E", 741, "E", "6", 1, 4)]
        //[TestCase(ApplianceTypes.HeatPump, 118, 9, 100, 183, ApplianceTypes.Boiler, 94, 42, 3.19f, 72, 4, 400, "B", 300, "B", "2", 1, 2)]
        //[TestCase(ApplianceTypes.HeatPump, 158, 10, 164, 151, ApplianceTypes.Boiler, 91, 18, SENTINAL, SENTINAL, 0, 489.3f, "E", SENTINAL, null, "6", -1, 4)]
        //[TestCase(ApplianceTypes.CHP, 140, 39, SENTINAL, SENTINAL, ApplianceTypes.Boiler, 94, 42, 3.19f, 72, 4, 400, "B", SENTINAL, null, "3", 1, 1.5f)]
        //public void pack1HeatPumpAsPrimaryReturnsCorrectRegEff_true(ApplianceTypes PrimType, float PrimAFUE, float PrimWatt, float PrimCold, float PrimWarm, ApplianceTypes SecType, float secAFUE, float secWatt, float SolArea, float SolEff, int SolNumber, float Container1Vol, string Container1Class,
        //                                   float Container2Vol, string Container2Class, string TempControlClass, int IdOfSolarContainer, float expectedValue)
        //{
        //    CreatePackageSolution(PrimType, PrimAFUE, PrimWatt, PrimCold, PrimWarm, SecType, secAFUE, secWatt, SolArea, SolEff, SolNumber, Container1Vol, Container1Class, Container2Vol, Container2Class, TempControlClass, IdOfSolarContainer);
        //    float RegulatorEffect = HeatPumpStrategy.CalculateEEI(CurrentPack).EffectOfTemperatureRegulatorClass;
        //    Assert.IsTrue(RegulatorEffect <= expectedValue + 0.1f && RegulatorEffect >= expectedValue - 0.1f);
        //}

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

        //[Test]
        //[TestCase(ApplianceTypes.HeatPump, 133, 43, 139, 136, ApplianceTypes.Boiler, 91, 18, 2.25f, 60, 2, 481, "C", 500, "B", "7", 1, 0)]
        //[TestCase(ApplianceTypes.HeatPump, 158, 10, 164, 151, ApplianceTypes.Boiler, 91, 18, 2.25f, 61, 6, 489.3f, "E", 741, "E", "6", 1, 6.6f)]
        //[TestCase(ApplianceTypes.HeatPump, 118, 9, 100, 183, ApplianceTypes.Boiler, 94, 42, 3.19f, 72, 4, 400, "B", 300, "B", "2", 2, 9.06f)]
        //[TestCase(ApplianceTypes.HeatPump, 158, 10, 164, 151, ApplianceTypes.Boiler, 91, 18, SENTINAL, SENTINAL, 0, 489.3f, "E", SENTINAL, null, "6", -1, 6.6f)]
        //[TestCase(ApplianceTypes.CHP, 140, 39, SENTINAL, SENTINAL, ApplianceTypes.Boiler, 94, 42, 3.19f, 72, 4, 400, "B", SENTINAL, null, "3", 1, 3.15f)]
        //public void pack1HeatPumpAsPrimaryReturnsCorrectSecEff_true(ApplianceTypes PrimType, float PrimAFUE, float PrimWatt, float PrimCold, float PrimWarm, ApplianceTypes SecType, float secAFUE, float secWatt, float SolArea, float SolEff, int SolNumber, float Container1Vol, string Container1Class,
        //                                   float Container2Vol, string Container2Class, string TempControlClass, int IdOfSolarContainer, float expectedValue)
        //{
        //    CreatePackageSolution(PrimType, PrimAFUE, PrimWatt, PrimCold, PrimWarm, SecType, secAFUE, secWatt, SolArea, SolEff, SolNumber, Container1Vol, Container1Class, Container2Vol, Container2Class, TempControlClass, IdOfSolarContainer);
        //    float SecondaryEffect = HeatPumpStrategy.CalculateEEI(CurrentPack).EffectOfSecondaryBoiler;
        //    //Assert.AreEqual(SecondaryEffect, expectedValue);
        //    Assert.IsTrue(SecondaryEffect <= expectedValue + 0.1f && SecondaryEffect >= expectedValue - 0.1f);
        //}

        [Test]
        [TestCase(ApplianceTypes.HeatPump, 133, 43, 139, 136, ApplianceTypes.Boiler, 91, 18, 2.25f, 60, 2, 481, "C", 500, "B", "7", 1, 0.65f)]
        [TestCase(ApplianceTypes.HeatPump, 158, 10, 164, 151, ApplianceTypes.Boiler, 91, 18, 2.25f, 61, 6, 489.3f, "E", 741, "E", "6", 1, 8.14f)]
        [TestCase(ApplianceTypes.HeatPump, 118, 9, 100, 183, ApplianceTypes.Boiler, 94, 42, 3.19f, 72, 4, 400, "B", 300, "B", "2", 2, 10.69f)]
        [TestCase(ApplianceTypes.CHP, 140, 39, SENTINAL, SENTINAL, ApplianceTypes.Boiler, 94, 42, 3.19f, 72, 4, 400, "B", SENTINAL, null, "3", 1, 3.84f)]
        public void pack1HeatPumpAsPrimaryReturnsCorrectSolEff_true(ApplianceTypes PrimType, float PrimAFUE, float PrimWatt, float PrimCold, float PrimWarm, ApplianceTypes SecType, float secAFUE, float secWatt, float SolArea, float SolEff, int SolNumber, float Container1Vol, string Container1Class,
                                           float Container2Vol, string Container2Class, string TempControlClass, int IdOfSolarContainer, float expectedValue)
        {
            CreatePackageSolution(PrimType, PrimAFUE, PrimWatt, PrimCold, PrimWarm, SecType, secAFUE, secWatt, SolArea, SolEff, SolNumber, Container1Vol, Container1Class, Container2Vol, Container2Class, TempControlClass, IdOfSolarContainer);
            float solarContribution = HeatPumpStrategy.CalculateEEI(CurrentPack).SolarHeatContribution;
            //Assert.AreEqual(solarContribution, expectedValue);
            Assert.IsTrue(solarContribution <= expectedValue + 0.1f && solarContribution >= expectedValue - 0.1f);
        }

        private void CreatePackageSolution(ApplianceTypes PrimType, float PrimAFUE, float PrimWatt, float PrimCold, float PrimWarm, ApplianceTypes SecType, float secAFUE, float secWatt, float SolArea, float SolEff, int SolNumber, float Container1Vol, string Container1Class,
                                           float Container2Vol, string Container2Class, string TempControlClass, int IdOfSolarContainer)
        {


            CurrentPack = new PackagedSolution();

            CurrentPack.PrimaryHeatingUnit = new Appliance()
            {
                Name = "TestPrim",
                Type = PrimType,
                DataSheet = new HeatingUnitDataSheet()
                {
                    AFUE = PrimAFUE,
                    WattUsage = PrimWatt,
                    AFUEColdClima = PrimCold,
                    AFUEWarmClima = PrimWarm
                }
            };

            CurrentPack.Appliances = new ApplianceList(new List<Appliance>());

            if (SecType != 0 && secWatt != SENTINAL && secAFUE != SENTINAL)
            {
                CurrentPack.Appliances.Add(new Appliance()
                {
                    Name = "Logano Plus",
                    Type = SecType,
                    DataSheet = new HeatingUnitDataSheet() { AFUE = secAFUE, WattUsage = secWatt }
                });              
            };

            if (Container1Class != null && Container1Vol != SENTINAL)
            {
                CurrentPack.Appliances.Add(new Appliance()
                {
                    Id = 1,
                    Name = "BST",
                    Type = ApplianceTypes.Container,
                    DataSheet = new ContainerDataSheet() { Volume = Container1Vol, Classification = Container1Class }
                });
            }

            if (Container2Class != null && Container2Vol != SENTINAL)
            {
                CurrentPack.Appliances.Add(new Appliance()
                {
                    Id = 2,
                    Name = "BST",
                    Type = ApplianceTypes.Container,
                    DataSheet = new ContainerDataSheet() { Volume = Container2Vol, Classification = Container2Class }
                });
            }

            if (TempControlClass != null)
            {
                CurrentPack.Appliances.Add(new Appliance()
                {
                    Name = "CW400",
                    Type = ApplianceTypes.TemperatureController,
                    DataSheet = new TemperatureControllerDataSheet() { Class = TempControlClass }
                });
            }

            if (SolArea != SENTINAL && SolEff != SENTINAL && IdOfSolarContainer != -1)
            {
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

                CurrentPack.SolarContainers.Add(CurrentPack.Appliances?.FirstOrDefault(solCon => solCon.Type == ApplianceTypes.Container && solCon.Id == IdOfSolarContainer));
            }
   
            






































            //    CurrentPack = new PackagedSolution()
            //    {
            //        PrimaryHeatingUnit = new Appliance()
            //        {
            //            DatabaseName = "Compress 5000",
            //            Type = PrimType,
            //            DataSheet = new HeatingUnitDataSheet() { AFUE = PrimAFUE, WattUsage = PrimWatt, AFUEColdClima = PrimCold, AFUEWarmClima = PrimWarm }
            //        }
            //    };

            //    CurrentPack.Appliances = new ApplianceList(new List<Appliance>() {
            //        new Appliance()
            //        {
            //            DatabaseName = "Logano Plus",
            //            Type = SecType,
            //            DataSheet = new HeatingUnitDataSheet() { AFUE = secAFUE, WattUsage = secWatt }
            //        },

            //        new Appliance()
            //        {
            //            Id = 1,
            //            DatabaseName = "BST",
            //            Type = ApplianceTypes.Container,
            //            DataSheet = new ContainerDataSheet() {Volume = Container1Vol, Classification = Container1Class }
            //        },
            //        new Appliance()
            //        {
            //            Id = 2,
            //            DatabaseName = "Logalux",
            //            Type = ApplianceTypes.Container,
            //            DataSheet = new ContainerDataSheet() {Volume = Container2Vol, Classification = Container2Class}
            //        },
            //        new Appliance()
            //        {
            //            DatabaseName = "CW400",
            //            Type = ApplianceTypes.TemperatureController,
            //            DataSheet = new TemperatureControllerDataSheet() {Class = TempControlClass}
            //        }

            //    });
            //    for (int i = 0; i < SolNumber; i++)
            //    {
            //        Appliance SolarCollector = new Appliance()
            //        {
            //            DatabaseName = "Logasol SKN",
            //            Type = ApplianceTypes.SolarPanel,
            //            DataSheet = new SolarCollectorDataSheet() { Area = SolArea, Efficency = SolEff }
            //        };

            //        CurrentPack.Appliances.Add(SolarCollector);
            //    }

            //    CurrentPack.SolarContainer = CurrentPack.Appliances?.FirstOrDefault(solCon => solCon.Type == ApplianceTypes.Container && solCon.Id == IdOfSolarContainer);
            }
        }
    }
