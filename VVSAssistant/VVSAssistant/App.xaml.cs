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
                    Price = 500
                };
                offer.OfferInformation = offerInformation;
                var packagedSolution = new PackagedSolution()
                {
                    CreationDate = DateTime.Now,
                    Name = "Example Solution"
                };
                var appliance = new Appliance()
                {
                    CreationDate = DateTime.Now,
                    Name = "Example Appliance",
                    Type = ApplianceTypes.Boiler
                };
                var heatPumpDataSheet = new HeatingUnitDataSheet()
                {
                    Price = 200
                };
                appliance.DataSheet = heatPumpDataSheet;
                packagedSolution.Appliances.Add(appliance);
                packagedSolution.Appliances.Add(appliance);
                offer.PackagedSolution = packagedSolution;
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
