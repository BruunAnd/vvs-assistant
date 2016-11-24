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
        public void PrimaryBoiler_PrimaryPackageNull_ReturnsNull()
        {
            var package = new PackageFactory().GetPackage(2);
            package.PrimaryHeatingUnit = null;
            var calculation = new BoilerAsPrimary();
            var result = calculation.CalculateEEI(package);
            Assert.AreEqual(result, null);
        }
        [Test]
        [TestCase(2,91)]
        public void CalculateEEI_CorrectAFUEContribution(int packageId, float expected)
        {
            var package = new PackageFactory().GetPackage(packageId);
            var calculation = new BoilerAsPrimary();
            var result = new EEICalculationResult();
            result = calculation.CalculateEEI(package);
            var AFUE = result.PrimaryHeatingUnitAFUE;
            
            Assert.IsTrue(expected <= AFUE+0.1f && expected <= AFUE+0.1f);
        }
        [Test]
        [TestCase(2,3)]
        public void CalculateEEI_CorrectTempContribution(int packageid, float expected)
        {
            var package = new PackageFactory().GetPackage(packageid);
            var calculation = new BoilerAsPrimary();
            var result = new EEICalculationResult();
            result = calculation.CalculateEEI(package);
            Assert.AreEqual(expected, (float)Math.Round(result.EffectOfTemperatureRegulatorClass,1));
        }
        [Test]
        [TestCase(2, 1, 0.2f)]
        public void CalculateEEI_CorrectSecondBoilerContribution(int packageId, int boilerId, float expected)
        {
            var package = new PackageFactory().GetPackage(packageId);
            var calculation = new BoilerAsPrimary();
            var result = new EEICalculationResult();
            result = calculation.CalculateEEI(package);
            var secBoiler = result.EffectOfSecondaryBoiler;

            Assert.IsTrue(expected <= secBoiler + 0.1f && expected <= secBoiler + 0.1f);
        }
        [Test]
        [TestCase(2,1.69f)]
        public void CalculateEEI_CorrectSolarContribution(int packageId, float expected)
        {
            var package = new PackageFactory().GetPackage(packageId);
            var calculation = new BoilerAsPrimary();
            var result = new EEICalculationResult();
            result = calculation.CalculateEEI(package);
            var solar = result.SolarHeatContribution;

            Assert.IsTrue(expected <= solar + 0.1f && expected <= solar + 0.1f);
        }
        [Test]
        [TestCase(2,0)]
        public void CalculateEEI_CorrectHeatpumpContribution(int packageId,float expected)
        {
            var package = new PackageFactory().GetPackage(packageId);
            var calculation = new BoilerAsPrimary();
            var result = new EEICalculationResult();
            result = calculation.CalculateEEI(package);
            Assert.AreEqual(expected, (float)Math.Round(result.EffectOfSecondaryHeatPump, 2));
        }
        [Test]
        [TestCase(2,0)]
        public void CalculateEEI_CorrectAdjustedContribution(int packageId, float expected)
        {
            var package = new PackageFactory().GetPackage(packageId);
            var calculation = new BoilerAsPrimary();
            var result = new EEICalculationResult();
            result = calculation.CalculateEEI(package);
            var adjusted = result.AdjustedContribution;

            Assert.IsTrue(expected <= adjusted + 0.1f && expected <= adjusted + 0.1f);
        }
        [Test]
        [TestCase(1,BoilerId.Condens5000, 95f)]
        public void CalculateEEI_CorrecrOverallResult(int packageId, BoilerId id,float expected)
        {
            var package = new PackageFactory().GetPackage(packageId);
            var calculation = new BoilerAsPrimary();
            package.Appliances.Add(new ApplianceFactory().GetBoiler(id) ?? new Appliance());
            var result = new EEICalculationResult();
            result = calculation.CalculateEEI(package);
            var EEI = Math.Round(result.EEI);

            Assert.IsTrue(expected <= EEI + 1f && expected <= EEI + 1f);
        }
        [Test]
        [TestCase(2,0)]
        public void CalculateEEI_CorrectLowTempEEI(int packageId, float expected)
        {
            var package = new PackageFactory().GetPackage(packageId);
            var calculation = new BoilerAsPrimary();
            var result = calculation.CalculateEEI(package);
            var lowTemp = result.PackagedSolutionAtColdTemperaturesAFUE;

            Assert.IsTrue(expected <= lowTemp + 0.1f && expected <= lowTemp + 0.1f);
        }
    }
    #region Factories
    class PackageFactory
    {
        public PackagedSolution GetPackage(int id)
        {
            switch (id)
            {
                case 1:
                    return new PackageStub();
                case 2:
                    return new PackageStub(BoilerId.LoganoSB150, ContainerId.ClassBHighVolume, BoilerId.Cerapur, SolarPanelId.LogasolSKN, 
                                           0,ContainerId.ClassBHighVolume, TempControlId.FB100);
                default:
                    return null;
            }
        }
    }
    class ApplianceFactory
    {
        public Appliance GetBoiler(BoilerId id)
        {
            switch (id)
            {
                case BoilerId.Cerapur:
                    return new ApplianceStub("Cerapur", new HeatingUnitDataSheet()
                    { WattUsage = 20, AFUE = 93, AFUEColdClima = 98.2f, AFUEWarmClima = 87.8f },
                    ApplianceTypes.Boiler);
                case BoilerId.Condens5000:
                    return null;
                case BoilerId.LoganoSB150:
                    return new ApplianceStub("LoganoPlusSB105", new HeatingUnitDataSheet()
                    { AFUE = 91, WattUsage = 18, AFUEColdClima = 99.2f, AFUEWarmClima = 92.5f },
                    ApplianceTypes.Heatpump);
                default:
                    return null;
            }
        }
        public Appliance GetHeatpump(HeatpumpId id)
        {
            switch (id)
            {
                default:
                    return null;
            }
        }
        public Appliance GetSolarPanel(SolarPanelId id)
        {
            switch (id)
            {
                case SolarPanelId.LogasolSKN:
                    return new ApplianceStub("LogasolSKN", new SolarCollectorDataSheet()
                    { Area = 2.25f, Efficency = 60 }, ApplianceTypes.SolarPanel);
                default:
                    return null;
            }
        }
        public Appliance GetContainer(ContainerId id)
        {
            switch (id)
            {
                case ContainerId.ClassBHighVolume:
                    return new ApplianceStub("SomeContiner", new ContainerDataSheet()
                    { Volume = 500, Classification = "B" }, ApplianceTypes.Container);
                default:
                    return null;
            }
        }
        public Appliance GetTempControl(TempControlId id)
        {
            switch (id)
            {
                case TempControlId.FB100:
                    return new ApplianceStub("FB100", new TemperatureControllerDataSheet()
                    { Class = "5" }, ApplianceTypes.TemperatureController);
                default:
                    return null;
            }
        }
    }
    #region Id's
    public enum ContainerId
    { ClassBHighVolume = 1}
    public enum HeatpumpId
    { }
    public enum SolarPanelId
    { LogasolSKN = 1 }
    public enum TempControlId
    { FB100 = 1}
    public enum BoilerId
    { Cerapur = 1, Condens5000, LoganoSB150 }
    #endregion
    class ApplianceStub : Appliance
    {
        public ApplianceStub(string name, HeatingUnitDataSheet datasheet, ApplianceTypes type)
        {
            Name = name; DataSheet = datasheet; Type = type;
        }
        public ApplianceStub(string name, ContainerDataSheet datasheet, ApplianceTypes type)
        {
            Name = name; DataSheet = datasheet; Type = type;
        }
        public ApplianceStub(string name, SolarCollectorDataSheet datasheet, ApplianceTypes type)
        {
            Name = name; DataSheet = datasheet; Type = type;
        }
        public ApplianceStub(string name, TemperatureControllerDataSheet datasheet, ApplianceTypes type)
        {
            Name = name; DataSheet = datasheet; Type = type;
        }
    }
    class PackageStub : PackagedSolution
    {
        public int Id { get; set; }
        public PackageStub(BoilerId priBoiler, ContainerId solarContain, BoilerId? secBoiler, SolarPanelId? solar, 
                           HeatpumpId? heatpump, ContainerId? container, TempControlId? tempControl)
        {
            var factory = new ApplianceFactory();
            PrimaryHeatingUnit = factory.GetBoiler((priBoiler));
            SolarContainer = factory.GetContainer((solarContain));
            Appliances.Add(factory.GetBoiler(secBoiler == null ? 0 : secBoiler.Value));
            Appliances.Add(factory.GetSolarPanel(solar == null ? 0 : solar.Value));
            Appliances.Add(factory.GetHeatpump(heatpump.Value) ?? new Appliance());
            Appliances.Add(factory.GetContainer(container == null ? 0 : container.Value));
            Appliances.Add(factory.GetTempControl(tempControl == null ? 0 : tempControl.Value));
        }
        public PackageStub()
        {
            Id = 1;
            PrimaryHeatingUnit = new Appliance()
            {
                Name = "LoganoPlusSB105",
                DataSheet = new HeatingUnitDataSheet()
                { AFUE = 91, WattUsage = 18, AFUEColdClima = 99.2f, AFUEWarmClima = 92.5f },
                Type = ApplianceTypes.Boiler,
                Id = 1
            };
            SolarContainer = new Appliance()
            {
                Name = "Some Container",
                Type = ApplianceTypes.Container,
                DataSheet = new ContainerDataSheet()
                { Volume = 500, Classification = "B" }
            };
            Appliances = new ApplianceList(new List<Appliance>()
            {
                new Appliance()
                {
                    Name = "Logasol SKN",
                    DataSheet = new SolarCollectorDataSheet()
                    { Area = 2.25f, Efficency=60},
                    Type = ApplianceTypes.SolarPanel
                },
                new Appliance()
                {
                    Name = "Some Container",
                    Type = ApplianceTypes.Container,
                    DataSheet = new ContainerDataSheet()
                    {Volume=500, Classification="B" }
                },
                new Appliance()
                {
                    Name = "Cerapur",
                    DataSheet = new HeatingUnitDataSheet()
                    { WattUsage=20, AFUE=93, AFUEColdClima=98.2f, AFUEWarmClima=87.8f},
                    Type = ApplianceTypes.Boiler
                },
                new Appliance()
                {
                    Name = "FB 100",
                    DataSheet = new TemperatureControllerDataSheet()
                    { Class = "5"},
                    Type = ApplianceTypes.TemperatureController
                }
            });
        }
    }
    #endregion
}
