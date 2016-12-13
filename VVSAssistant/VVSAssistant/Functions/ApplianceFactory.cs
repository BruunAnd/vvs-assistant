using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VVSAssistant.Models;
using VVSAssistant.Models.DataSheets;

namespace VVSAssistant.Functions
{
    class ApplianceFactory
    {
        public Appliance CreateAppliance(ApplianceTypes type)
        {
            Appliance app = new Appliance();
            switch (type)
            {
                case ApplianceTypes.HeatPump:
                    app.DataSheet = new HeatingUnitDataSheet();
                    return app;
                case ApplianceTypes.Boiler:
                    app.DataSheet = new HeatingUnitDataSheet();
                    return app;
                case ApplianceTypes.TemperatureController:
                    app.DataSheet = new TemperatureControllerDataSheet();
                    return app;
                case ApplianceTypes.SolarPanel:
                    app.DataSheet = new SolarCollectorDataSheet();
                    return app;
                case ApplianceTypes.Container:
                    app.DataSheet = new ContainerDataSheet();
                    return app;
                case ApplianceTypes.LowTempHeatPump:
                    app.DataSheet = new HeatingUnitDataSheet();
                    return app;
                case ApplianceTypes.CHP:
                    app.DataSheet = new HeatingUnitDataSheet();
                    return app;
                case ApplianceTypes.SolarStation:
                    app.DataSheet = new SolarStationDataSheet();
                    return app;
                case ApplianceTypes.WaterHeater:
                    app.DataSheet = new HeatingUnitDataSheet();
                    return app;
                default:
                    return null;
                    break;
            }
        }
    }
}
