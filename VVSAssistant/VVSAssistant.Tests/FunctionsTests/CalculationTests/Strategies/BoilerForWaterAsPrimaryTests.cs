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
        public void CalculateEEI_CalculatesTheCorrectResult_FirstOffer()
        {
            
        }
        [Test]
        [TestCase(15, 2, 48)]
        [TestCase(10,5, 64)]
        [TestCase(35, 10, 158)]
        public void SolCalMethodQaux(int pumpConsumption, int standbyConsumption, float expected)
        {
            var package = new PackagedSolution() { PrimaryHeatingUnit = new Appliance() { DataSheet = new WaterHeatingUnitDataSheet()} };
            var data = package.PrimaryHeatingUnit.DataSheet as WaterHeatingUnitDataSheet;
            data.SolPumpConsumption = pumpConsumption;
            data.SolStandbyConsumption = standbyConsumption;

            var calculation = new BoilerForWater();
            var result = calculation.SolCalMethodQaux(package.PrimaryHeatingUnit.DataSheet as WaterHeatingUnitDataSheet);

            Assert.AreEqual(result, expected);
        }
    }
}