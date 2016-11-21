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

            new AssistantContext().Database.Delete();
            // Test database stuff
            using (var db = new AssistantContext())
            {
                // add a new client 'Iaro'
                var clientInformation = new ClientInformation() { Name="Iaroslav", Address="Kvadratet", Email="iaro@russia.ru", PhoneNumber="88888888"};
                db.ClientInformation.Add(clientInformation);
                var client = new Client { CreationDate = DateTime.Now, ClientInformation = clientInformation };
                db.Clients.Add(client);
                db.SaveChanges();
                // offer example
                var offer = new Offer
                {
                    Client = client,
                    CreationDate = DateTime.Now
                };
                var offerInformation = new OfferInformation()
                {
                    Description = "Example description",
                    Price = 500
                };
                db.OfferInformation.Add(offerInformation);
                offer.OfferInformation = offerInformation;
                var packagedSolution = new PackagedSolution()
                {
                    CreationDate = DateTime.Now,
                    Name = "Example Solution"
                };
                db.PackagedSolutions.Add(packagedSolution);
                var appliance = new Appliance()
                {
                    CreationDate = DateTime.Now,
                    Name = "Example Appliance",
                    Type = ApplianceTypes.Boiler
                };
                packagedSolution.Appliances.Add(appliance);
                var heatPumpDataSheet = new HeatingUnitDataSheet()
                {
                    Price = 200
                };
                db.DataSheets.Add(heatPumpDataSheet);
                appliance.DataSheet = heatPumpDataSheet;
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
