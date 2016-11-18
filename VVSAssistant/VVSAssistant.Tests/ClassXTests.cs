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
        // Det er anbefalet at bruge factory i stedet for Setup, 
        // men ved ikke hvordan endnu
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

        // TearDown skal heller ikke bruges 
        [TearDown]
        public void TearDown()
        {

        }
    }
}
