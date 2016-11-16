using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using VVSAssistant.Models;

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

            // Test database stuff
            using (var db = new Models.AssistantModelContainer())
            {
                var clientInformation = new ClientInformation() { Name="Iaroslav", Address="Kvadratet", Email="iaro@russia.ru", PhoneNumber="88888888"};
                db.ClientInformation.Add(clientInformation);
                var client = new Client();
                client.CreationDate = DateTime.Now;
                client.ClientInformation = clientInformation;
                db.Clients.Add(client);
                db.SaveChanges();
            }

        }
    }
}
