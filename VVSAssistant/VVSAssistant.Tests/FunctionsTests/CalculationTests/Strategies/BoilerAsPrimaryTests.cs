﻿using NUnit.Framework;
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
        public void PrimaryBoilerCalculateEEI_PrimaryPackageNull_ReturnsNull()
        {
            var package = new PackageFactory().GetPackage(PackagedSolutionId.PrimaryBoilerSame);
            package.PrimaryHeatingUnit = null;
            var calculation = new BoilerAsPrimary();
            var result = calculation.CalculateEEI(package);
            Assert.AreEqual(result, null);
        }
        [Test]
        public void PrimaryBoilerCalculateEEI_PrimarySolarContainerNull_ReturnsNull()
        {
            var package = new PackageFactory().GetPackage(PackagedSolutionId.PrimaryBoilerSame);
            package.SolarContainer = null;
            var calculation = new BoilerAsPrimary();
            var result = calculation.CalculateEEI(package);
            Assert.AreEqual(result.SolarHeatContribution, 0);
        }
        [Test]
        public void PrimaryBoilerCalculateEEI_AllAboutThemNulls()
        {
            var package = new PackageFactory().GetPackage(PackagedSolutionId.PrimaryBoilerNulls);
            var calculation = new BoilerAsPrimary();
            var result = calculation.CalculateEEI(package);
            Assert.AreEqual(result.EEI, result.PrimaryHeatingUnitAFUE);
        }
        [Test]
        [TestCase(PackagedSolutionId.PrimaryBoilerOHeatPump,91)]
        public void PrimaryBoilerCalculateEEI_CorrectAFUEContribution(PackagedSolutionId id, float expected)
        {
            var package = new PackageFactory().GetPackage(id);
            var calculation = new BoilerAsPrimary();
            var result = new EEICalculationResult();
            result = calculation.CalculateEEI(package);
            var AFUE = result.PrimaryHeatingUnitAFUE;
            
            Assert.IsTrue(expected <= AFUE+0.1f && expected <= AFUE+0.1f);
        }
        [Test]
        [TestCase(PackagedSolutionId.PrimaryBoilerOHeatPump,3)]
        [TestCase(PackagedSolutionId.PrimaryBoilerWHeatPump, 3)]
        public void PrimaryBoilerCalculateEEI_CorrectTempContribution(PackagedSolutionId id, float expected)
        {
            var package = new PackageFactory().GetPackage(id);
            var calculation = new BoilerAsPrimary();
            var result = new EEICalculationResult();
            result = calculation.CalculateEEI(package);
            Assert.AreEqual(expected, (float)Math.Round(result.EffectOfTemperatureRegulatorClass,1));
        }
        [Test]
        [TestCase(PackagedSolutionId.PrimaryBoilerOHeatPump, 1, 0.2f)]
        [TestCase(PackagedSolutionId.PrimaryBoilerWHeatPump, 1, 0.2f)]
        public void PrimaryBoilerCalculateEEI_CorrectSecondBoilerContribution(PackagedSolutionId id, int boilerId, float expected)
        {
            var package = new PackageFactory().GetPackage(id);
            var calculation = new BoilerAsPrimary();
            var result = new EEICalculationResult();
            result = calculation.CalculateEEI(package);
            var secBoiler = result.EffectOfSecondaryBoiler;

            Assert.IsTrue(expected <= secBoiler + 0.1f && expected <= secBoiler + 0.1f);
        }
        [Test]
        [TestCase(PackagedSolutionId.PrimaryBoilerOHeatPump,1.69f)]
        [TestCase(PackagedSolutionId.PrimaryBoilerWHeatPump, 1.69f)]
        public void PrimaryBoilerCalculateEEI_CorrectSolarContribution(PackagedSolutionId id, float expected)
        {
            var package = new PackageFactory().GetPackage(id);
            var calculation = new BoilerAsPrimary();
            var result = new EEICalculationResult();
            result = calculation.CalculateEEI(package);
            var solar = result.SolarHeatContribution;

            Assert.IsTrue(expected <= solar + 0.1f && expected <= solar + 0.1f);
        }
        [Test]
        [TestCase(PackagedSolutionId.PrimaryBoilerOHeatPump,0)]
        public void PrimaryBoilerCalculateEEI_CorrectHeatpumpContribution(PackagedSolutionId id,float expected)
        {
            var package = new PackageFactory().GetPackage(id);
            var calculation = new BoilerAsPrimary();
            var result = new EEICalculationResult();
            result = calculation.CalculateEEI(package);
            Assert.AreEqual(expected, (float)Math.Round(result.EffectOfSecondaryHeatPump, 2));
        }
        [Test]
        [TestCase(PackagedSolutionId.PrimaryBoilerOHeatPump,0)]
        public void PrimaryBoilerCalculateEEI_CorrectAdjustedContribution(PackagedSolutionId id, float expected)
        {
            var package = new PackageFactory().GetPackage(id);
            var calculation = new BoilerAsPrimary();
            var result = new EEICalculationResult();
            result = calculation.CalculateEEI(package);
            var adjusted = result.AdjustedContribution;

            Assert.IsTrue(expected <= adjusted + 0.1f && expected <= adjusted + 0.1f);
        }
        [Test]
        [TestCase(PackagedSolutionId.PrimaryBoilerOHeatPump,BoilerId.Condens5000, 95f)]
        public void PrimaryBoilerCalculateEEI_CorrecrOverallResult(PackagedSolutionId packId, BoilerId id,float expected)
        {
            var package = new PackageFactory().GetPackage(packId);
            var calculation = new BoilerAsPrimary();
            package.Appliances.Add(new ApplianceFactory().GetBoiler(id) ?? new Appliance());
            var result = new EEICalculationResult();
            result = calculation.CalculateEEI(package);
            var EEI = Math.Round(result.EEI);

            Assert.IsTrue(expected <= EEI + 1f && expected <= EEI + 1f);
        }
        [Test]
        [TestCase(PackagedSolutionId.PrimaryBoilerOHeatPump,0)]
        public void PrimaryBoilerCalculateEEI_CorrectLowTempEEI(PackagedSolutionId id, float expected)
        {
            var package = new PackageFactory().GetPackage(id);
            var calculation = new BoilerAsPrimary();
            var result = calculation.CalculateEEI(package);
            var lowTemp = result.PackagedSolutionAtColdTemperaturesAFUE;

            Assert.IsTrue(expected <= lowTemp + 0.1f && expected <= lowTemp + 0.1f);
        }
    }
}
