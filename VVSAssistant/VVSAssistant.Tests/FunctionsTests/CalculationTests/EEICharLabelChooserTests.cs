using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VVSAssistant.Functions.Calculation;
using VVSAssistant.Models;
using VVSAssistant.Models.DataSheets;

namespace VVSAssistant.Tests.FunctionsTests.CalculationTests
{
    [TestFixture]
    class EEICharLabelChooserTests
    {
        [Test]
        [TestCase(ApplianceTypes.Boiler, 140,"A++")]
        [TestCase(ApplianceTypes.Boiler, 99, "A+")]
        [TestCase(ApplianceTypes.HeatPump, 35, "E")]
        [TestCase(ApplianceTypes.Boiler, 188, "A+++")]
        [TestCase(ApplianceTypes.Boiler, 87, "B")]
        [TestCase(ApplianceTypes.Boiler, 76, "C")]

        public void LabelChooserRegularReturnscCorrectLabel_true(ApplianceTypes Type, float calcEEI, string expected)
        {
            Assert.AreEqual(EEICharLabelChooser.EEIChar(Type, calcEEI),expected);
        }

        [Test]
        [TestCase(UseProfileType.XL, 103, "A")]
        [TestCase(UseProfileType.XL, 104, "A")]
        [TestCase(UseProfileType.XXL, 10, "G")]
        [TestCase(UseProfileType.L, 67, "B")]
        public void LabelChooserWaterReturnscCorrectLabel_true(UseProfileType Type, float calcEEI, string expected)
        {
            Assert.AreEqual(EEICharLabelChooser.EEIChar(Type, calcEEI), expected);
        }
    }
}
