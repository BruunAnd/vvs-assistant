using VVSAssistant.Models;

namespace VVSAssistant.Tests.FunctionsTests.CalculationTests.Strategies
{
    class PackageStub : PackagedSolution
    {
        public int Id { get; set; }
        public PackageStub(BoilerId priBoiler, ContainerId solarContain, BoilerId? secBoiler, SolarPanelId? solar, 
            HeatpumpId? heatpump, ContainerId? container, TempControlId? tempControl)
        {
            var factory = new ApplianceFactory();
            PrimaryHeatingUnit = factory.GetBoiler((priBoiler));
            SolarContainer = factory.GetContainer((solarContain));
            Appliances.Add(factory.GetBoiler(secBoiler ?? 0));
            Appliances.Add(factory.GetSolarPanel(solar ?? 0));
            Appliances.Add(factory.GetHeatpump(heatpump ?? 0));
            Appliances.Add(factory.GetContainer(container ?? 0));
            Appliances.Add(factory.GetTempControl(tempControl ?? 0));
        }
        public PackageStub(BoilerId priBoiler, ContainerId solarContainer, SolarPanelId? solar,
            SolarStationId? solarStation)
        {
            var factory = new ApplianceFactory();
            PrimaryHeatingUnit = factory.GetBoiler((priBoiler));
            SolarContainer = factory.GetContainer((solarContainer));
            Appliances.Add(factory.GetSolarPanel(solar ?? 0));
            Appliances.Add(factory.GetSolarStation(solarStation ?? 0));
        }
    }
}