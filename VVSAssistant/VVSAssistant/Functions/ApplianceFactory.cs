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

            if (type == ApplianceTypes.Boiler || type == ApplianceTypes.CHP ||
                type == ApplianceTypes.HeatPump || type == ApplianceTypes.LowTempHeatPump ||
                type == ApplianceTypes.WaterHeater)
                { app.DataSheet = new HeatingUnitDataSheet(); return app; }
            else if (type == ApplianceTypes.SolarPanel)
                { app.DataSheet = new SolarCollectorDataSheet(); return app; }
            else if (type == ApplianceTypes.SolarStation)
                { app.DataSheet = new SolarStationDataSheet(); return app; }
            else if (type == ApplianceTypes.TemperatureController)
                { app.DataSheet = new TemperatureControllerDataSheet(); return app; }
            else if (type == ApplianceTypes.Container)
                { app.DataSheet = new ContainerDataSheet(); return app; }
            else
                return null;
        }
    }
}
