using VVSAssistant.Models;
using VVSAssistant.Models.DataSheets;

namespace VVSAssistant.Tests.FunctionsTests.CalculationTests.Strategies
{
    class ApplianceStub : Appliance
    {
        public ApplianceStub(string name, HeatingUnitDataSheet datasheet, ApplianceTypes type)
        {
            Name = name; DataSheet = datasheet; Type = type;
        }
        public ApplianceStub(string name, ContainerDataSheet datasheet, ApplianceTypes type)
        {
            Name = name; DataSheet = datasheet; Type = type;
        }
        public ApplianceStub(string name, SolarCollectorDataSheet datasheet, ApplianceTypes type)
        {
            Name = name; DataSheet = datasheet; Type = type;
        }
        public ApplianceStub(string name, TemperatureControllerDataSheet datasheet, ApplianceTypes type)
        {
            Name = name; DataSheet = datasheet; Type = type;
        }
        public ApplianceStub(string name, SolarStationDataSheet datasheet, ApplianceTypes type)
        {
            Name = name; DataSheet = datasheet; Type = type;
        }
    }
}