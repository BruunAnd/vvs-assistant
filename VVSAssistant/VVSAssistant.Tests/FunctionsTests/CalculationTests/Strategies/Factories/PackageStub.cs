using VVSAssistant.Models;

namespace VVSAssistant.Tests.FunctionsTests.CalculationTests.Strategies
{
    class PackageStub : PackagedSolution
    {
        public PackageStub(BoilerId priBoiler, ContainerId? solarContain, BoilerId? secBoiler, SolarPanelId? solar, int numberOfSolars,
            HeatpumpId? heatpump, ContainerId? container, TempControlId? tempControl)
        {
            var factory = new ApplianceFactory();
            ApplianceInstances.Add(new ApplianceInstance()
            { Appliance = factory.GetBoiler((priBoiler)), IsPrimary = true });

            ApplianceInstances.Add(new ApplianceInstance()
            { Appliance = factory.GetContainer((solarContain ?? 0)),
                IsSolarContainer = true});
            ApplianceInstances.Add(new ApplianceInstance()
            {
                Appliance = factory.GetContainer(solarContain ?? 0)
            });
            ApplianceInstances.Add(new ApplianceInstance()
            {
                Appliance = factory.GetBoiler(secBoiler ?? 0)
            });
            for (int i = 0; i < numberOfSolars; i++)
            {
                ApplianceInstances.Add(new ApplianceInstance()
                {
                    Appliance = factory.GetSolarPanel(solar ?? 0)
                });
            }
            ApplianceInstances.Add(new ApplianceInstance()
            {
                Appliance = factory.GetHeatpump(heatpump ?? 0)
            });
            ApplianceInstances.Add(new ApplianceInstance()
            {
                Appliance = factory.GetContainer(container ?? 0)
            });
            ApplianceInstances.Add(new ApplianceInstance()
            {
                Appliance = factory.GetTempControl(tempControl ?? 0)
            });
        }
        public PackageStub(BoilerId priBoiler, ContainerId? solarContain, WaterHeaterId waterHeater, SolarPanelId? solar, 
            int numberOfSolars, SolarStationId solarStation, TempControlId? tempControl)
        {
            var factory = new ApplianceFactory();
            ApplianceInstances.Add(new ApplianceInstance()
            { Appliance = factory.GetBoiler((priBoiler)), IsPrimary = true });

            ApplianceInstances.Add(new ApplianceInstance()
            { Appliance = factory.GetContainer((solarContain ?? 0)),
              IsSolarContainer=true
            });
            ApplianceInstances.Add(new ApplianceInstance()
            {
                Appliance = factory.GetContainer(solarContain ?? 0)
            });
            for (int i = 0; i < numberOfSolars; i++)
            {
                ApplianceInstances.Add(new ApplianceInstance()
                {
                    Appliance = factory.GetSolarPanel(solar ?? 0)
                });
            }
            ApplianceInstances.Add(new ApplianceInstance()
            {
                Appliance = factory.GetTempControl(tempControl ?? 0)
            });
            ApplianceInstances.Add(new ApplianceInstance()
            {
                Appliance = factory.GetWaterHeater(waterHeater)
            });
            ApplianceInstances.Add(new ApplianceInstance()
            {
                Appliance = factory.GetSolarStation(solarStation)
            });
            ApplianceInstances.Add(new ApplianceInstance()
            {
                Appliance = factory.GetTempControl(tempControl ?? 0)
            });
        }
        public PackageStub(BoilerId priBoiler, ContainerId? solarContainer, SolarPanelId? solar, int numberOfSolars,
            SolarStationId? solarStation)
        {

            var factory = new ApplianceFactory();
            ApplianceInstances.Add(new ApplianceInstance()
            { Appliance = factory.GetBoiler((priBoiler)), IsPrimary=true });
            ApplianceInstances.Add(new ApplianceInstance()
            {
                Appliance = factory.GetContainer(solarContainer ?? 0),
                IsSolarContainer=true
            });
            for (int i = 0; i < numberOfSolars; i++)
            {
                ApplianceInstances.Add(new ApplianceInstance()
                {
                    Appliance = factory.GetSolarPanel(solar ?? 0)
                });
            }
            ApplianceInstances.Add(new ApplianceInstance()
            {
                Appliance = factory.GetSolarStation(solarStation ?? 0)
            });
        }

        public PackageStub(HeatpumpId? priPump, ContainerId? solarContain, BoilerId? secBoiler, SolarPanelId? solar,
            int? numberOfSolars, ContainerId? container, TempControlId? tempControl)
        {
            var factory = new ApplianceFactory();
            ApplianceInstances.Add(new ApplianceInstance()
            { Appliance = factory.GetHeatpump((priPump ?? 0)), IsPrimary=true });
            ApplianceInstances.Add(new ApplianceInstance()
            {
                Appliance = factory.GetContainer(solarContain ?? 0),
                IsSolarContainer = true
            });
            ApplianceInstances.Add(new ApplianceInstance()
            {
                Appliance = factory.GetContainer(container ?? 0)
            });
            for (int i = 0; i < numberOfSolars; i++)
            {
                ApplianceInstances.Add(new ApplianceInstance()
                {
                    Appliance = factory.GetSolarPanel(solar ?? 0)
                });
            }
            ApplianceInstances.Add(new ApplianceInstance()
            {
                Appliance = factory.GetBoiler(secBoiler ?? 0)
            });
            ApplianceInstances.Add(new ApplianceInstance()
            {
                Appliance = factory.GetTempControl(tempControl ?? 0)
            });
        }
        public PackageStub(HeatpumpId priPump, ContainerId solarContainer, int numContainers, BoilerId? secBoiler, SolarPanelId? solar, 
            int? numberOfSolars, ContainerId? container, TempControlId? tempControl)
        {
            var factory = new ApplianceFactory();
            ApplianceInstances.Add(new ApplianceInstance()
            { Appliance = factory.GetHeatpump((priPump)), IsPrimary = true });
            ApplianceInstances.Add(new ApplianceInstance()
            {
                Appliance = factory.GetContainer(solarContainer),
                IsSolarContainer = true
            });
            ApplianceInstances.Add(new ApplianceInstance()
            {
                Appliance = factory.GetContainer(container ?? 0)
            });
            for (int i = 0; i < numberOfSolars; i++)
            {
                ApplianceInstances.Add(new ApplianceInstance()
                {
                    Appliance = factory.GetSolarPanel(solar ?? 0)
                });
            }
            ApplianceInstances.Add(new ApplianceInstance()
            {
                Appliance = factory.GetBoiler(secBoiler ?? 0)
            });
            ApplianceInstances.Add(new ApplianceInstance()
            {
                Appliance = factory.GetTempControl(tempControl ?? 0)
            });
            for (int i = 0; i < numContainers; i++)
            {
                ApplianceInstances.Add(new ApplianceInstance()
                {
                    Appliance = factory.GetContainer(solarContainer),
                    IsSolarContainer=true
                });
            }
            for (int i = 0; i < numContainers-1; i++)
            {
                ApplianceInstances.Add(new ApplianceInstance()
                {
                    Appliance = factory.GetContainer(solarContainer)
                });
            }
        }
        public PackageStub(BoilerId primary, ContainerId? solarContainer,int numContainers, SolarPanelId? solar,
            int? numberOfSolars, TempControlId? tempControl)
        {
            var factory = new ApplianceFactory();
            ApplianceInstances.Add(new ApplianceInstance()
            { Appliance = factory.GetBoiler(primary), IsPrimary = true });
            ApplianceInstances.Add(new ApplianceInstance()
            {
                Appliance = factory.GetContainer(solarContainer ?? 0),
                IsSolarContainer = true
            });
            for (int i = 0; i < numberOfSolars; i++)
            {
                ApplianceInstances.Add(new ApplianceInstance()
                {
                    Appliance = factory.GetSolarPanel(solar ?? 0)
                });
            }
            ApplianceInstances.Add(new ApplianceInstance()
            {
                Appliance = factory.GetTempControl(tempControl ?? 0)
            });
            for (int i = 0; i < numContainers; i++)
            {
                ApplianceInstances.Add(new ApplianceInstance()
                {
                    Appliance = factory.GetContainer(solarContainer ?? 0),
                    IsSolarContainer = true
                });
            }
            for (int i = 0; i < numContainers - 1; i++)
            {
                ApplianceInstances.Add(new ApplianceInstance()
                {
                    Appliance = factory.GetContainer(solarContainer ?? 0)
                });
            }
        }
        public PackageStub(BoilerId primary, ContainerId? solarContainer, int numContainers, SolarPanelId? solar,
            int? numberOfSolars, SolarStationId station)
        {
            var factory = new ApplianceFactory();
            ApplianceInstances.Add(new ApplianceInstance()
            { Appliance = factory.GetBoiler(primary), IsPrimary = true });
            ApplianceInstances.Add(new ApplianceInstance()
            {
                Appliance = factory.GetContainer(solarContainer ?? 0),
                IsSolarContainer = true
            });
            for (int i = 0; i < numberOfSolars; i++)
            {
                ApplianceInstances.Add(new ApplianceInstance()
                {
                    Appliance = factory.GetSolarPanel(solar ?? 0)
                });
            }
            for (int i = 0; i < numContainers; i++)
            {
                ApplianceInstances.Add(new ApplianceInstance()
                {
                    Appliance = factory.GetContainer(solarContainer ?? 0),
                    IsSolarContainer = true
                });
            }
            for (int i = 0; i < numContainers - 1; i++)
            {
                ApplianceInstances.Add(new ApplianceInstance()
                {
                    Appliance = factory.GetContainer(solarContainer ?? 0)
                });
            }
            ApplianceInstances.Add(new ApplianceInstance()
            {
                Appliance = factory.GetSolarStation(station)
            });
        }

        public PackageStub(CHPId? priCHP, ContainerId? solarContain, BoilerId? secBoiler, SolarPanelId? solar,
    int? numberOfSolars, ContainerId? container, TempControlId? tempControl)
        {
            var factory = new ApplianceFactory();
            ApplianceInstances.Add(new ApplianceInstance()
            { Appliance = factory.GetCHP(priCHP ?? 0), IsPrimary = true });
            ApplianceInstances.Add(new ApplianceInstance()
            {
                Appliance = factory.GetContainer(solarContain ?? 0),
                IsSolarContainer = true
            });
            for (int i = 0; i < numberOfSolars; i++)
            {
                ApplianceInstances.Add(new ApplianceInstance()
                {
                    Appliance = factory.GetSolarPanel(solar ?? 0)
                });
            }
            ApplianceInstances.Add(new ApplianceInstance()
            {
                Appliance = factory.GetBoiler(secBoiler ?? 0)
            });
            ApplianceInstances.Add(new ApplianceInstance()
            {
                Appliance = factory.GetSolarPanel(solar ?? 0)
            });
            ApplianceInstances.Add(new ApplianceInstance()
            {
                Appliance = factory.GetContainer(container ?? 0)
            });
            ApplianceInstances.Add(new ApplianceInstance()
            {
                Appliance = factory.GetTempControl(tempControl ?? 0)
            });
        }

        public PackageStub(CHPId priPump, ContainerId solarContainer, int numContainers, BoilerId? secBoiler, SolarPanelId? solar,
            int? numberOfSolars, ContainerId? container, TempControlId? tempControl)
        {
            var factory = new ApplianceFactory();
            ApplianceInstances.Add(new ApplianceInstance()
            { Appliance = factory.GetCHP(priPump), IsPrimary = true });
            ApplianceInstances.Add(new ApplianceInstance()
            {
                Appliance = factory.GetContainer(solarContainer),
                IsSolarContainer = true
            });
            for (int i = 0; i < numberOfSolars; i++)
            {
                ApplianceInstances.Add(new ApplianceInstance()
                {
                    Appliance = factory.GetSolarPanel(solar ?? 0)
                });
            }
            ApplianceInstances.Add(new ApplianceInstance()
            {
                Appliance = factory.GetBoiler(secBoiler ?? 0)
            });
            ApplianceInstances.Add(new ApplianceInstance()
            {
                Appliance = factory.GetSolarPanel(solar ?? 0)
            });
            ApplianceInstances.Add(new ApplianceInstance()
            {
                Appliance = factory.GetContainer(container ?? 0)
            });
            ApplianceInstances.Add(new ApplianceInstance()
            {
                Appliance = factory.GetTempControl(tempControl ?? 0)
            });
            for (int i = 0; i < numContainers; i++)
            {
                ApplianceInstances.Add(new ApplianceInstance()
                {
                    Appliance = factory.GetContainer(solarContainer),
                    IsSolarContainer = true
                });
            }
            for (int i = 0; i < numContainers-1; i++)
            {
                ApplianceInstances.Add(new ApplianceInstance()
                {
                    Appliance = factory.GetContainer(solarContainer)
                });
            }
        }
    }
}