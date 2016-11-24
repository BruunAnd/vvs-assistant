using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        /*private ClientViewModel _testClient;
        private PackagedSolutionViewModel _testPackSol;
        private ApplianceViewModel _testAppliance1;
        private ApplianceViewModel _testAppliance2;*/

        [SetUp]
        public void Setup()
        {
            //testModel = new CreateOfferViewModel();

            ///* Client Setup */
            //_testClient = new ClientViewModel(new Client());
            //_testClient.ClientInformation.Address = "test";
            //_testClient.ClientInformation.DatabaseName = "test";
            //_testClient.ClientInformation.Email = "test";
            //_testClient.ClientInformation.PhoneNumber = "test";
            //_testClient.Offers = new System.Collections.ObjectModel.ObservableCollection<OfferViewModel>();
            //_testClient.CreationDate = DateTime.Now;
            //_testClient.Id = 1;

            ///* Appliance 1 setup */
            //_testAppliance1 = new ApplianceViewModel(new Appliance());
            //_testAppliance1.DatabaseName = "test";
            //_testAppliance1.Type = ApplianceTypes.Container;

            ///* Appliance 2 setup */
            //_testAppliance2 = new ApplianceViewModel(new Appliance());
            //_testAppliance1.DatabaseName = "test";
            //_testAppliance1.Type = ApplianceTypes.Boiler;

            ///* Packaged solution setup */
            //_testPackSol = new PackagedSolutionViewModel(new PackagedSolution());
            //_testPackSol.Appliances.Add(_testAppliance1);
            //_testPackSol.Appliances.Add(_testAppliance2);
            //_testPackSol.DatabaseName = "test";
        }
        /*
        [Test]
        public void InformationAssignmentTest()
        {
            testModel.Offer.Client.ClientInformation.DatabaseName = "Anders";
            testModel.Offer.PackagedSolution.Appliances.Add(_testAppliance1);

            Assert.AreEqual(testModel.Offer.Client.ClientInformation.DatabaseName, "Anders");
            Assert.AreEqual(testModel.Offer.PackagedSolution.Appliances[0], _testAppliance1);
        }

        [Test]
        public void ObjectAssignmentTest()
        {
            testModel.Offer.Client = _testClient;
            Assert.AreEqual(testModel.Offer.Client.ClientInformation.DatabaseName, "test");
        }

        [Test]
        public void ValueAndObjectAssignment()
        {
            testModel.Offer.Client.ClientInformation.DatabaseName = "Anders";
            testModel.Offer.Client = _testClient;
            Assert.AreEqual(testModel.Offer.Client.ClientInformation.DatabaseName, "test");
        }

        [Test]
        public void InformationValidityTest()
        {
            Assert.IsFalse(testModel.VerifyNeededInformation());

            OfferViewModel offer = new OfferViewModel(new Offer());
            offer.Client = _testClient;
            offer.PackagedSolution = _testPackSol;
            offer.Id = 1;
            offer.Materials.Add(new MaterialViewModel(new Material()));
            offer.Salaries.Add(new SalaryViewModel(new Salary()));

            testModel.Offer = offer;
            Assert.IsTrue(testModel.VerifyNeededInformation());

        }
        */
        /* Mr. Gorbachev, */ [TearDown] /* this wall*/
        public void TearDown()
        {
            testModel = null;
        }
    }
}
