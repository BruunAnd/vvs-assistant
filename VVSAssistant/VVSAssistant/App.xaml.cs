using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
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
            InitializeCultureInfo();
            return;
            ShittySeed();
        }
        private void InitializeCultureInfo()
        {
            CultureInfo customCulture = (CultureInfo)
                        System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";
            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;
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

                var material = new Material()
                {
                    UnitCostPrice = 150,
                    UnitSalesPrice = 180
                };

                var salary = new Salary()
                {
                    UnitSalesPrice = 180,
                    UnitCostPrice = 150
                };

                /* Appliances */
                db.Appliances.Add(new Appliance("Cerapur", new HeatingUnitDataSheet()
                { WattUsage = 20, AFUE = 93, AFUEColdClima = 98.2f, AFUEWarmClima = 87.8f }, ApplianceTypes.Boiler)
                { CreationDate = DateTime.Now, UnitCostPrice = 150 });
                db.Appliances.Add(new Appliance("LoganoPlusSB105", new HeatingUnitDataSheet()
                { AFUE = 91, WattUsage = 18, AFUEColdClima = 99.2f, AFUEWarmClima = 92.5f },
                        ApplianceTypes.Boiler)
                { CreationDate = DateTime.Now, UnitCostPrice = 150 });
                db.Appliances.Add(new Appliance("EuroPurACUWater", new HeatingUnitDataSheet()
                {
                    AFUE = 91,
                    WattUsage = 18,
                    AFUEColdClima = 96.5f,
                    AFUEWarmClima = 87.7f,
                    WaterHeatingEffiency = 82,
                    UseProfile = UseProfileType.XL,
                    Vbu = 0
                },
                        ApplianceTypes.Boiler)
                { CreationDate = DateTime.Now, UnitCostPrice = 150 });
                db.Appliances.Add(new Appliance("Condens9000Water", new HeatingUnitDataSheet()
                {
                    AFUE = 94,
                    WattUsage = 29,
                    AFUEColdClima = 98.7f,
                    AFUEWarmClima = 88.5f,
                    WaterHeatingEffiency = 82,
                    UseProfile = UseProfileType.XL,
                    Vbu = 1
                },
                        ApplianceTypes.Boiler)
                { CreationDate = DateTime.Now, UnitCostPrice = 150 });

                db.Appliances.Add(new Appliance("EuroPurUnitSolar", new HeatingUnitDataSheet()
                {
                    AFUE = 92,
                    WattUsage = 13,
                    AFUEColdClima = 98.1f,
                    AFUEWarmClima = 87.8f,
                    WaterHeatingEffiency = 85,
                    UseProfile = UseProfileType.XL,
                    Vbu = 34,
                    StandingLoss = 92.5f
                     ,
                    Vnorm = 204,
                    Psb = 4
                }, ApplianceTypes.Boiler)
                { CreationDate = DateTime.Now, UnitCostPrice = 150 });
                db.Appliances.Add(new Appliance("Condens5000", new HeatingUnitDataSheet()
                {
                    UseProfile = UseProfileType.XXL,
                    WattUsage = 24,
                    Vbu = 110,
                    Psb = 2,
                    WaterHeatingEffiency = 78,
                    isWaterHeater = true,
                    isRoomHeater = true,
                    AFUEColdClima = 98,
                    AFUEWarmClima = 87.8f
                }, ApplianceTypes.Boiler)
                { CreationDate = DateTime.Now, UnitCostPrice = 150 });
                db.Appliances.Add(new Appliance("LoganoPlusSB105", new HeatingUnitDataSheet()
                { AFUE = 94, WattUsage = 42 }, ApplianceTypes.Boiler)
                { CreationDate = DateTime.Now, UnitCostPrice = 150 });
                db.Appliances.Add(new Appliance("Vitoladens 300-W", new HeatingUnitDataSheet()
                { AFUE = 91, WattUsage = 18, InternalTempControl = "3" },
                        ApplianceTypes.Boiler)
                { CreationDate = DateTime.Now, UnitCostPrice = 150 });
                db.Appliances.Add(new Appliance("SBT100-3", new SolarStationDataSheet()
                { SolStandbyConsumption = 2.72f, SolPumpConsumption = 45 }, ApplianceTypes.SolarStation)
                { CreationDate = DateTime.Now, UnitCostPrice = 150 });
                db.Appliances.Add(new Appliance("SBT65-3", new SolarStationDataSheet()
                { SolStandbyConsumption = 2.72f, SolPumpConsumption = 35f }, ApplianceTypes.SolarStation)
                { CreationDate = DateTime.Now, UnitCostPrice = 150 });
                db.Appliances.Add(new Appliance("SBT35-3", new SolarStationDataSheet()
                { SolStandbyConsumption = 2.72f, SolPumpConsumption = 30f }, ApplianceTypes.SolarStation)
                { CreationDate = DateTime.Now, UnitCostPrice = 150 });
                db.Appliances.Add(new Appliance("SBT160-3", new SolarStationDataSheet()
                { SolStandbyConsumption = 2.72f, SolPumpConsumption = 70f }, ApplianceTypes.SolarStation)
                { CreationDate = DateTime.Now, UnitCostPrice = 150 });
                db.Appliances.Add(new Appliance("Compress7000", new HeatingUnitDataSheet()
                { AFUE = 158, AFUEColdClima = 164, AFUEWarmClima = 151, WattUsage = 10 }, ApplianceTypes.HeatPump)
                { CreationDate = DateTime.Now, UnitCostPrice = 150 });
                db.Appliances.Add(new Appliance("Vitocal 200-S", new HeatingUnitDataSheet()
                { AFUE = 118, AFUEColdClima = 100, AFUEWarmClima = 183, WattUsage = 9, InternalTempControl = "2" }, ApplianceTypes.HeatPump)
                { CreationDate = DateTime.Now, UnitCostPrice = 150 });
                db.Appliances.Add(new Appliance("Vitocal 200-S", new HeatingUnitDataSheet()
                { AFUE = 133, AFUEColdClima = 135, AFUEWarmClima = 132, WattUsage = 43, InternalTempControl = "7" }, ApplianceTypes.HeatPump)
                { CreationDate = DateTime.Now, UnitCostPrice = 150 });
                db.Appliances.Add(new Appliance("Vitocal 350-A", new HeatingUnitDataSheet()
                { AFUE = 112, AFUEColdClima = 98, AFUEWarmClima = 138, WattUsage = 10, InternalTempControl = "3" }, ApplianceTypes.HeatPump)
                { CreationDate = DateTime.Now, UnitCostPrice = 150 });
                db.Appliances.Add(new Appliance("Compress6000 AW-5", new HeatingUnitDataSheet()
                { AFUE = 139, AFUEColdClima = 130, AFUEWarmClima = 164, WattUsage = 4 }, ApplianceTypes.HeatPump)
                { CreationDate = DateTime.Now, UnitCostPrice = 150 });
                db.Appliances.Add(new Appliance("LogasolSKN", new SolarCollectorDataSheet()
                {
                    Area = 2.25f,
                    Efficency = 60,
                    N0 = 0.766f,
                    a1 = 3.22f,
                    a2 = 0.015f,
                    IAM = 0.92f
                }, ApplianceTypes.SolarPanel)
                { CreationDate = DateTime.Now, UnitCostPrice = 150 });
                db.Appliances.Add(new Appliance("FKC25", new SolarCollectorDataSheet()
                {
                    Area = 2.25f,
                    Efficency = 61,
                    N0 = 0.766f,
                    a1 = 3.22f,
                    a2 = 0.015f,
                    IAM = 0.92f
                }, ApplianceTypes.SolarPanel)
                { CreationDate = DateTime.Now, UnitCostPrice = 150 });
                db.Appliances.Add(new Appliance("Vitosol 300-T", new SolarCollectorDataSheet()
                { Area = 3.19f, Efficency = 72 }, ApplianceTypes.SolarPanel)
                { CreationDate = DateTime.Now, UnitCostPrice = 150 });
                db.Appliances.Add(new Appliance("Logasol SKN 4.0", new SolarCollectorDataSheet()
                { Area = 2.25f, Efficency = 61, isRoomHeater = true }, ApplianceTypes.SolarPanel)
                { CreationDate = DateTime.Now, UnitCostPrice = 150 });
                db.Appliances.Add(new Appliance("Vitosol 200-T", new SolarCollectorDataSheet()
                { Area = 1.33f, Efficency = 67.5f, isRoomHeater = true }, ApplianceTypes.SolarPanel)
                { CreationDate = DateTime.Now, UnitCostPrice = 150 });
                db.Appliances.Add(new Appliance("Vitosol 200-T SP2A", new SolarCollectorDataSheet()
                { Area = 3.19f, Efficency = 67.4f, isRoomHeater = true }, ApplianceTypes.SolarPanel)
                { CreationDate = DateTime.Now, UnitCostPrice = 150 });
                db.Appliances.Add(new Appliance("SomeContiner", new ContainerDataSheet()
                { Volume = 500, Classification = "B", StandingLoss = 80 }, ApplianceTypes.Container)
                { CreationDate = DateTime.Now, UnitCostPrice = 150 });
                db.Appliances.Add(new Appliance("BST 500", new ContainerDataSheet()
                { Volume = 481, Classification = "C", StandingLoss = 100 }, ApplianceTypes.Container)
                { CreationDate = DateTime.Now, UnitCostPrice = 150 });
                db.Appliances.Add(new Appliance("SM 500", new ContainerDataSheet()
                { Volume = 500, Classification = "B", StandingLoss = 80 }, ApplianceTypes.Container)
                { CreationDate = DateTime.Now, UnitCostPrice = 150 });
                db.Appliances.Add(new Appliance("Vitocell 140-E 400l", new ContainerDataSheet()
                { Volume = 400, Classification = "B", StandingLoss = 75 }, ApplianceTypes.Container)
                { CreationDate = DateTime.Now, UnitCostPrice = 150 });
                db.Appliances.Add(new Appliance("Vitocell 140-E 950l", new ContainerDataSheet()
                { Volume = 950, Classification = "C" }, ApplianceTypes.Container)
                { CreationDate = DateTime.Now, UnitCostPrice = 150 });
                db.Appliances.Add(new Appliance("Vitocell 300-B", new ContainerDataSheet()
                { Volume = 300, Classification = "C", StandingLoss = 80 }, ApplianceTypes.Container)
                { CreationDate = DateTime.Now, UnitCostPrice = 150 });
                db.Appliances.Add(new Appliance("BST 500/80", new ContainerDataSheet()
                { Volume = 489.3f, Classification = "E", StandingLoss = 163 }, ApplianceTypes.Container)
                { CreationDate = DateTime.Now, UnitCostPrice = 150 });
                db.Appliances.Add(new Appliance("SW 750", new ContainerDataSheet()
                { Volume = 741f, Classification = "E", StandingLoss = 179 }, ApplianceTypes.Container)
                { CreationDate = DateTime.Now, UnitCostPrice = 150 });
                db.Appliances.Add(new Appliance("BST 500/80", new ContainerDataSheet()
                { Volume = 93.0f, Classification = "C", StandingLoss = 50 }, ApplianceTypes.Container)
                { CreationDate = DateTime.Now, UnitCostPrice = 150 });
                db.Appliances.Add(new Appliance("BST 500-6", new ContainerDataSheet()
                { Volume = 495.0f, Classification = "B", StandingLoss = 82 }, ApplianceTypes.Container)
                { CreationDate = DateTime.Now, UnitCostPrice = 150 });
                db.Appliances.Add(new Appliance("FB100", new TemperatureControllerDataSheet()
                { Class = "5" }, ApplianceTypes.TemperatureController)
                { CreationDate = DateTime.Now, UnitCostPrice = 150 });
                db.Appliances.Add(new Appliance("CW400", new TemperatureControllerDataSheet()
                { Class = "6" }, ApplianceTypes.TemperatureController)
                { CreationDate = DateTime.Now, UnitCostPrice = 150 });
                db.Appliances.Add(new Appliance("Vitotronic 200", new TemperatureControllerDataSheet()
                { Class = "3" }, ApplianceTypes.TemperatureController)
                { CreationDate = DateTime.Now, UnitCostPrice = 150 });
                db.Appliances.Add(new Appliance("Vitobloc 200", new HeatingUnitDataSheet()
                { AFUE = 140, WattUsage = 39, InternalTempControl = "3" }, ApplianceTypes.CHP)
                { CreationDate = DateTime.Now, UnitCostPrice = 150 });
                packagedSolution.Appliances.Add(new Appliance("Compress3000", new HeatingUnitDataSheet()
                {
                    Vnorm = 260,
                    Vbu = 20,
                    StandingLoss = 159,
                    WaterHeatingEffiency = 95,
                    AFUEColdClima = 83,
                    AFUEWarmClima = 114,
                    isWaterHeater = true,
                    UseProfile = UseProfileType.XL
                }, ApplianceTypes.WaterHeater)
                { CreationDate = DateTime.Now, UnitCostPrice = 150 });

                /* /Appliances */
                offer.PackagedSolution = packagedSolution;
                offer.Materials.Add(material);
                offer.Salaries.Add(salary);
                db.Offers.Add(offer);
                db.SaveChanges();
            }

            // get datasheet test
            using (var ctx = new AssistantContext())
            {
                //ctx.DataSheets.ToList().ForEach(sheet => Console.WriteLine(sheet is HeatingUnitDataSheet));
            }
        }
    }
}
