﻿using VVSAssistant.Models;

namespace VVSAssistant.Tests.FunctionsTests.CalculationTests.Strategies
{
    class PackageStub : PackagedSolution
    {
        public int Id { get; set; }
        public PackageStub(BoilerId priBoiler, ContainerId solarContain, BoilerId? secBoiler, SolarPanelId? solar, int numberOfSolars,
            HeatpumpId? heatpump, ContainerId? container, TempControlId? tempControl)
        {
            var factory = new ApplianceFactory();
            PrimaryHeatingUnit = factory.GetBoiler((priBoiler));
            SolarContainer = factory.GetContainer((solarContain));
            Appliances.Add(factory.GetBoiler(secBoiler ?? 0));
            for (int i = 0; i < numberOfSolars; i++)
            {
                Appliances.Add(factory.GetSolarPanel(solar ?? 0));
            }
            Appliances.Add(factory.GetHeatpump(heatpump ?? 0));
            Appliances.Add(factory.GetContainer(container ?? 0));
            Appliances.Add(factory.GetTempControl(tempControl ?? 0));
        }
        public PackageStub(BoilerId priBoiler, ContainerId solarContainer, SolarPanelId? solar, int numberOfSolars,
            SolarStationId? solarStation)
        {
            var factory = new ApplianceFactory();
            PrimaryHeatingUnit = factory.GetBoiler((priBoiler));
            SolarContainer = factory.GetContainer((solarContainer));
            for (int i = 0; i < numberOfSolars; i++)
            {
                Appliances.Add(factory.GetSolarPanel(solar ?? 0));
            }
            Appliances.Add(factory.GetSolarStation(solarStation ?? 0));
        }

        public PackageStub(HeatpumpId priPump, ContainerId solarContain, BoilerId? secBoiler, SolarPanelId? solar, int? numberOfSolars, ContainerId? container, TempControlId? tempControl)
        {
            var factory = new ApplianceFactory();
            PrimaryHeatingUnit = factory.GetHeatpump((priPump));
            SolarContainer = factory.GetContainer((solarContain));
            Appliances.Add(factory.GetBoiler(secBoiler ?? 0));

            for (int i = 0; i < numberOfSolars; i++)
            {
                Appliances.Add(factory.GetSolarPanel(solar ?? 0));
            }
            Appliances.Add(factory.GetBoiler(secBoiler ?? 0));
            Appliances.Add(factory.GetContainer(container ?? 0));
            Appliances.Add(factory.GetTempControl(tempControl ?? 0));
        }
    }
}