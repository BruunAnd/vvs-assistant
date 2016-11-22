using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VVSAssistant.Functions.Calculation;

namespace VVSAssistant.Tests.FunctionsTests.CalculationTests
{
    [TestFixture]
    class UtilityClassTests
    {
        [Test]
        [TestCase(0.0f,true,true)]
        [TestCase(0.0f, true, false)]
        [TestCase(0.0f, false, true)]
        [TestCase(0.0f, false, false)]
        [TestCase(0.4f, true, true)]
        [TestCase(0.35f, true, true)]
        [TestCase(0.7f, true, true)]
        [TestCase(0.8f, true, true)]
        [TestCase(0.199999f, true, true)]
        [TestCase(5.0f, true, true)]
        [TestCase(0.5f, true, true)]
        public void GetWeightingReturnsFloat_True(float input, bool firstBool, bool secondBool)
        {
            Assert.AreEqual(UtilityClass.GetWeighting(input, firstBool, secondBool).GetType(),typeof(float));
        }
        
        [Test]
        [TestCase(0.59f, false, true, 0.02f)]
        [TestCase(0.59f, true, true, 0.0f)]
        [TestCase(0.59f, false, false, 0.98f)]
        [TestCase(0.59f, true, false, 1.0f)]
        [TestCase(0.0735f, false, true, 0.78f)]
        [TestCase(0.7f, false, true, 0.00f)]

        public void GetWeightingReturnsCurrectAnswer_True(float input, bool firstBool, bool secondBool, float expectedAnswer)
        {
            Assert.AreEqual(UtilityClass.GetWeighting(input, firstBool, secondBool), expectedAnswer);
        }

    }
}
