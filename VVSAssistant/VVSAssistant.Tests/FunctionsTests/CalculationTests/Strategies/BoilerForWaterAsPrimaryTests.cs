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
        [TestCase(15, 2, 48)]
        [TestCase(10,5, 64)]
        [TestCase(35, 10, 158)]
        [TestCase(45, 2.72f, 114)]
        [TestCase(70, 2.72f, 164)]
        [TestCase(35, 2.72f, 94)]
        public void SolCalMethodQaux(float pumpConsumption, float standbyConsumption, float expected)
        {
            var package = new PackagedSolution() { PrimaryHeatingUnit = new Appliance() { DataSheet = new WaterHeatingUnitDataSheet()} };
            var data = package.PrimaryHeatingUnit.DataSheet as WaterHeatingUnitDataSheet;
            data.SolPumpConsumption = pumpConsumption;
            data.SolStandbyConsumption = standbyConsumption;

            var calculation = new BoilerForWater();
            var result = calculation.SolCalMethodQaux(package.PrimaryHeatingUnit.DataSheet as WaterHeatingUnitDataSheet);

            Assert.AreEqual(result, expected);
        }
        [Test]
        // Skal være 3075 for SolarContribution bliver rigtig
        [TestCase(2,3074)]
        [TestCase(3, 3099)]
        [TestCase(4,3074)]
        public void SolcalMethodQnonsol_CalculatesQnonsol(int packageId, float expected)
        {
            var package = new PackagedSolutionFactory().GetPackagedSolution(packageId);
            var data = package.PrimaryHeatingUnit.DataSheet as WaterHeatingUnitDataSheet;
            var calculation = new BoilerForWater();
            float QnonsolTest = calculation.SolCalMethodQnonsol(data);
            
            Assert.AreEqual(expected, Math.Round(QnonsolTest));
        }

        [Test]
        [TestCase(2,82)]
        [TestCase(4,82)]
        public void WaterPrimaryCalculateEEI_CorrectEnergiEfficiency(int packageId, float expected)
        {
            var package = new PackagedSolutionFactory().GetPackagedSolution(packageId);
            var data = package.PrimaryHeatingUnit.DataSheet as WaterHeatingUnitDataSheet;
            var calculation = new BoilerForWater();
            var result = calculation.CalculateEEI(package);
            var EEI = result.WaterHeatingEffciency;
            Assert.IsTrue(expected + 0.1f >= EEI && EEI >= expected - 0.1f);
        }
        [Test]
        [TestCase(2,17.65f)]
        [TestCase(4, 21.82f)]
        public void WaterPrimaryCalculateEEI_CorrectSolarContribution(int packageId, float expected)
        {
            var package = new PackagedSolutionFactory().GetPackagedSolution(packageId);
            var data = package.PrimaryHeatingUnit.DataSheet as WaterHeatingUnitDataSheet;
            var calculation = new BoilerForWater();
            var result = calculation.CalculateEEI(package);
            var EEI = Math.Round(result.SolarHeatContribution, 2);
            
            Assert.IsTrue(expected + 0.1f >= EEI && EEI >= expected - 0.1f);
        }
        [Test]
        [TestCase(2,100)]
        [TestCase(4, 104)]
        public void WaterPrimaryCalculateEEI_CalculatesEEICompletePackagedSolution(int packageId, float expected)
        {
            var package = new PackagedSolutionFactory().GetPackagedSolution(packageId);
            var data = package.PrimaryHeatingUnit.DataSheet as WaterHeatingUnitDataSheet;
            var calculation = new BoilerForWater();
            var result = calculation.CalculateEEI(package);
            var EEI = (float)Math.Round(result.EEI);
            Assert.IsTrue(expected+1f >= EEI && EEI >= expected-1f);
        }
        [Test]
        [TestCase(2,107)]
        [TestCase(4,113)]
        public void WaterPrimaryCalculateEEI_CorrectWarmerEEI(int packageId, float expected)
        {
            var package = new PackagedSolutionFactory().GetPackagedSolution(packageId);
            var data = package.PrimaryHeatingUnit.DataSheet as WaterHeatingUnitDataSheet;
            var calculation = new BoilerForWater();
            var result = calculation.CalculateEEI(package);
            var EEI = (float)Math.Round(result.PackagedSolutionAtWarmTemperaturesAFUE);
            Assert.IsTrue(expected + 1f >= EEI && EEI >= expected - 1f);
        }
        [Test]
        [TestCase(2,96)]
        [TestCase(4, 99)]
        public void WaterPrimaryCalculateEEI_CorrectColderEEI(int packageId, float expected)
        {
            var package = new PackagedSolutionFactory().GetPackagedSolution(packageId);
            var data = package.PrimaryHeatingUnit.DataSheet as WaterHeatingUnitDataSheet;
            var calculation = new BoilerForWater();
            var result = calculation.CalculateEEI(package);
            var EEI = (float)Math.Round(result.PackagedSolutionAtColdTemperaturesAFUE);
            Assert.IsTrue(expected + 1f >= EEI && EEI >= expected - 1f);
        }
        #region Factories
    }
    class DataSheetFactory
    {
        public WaterHeatingUnitDataSheet GetDataSheet(int id)
        {
            DataSheet datasheet;
            switch(id)
            {
                case 1:
                    datasheet = new DataSheetStub(1.94f, 0.761f, 3.08f, 0.012f, 0.94f, 190.3f, 
                                             20f, 64f, 45f, 2.72f, UseProfileType.XL, 95f);
                    break;
                case 2:
                    datasheet = new DataSheetStub(2.25f, 0.766f, 3.22f, 0.015f, 0.92f, 500f,
                                             0f, 80f, 70, 2.72f, UseProfileType.XL, 82f);
                    break;
                case 3:
                    datasheet = new DataSheetStub(2.25f, 0.766f, 3.22f, 0.015f, 0.92f, 500f,
                                             20f, 80f, 70, 2.72f, UseProfileType.XL, 82f);
                    break;
                case 4:
                    datasheet = new DataSheetStub(2.25f, 0.766f, 3.22f, 0.015f, 0.92f, 500f,
                                             0f, 80f, 35, 2.72f, UseProfileType.XL, 82f);
                    break;
                case 5:
                    datasheet = new DataSheetStub(2.25f, 0.766f, 3.22f, 0.015f, 0.92f, 500f,
                                             34f, 80f, 30, 2.72f, UseProfileType.XL, 85f);
                    break;
                case 6:
                    datasheet = new DataSheetStub(2.25f, 0.766f, 3.22f, 0.015f, 0.92f, 500f,
                                             0f, 80f, 30, 2.72f, UseProfileType.XL, 82f);
                    break;
                default:
                    return null;
            }
            return datasheet as WaterHeatingUnitDataSheet;
        }
    }
    class PackagedSolutionFactory
    {
        public PackagedSolution GetPackagedSolution(int id)
        {
            switch (id)
            {
                case 1:
                    return new PackagedSolutionStub(1);
                case 2:
                    return new PackagedSolutionStub(2);
                case 3:
                    return new PackagedSolutionStub(3);
                case 4:
                    return new PackagedSolutionStub(4);
                case 5:
                    return new PackagedSolutionStub(6);
                default:
                    return null;
            }
        }
    }
    class PackagedSolutionStub : PackagedSolution
    {
        public PackagedSolutionStub(int datasheetId)
        {
            PrimaryHeatingUnit = new Appliance
            { DataSheet = new DataSheetFactory().GetDataSheet(datasheetId) };
        }
    }
    class DataSheetStub : WaterHeatingUnitDataSheet
    {
        public DataSheetStub(float Asol, float N0, float a1, float a2, float IAM, 
                float Vnorm, float Vbu, float stdLoss, float solPump, float solsb, 
                UseProfileType type, float efficiency)
        {
            this.Asol = Asol; this.N0 = N0; this.a1 = a1;
            this.a2 = a2; this.IAM = IAM; this.Volume = Vnorm;
            this.Vbu = Vbu; this.StandingLoss = stdLoss;
            this.SolPumpConsumption = solPump;
            this.SolStandbyConsumption = solsb;
            this.UseProfile = type;
            this.WaterHeatingEffiency = efficiency;
        }
    }
    #endregion
}