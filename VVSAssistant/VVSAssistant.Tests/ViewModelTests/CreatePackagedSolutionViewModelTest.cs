using MahApps.Metro.Controls.Dialogs;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VVSAssistant.Database;
using VVSAssistant.Models;
using VVSAssistant.Models.DataSheets;
using VVSAssistant.ViewModels;

namespace VVSAssistant.Tests.ViewModelTests
{
    [TestFixture]
    class CreatePackagedSolutionViewModelTest
    {
        AssistantContext ctx;
        CreatePackagedSolutionViewModel model;
        Appliance app1;
        Appliance app2;

        [SetUp]
        public void Setup()
        {
            ctx = new AssistantContext();
            model = new CreatePackagedSolutionViewModel(new DialogCoordinator());
            app1 = new Appliance() {Name="hej", DataSheet = new HeatingUnitDataSheet(), CreationDate = DateTime.Now};
            app2 = new Appliance() {Name="husk navn", DataSheet = new ContainerDataSheet(), CreationDate = DateTime.Now};
        }

        [Test]
        public void LoadDataFromDatabaseTest()
        {
            new AssistantContext().Database.Delete();
            ctx = new AssistantContext();
            ctx.Appliances.Add(app1);
            ctx.Appliances.Add(app2);
            ctx.SaveChanges();

            model.LoadDataFromDatabase();
            Assert.IsTrue(model.Appliances.Count != 0);
        }

        [Test]
        public void LoadExistingPackagedSolutionTest()
        {
            new AssistantContext().Database.Delete();
            ctx = new AssistantContext();
            PackagedSolution pack = new PackagedSolution() { CreationDate = DateTime.Now };
            pack.ApplianceInstances.Add(new ApplianceInstance() {Appliance= app1 });
            pack.ApplianceInstances.Add(new ApplianceInstance() {Appliance= app2 });
            ctx.PackagedSolutions.Add(pack);
            ctx.SaveChanges();
            model.LoadExistingPackagedSolution(pack.Id);
            Assert.IsTrue((model.AppliancesInPackagedSolutionView.Cast<object>().Count() == 2));
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
    }
}
