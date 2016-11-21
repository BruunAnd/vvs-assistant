using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VVSAssistant.Models;
using VVSAssistant.ViewModels;

namespace VVSAssistant.Tests.ViewModelTests
{
    [TestFixture]
    class CreateOfferViewModelTest
    {
        private CreateOfferViewModel testModel;
        private ClientViewModel TestClient;

        [SetUp]
        public void Setup()
        {
            testModel = new CreateOfferViewModelTest();
        }

        [Test]
        public void InformationAssignmentTest()
        {

        }

        [TearDown]
        public void TearDown()
        {
            testModel = null;
        }
    }
}
