using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using VVSAssistant.Database;
using VVSAssistant.Models;
using VVSAssistant.Models.DataSheets;

namespace VVSAssistant
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            //return;
            ShittySeed();
        }

        private void ShittySeed()
        {
            new AssistantContext().Database.Delete();
            // Test database stuff
            using (var db = new AssistantContext())
            {
                // add a new client 'Iaro'
                var clientInformation = new ClientInformation { Name = "Iaroslav", Address = "Kvadratet", Email = "iaro@russia.ru", PhoneNumber = "88888888" };
                var client = new Client { CreationDate = DateTime.Now, ClientInformation = clientInformation };
                // offer example
                var offer = new Offer
                {
                    Client = client,
                    CreationDate = DateTime.Now
                };
                var offerInformation = new OfferInformation()
                {
                    Intro = "Example intro",
                    Title = "Example Title"
                };
                offer.OfferInformation = offerInformation;
                var packagedSolution = new PackagedSolution()
                {
                    CreationDate = DateTime.Now,
                    Name = "Example Solution"
                };

                db.PackagedSolutions.Add(packagedSolution);

                //First appliance
                var appliance = new Appliance()
                {
                    CreationDate = DateTime.Now,
                    Name = "Vultorp XP",
                    Type = ApplianceTypes.Container
                };
                var dataSheet = new ContainerDataSheet()
                {
                    Price = 150
                };
                appliance.DataSheet = dataSheet;
                var unitPrice = new UnitPrice()
                {
                    UnitCostPrice = 150,
                    Quantity = 1
                };
                appliance.UnitPrice = unitPrice;

                //Second appliance
                var appliance2 = new Appliance()
                {
                    CreationDate = DateTime.Now,
                    Name = "MegaBoiler2000",
                    Type = ApplianceTypes.Boiler
                };
                var dataSheet2 = new HeatingUnitDataSheet()
                {
                    Price = 120
                };
                appliance2.DataSheet = dataSheet2;
                var unitPrice2 = new UnitPrice()
                {                    
                    UnitCostPrice = 120,
                    Quantity = 1
                };
                appliance2.UnitPrice = unitPrice2;

                //Third appliance
                var appliance3 = new Appliance()
                {
                    CreationDate = DateTime.Now,
                    Name = "DatSolar",
                    Type = ApplianceTypes.SolarPanel
                };
                var heatPumpDataSheet3 = new SolarCollectorDataSheet()
                {
                    Price = 120
                };
                appliance3.DataSheet = heatPumpDataSheet3;
                var unitPrice3 = new UnitPrice()
                {
                    UnitCostPrice = 120,
                    Quantity = 1
                };
                appliance3.UnitPrice = unitPrice3;
                db.Appliances.Add(appliance3);

                //Fourth appliance
                var appliance4 = new Appliance()
                {
                    CreationDate = DateTime.Now,
                    Name = "DatTempController",
                    Type = ApplianceTypes.TemperatureController
                };
                var heatPumpDataSheet4 = new TemperatureControllerDataSheet()
                {
                    Price = 120
                };
                appliance4.DataSheet = heatPumpDataSheet4;
                var unitPrice4 = new UnitPrice()
                {
                    UnitCostPrice = 120,
                    Quantity = 1
                };
                appliance4.UnitPrice = unitPrice4;
                db.Appliances.Add(appliance4);

                packagedSolution.Appliances.Add(appliance);
                packagedSolution.Appliances.Add(appliance2);

                var material = new Material()
                {
                    UnitCostPrice = 150,
                    UnitSalesPrice = 180,
                    Reference = new MaterialReference()
                    {
                        Name = "Rør"
                    }
                };

                var salary = new Salary()
                {
                    UnitSalesPrice = 180,
                    UnitCostPrice = 150
                };

                offer.PackagedSolution = packagedSolution;
                offer.Materials.Add(material);
                offer.Salaries.Add(salary);
                db.Offers.Add(offer);
                db.SaveChanges();
            }

            // get datasheet test
            using (var ctx = new AssistantContext())
            {
                ctx.DataSheets.ToList().ForEach(sheet => Console.WriteLine(sheet is HeatingUnitDataSheet));
            }
        }
    }
}
