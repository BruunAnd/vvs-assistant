using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace VVSAssistant.Tests
{
    [TestFixture]
    public class ClassXTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        [Category("Very Important Test Never Delete Mis")]
        //Method_StateUnderTest_ExpectedBehavior
        public void IsAndersFaglord_IstUndFaglord_True()
        {
            //arrange
            bool andersIstUndFaglord = false;
            //act
            andersIstUndFaglord = true;
            //assert
            Assert.IsTrue(andersIstUndFaglord);
        }

        [TearDown]
        public void TearDown()
        {

        }
    }
}
