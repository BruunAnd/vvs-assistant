using MahApps.Metro.Controls.Dialogs;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VVSAssistant.Models;
using VVSAssistant.Models.DataSheets;
using VVSAssistant.ViewModels;

namespace VVSAssistant.Tests.ViewModelTests
{
    [TestFixture]
    class CreatePackagedSolutionViewModelTest
    {
        CreatePackagedSolutionViewModel model;
        Appliance app1;
        Appliance app2;

        [SetUp]
        public void Setup()
        {
            model = new CreatePackagedSolutionViewModel(new DialogCoordinator());
            app1 = new Appliance() { DataSheet = new HeatingUnitDataSheet()};
            app2 = new Appliance() { DataSheet = new ContainerDataSheet()};
        }
    }
}
