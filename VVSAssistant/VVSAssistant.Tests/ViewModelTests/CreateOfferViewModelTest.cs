using MahApps.Metro.Controls.Dialogs;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VVSAssistant.Common.ViewModels;
using VVSAssistant.Database;
using VVSAssistant.Functions.Calculation;
using VVSAssistant.Models;
using VVSAssistant.Models.DataSheets;
using VVSAssistant.ViewModels;

namespace VVSAssistant.Tests.ViewModelTests
{
    [TestFixture]
    class CreateOfferViewModelTest
    {
        private CreateOfferViewModel testModel;
        private PackagedSolution testPack;
        private PackagedSolution testPack2;
        private Client testClient;
        private Appliance testApp1;
        private Appliance testApp2;
        private Offer off;
        private AssistantContext ctx;

        [SetUp]
        public void Setup()
        {
            testModel = new CreateOfferViewModel(new DialogCoordinator());

            testApp1 = new Appliance() { Name = "app1", CreationDate = DateTime.Now, Type = ApplianceTypes.Boiler};
            testApp1.DataSheet = new HeatingUnitDataSheet();

            testApp2 = new Appliance() { Name = "app2", CreationDate = DateTime.Now, Type = ApplianceTypes.Container};
            testApp2.DataSheet = new ContainerDataSheet();

            testPack = new PackagedSolution() { Name = "testPack"};
            testPack.ApplianceInstances.Add(new ApplianceInstance() {Appliance= testApp1, IsPrimary=true });
            testPack.ApplianceInstances.Add(new ApplianceInstance() {Appliance= testApp2 });
            testPack.CreationDate = DateTime.Now;
            testPack.EnergyLabel = new List<EEICalculationResult>();

            testPack2 = new PackagedSolution() { Name = "testPack2" };
            testPack2.ApplianceInstances.Add(new ApplianceInstance() { Appliance = testApp2, IsPrimary = true });
            testPack2.ApplianceInstances.Add(new ApplianceInstance() { Appliance = testApp1 });
            testPack2.CreationDate = DateTime.Now;
            testPack2.EnergyLabel = new List<EEICalculationResult>();

            testClient = new Client();
            testClient.ClientInformation = new ClientInformation();
            testClient.CreationDate = DateTime.Now;

            off = new Offer();
            off.CreationDate = DateTime.Now;
            off.PackagedSolution = testPack;
            off.Client = testClient;
        }

        [Test]
        public void InitialDBTest()
        {
            new AssistantContext().Database.Delete();
            ctx = new AssistantContext();
            ctx.Offers.Add(off);
            ctx.SaveChanges();
            Assert.IsTrue(ctx.Offers.Any(o => o.Id == off.Id));
        }

        [Test]
        public void LoadExistingOfferTest()
        {
            new AssistantContext().Database.Delete();
            ctx = new AssistantContext();

            ctx.Offers.Add(off);
            ctx.SaveChanges();

            testModel.LoadExistingOffer(off.Id);
            Assert.IsTrue(DoContentsMatch(off.Appliances, testModel.Offer.Appliances));
            Assert.IsTrue(DoContentsMatch(off.Materials, testModel.Offer.Materials));
            Assert.IsTrue(DoContentsMatch(off.Salaries, testModel.Offer.Salaries));
        }

        [Test]
        public void LoadDataFromDatabaseTest()
        {
            new AssistantContext().Database.Delete();
            ctx = new AssistantContext();

            ctx.PackagedSolutions.Add(testPack);
            ctx.SaveChanges();

            Assert.IsTrue(ctx.PackagedSolutions.Any(p => p.Id == testPack.Id));
        }

        [TearDown]
        public void TearDown()
        {
            testModel = null;
        }

        public bool DoContentsMatch(IEnumerable<object> first, IEnumerable<object> second)
        {
            foreach (var item in first)
            {
                if (!second.Contains(item))
                {
                    return false;
                }
            }
            return true;
        }

        [Test]
        public void PackagedSolutionSelectedCmdTest()
        {
            testModel.SelectedPackagedSolution = testPack2;
            testModel.PackagedSolutionSelectedCmd.Execute(null);
            Assert.IsFalse(testModel.IsDataSaved);
            Assert.IsFalse(testModel.ArePackagedSolutionsVisible);
            Assert.True(testModel.IsComponentTabVisible);
            Assert.AreEqual(testPack2, testModel.Offer.PackagedSolution);
            Assert.IsTrue(testModel.AppliancesInOffer.Count == 2);
        }
    }
}
