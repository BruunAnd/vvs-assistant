using VVSAssistant.Models;
using VVSAssistant.Models.DataSheets;

namespace VVSAssistant.Tests.FunctionsTests.CalculationTests.Strategies
{
    class ApplianceFactory
    {
        public Appliance GetBoiler(BoilerId id)
        {
            switch (id)
            {
                case BoilerId.Cerapur:
                    return new ApplianceStub("Cerapur", new HeatingUnitDataSheet()
                            { WattUsage = 20, AFUE = 93, AFUEColdClima = 98.2f, AFUEWarmClima = 87.8f },
                        ApplianceTypes.Boiler);
                case BoilerId.LoganoSB150:
                    return new ApplianceStub("LoganoPlusSB105", new HeatingUnitDataSheet()
                            { AFUE = 91, WattUsage = 18, AFUEColdClima = 99.2f, AFUEWarmClima = 92.5f },
                        ApplianceTypes.Boiler);
                case BoilerId.EuroPurACUWater:
                    return new ApplianceStub("EuroPurACUWater", new HeatingUnitDataSheet()
                    { AFUE = 91, WattUsage = 18, AFUEColdClima = 96.5f, AFUEWarmClima = 87.7f,
                      WaterHeatingEffiency=82, UseProfile = UseProfileType.XL, Vbu = 0},
                        ApplianceTypes.Boiler);
                case BoilerId.Condens9000Water:
                    return new ApplianceStub("Condens9000Water", new HeatingUnitDataSheet()
                    { AFUE = 94, WattUsage = 29, AFUEColdClima = 98.7f, AFUEWarmClima = 88.5f,
                      WaterHeatingEffiency=82, UseProfile = UseProfileType.XL, Vbu=1},
                        ApplianceTypes.Boiler);
                case BoilerId.EuroPurUnitSolarWater:
                     return new ApplianceStub("EuroPurUnitSolar", new HeatingUnitDataSheet()
                    { AFUE = 92, WattUsage = 13, AFUEColdClima = 98.1f, AFUEWarmClima = 87.8f,
                      WaterHeatingEffiency=85, UseProfile = UseProfileType.XL, Vbu=34, StandingLoss=92.5f
                     ,Vnorm = 204, Psb=4}, ApplianceTypes.Boiler);
                case BoilerId.Condens5000:
                    return new ApplianceStub("Condens5000", new HeatingUnitDataSheet()
                    { UseProfile = UseProfileType.XXL, WattUsage=24, Vbu=110, Psb=2, WaterHeatingEffiency=78,
                        isWaterHeater =true, isRoomHeater=true, AFUEColdClima = 98, AFUEWarmClima=87.8f}, ApplianceTypes.Boiler);
                case BoilerId.Vitodens200:
                    return new ApplianceStub("LoganoPlusSB105", new HeatingUnitDataSheet()
                    { AFUE = 94, WattUsage = 42}, ApplianceTypes.Boiler);
                case BoilerId.Vitoladens300W:
                    return new ApplianceStub("Vitoladens 300-W", new HeatingUnitDataSheet()
                    { AFUE = 91, WattUsage = 18, InternalTempControl = "3"},
                        ApplianceTypes.Boiler);
                default:
                    return new Appliance();
            }
        }
        public Appliance GetSolarStation(SolarStationId id)
        {
            switch (id)
            {
                case SolarStationId.SBT1003:
                    return new ApplianceStub("SBT100-3", new SolarStationDataSheet()
                    { SolStandbyConsumption = 2.72f, SolPumpConsumption = 45 }, ApplianceTypes.SolarStation);
                case SolarStationId.SBT653:
                    return new ApplianceStub("SBT65-3", new SolarStationDataSheet()
                    { SolStandbyConsumption = 2.72f, SolPumpConsumption = 35f }, ApplianceTypes.SolarStation);
                case SolarStationId.SBT353:
                    return new ApplianceStub("SBT35-3", new SolarStationDataSheet()
                    { SolStandbyConsumption = 2.72f, SolPumpConsumption = 30f }, ApplianceTypes.SolarStation);
                default:
                    return new Appliance();
            }
        }
        public Appliance GetHeatpump(HeatpumpId id)
        {
            switch (id)
            {
                case HeatpumpId.Compress7000:
                    return new ApplianceStub("Compress7000", new HeatingUnitDataSheet()
                    { AFUE=158, AFUEColdClima=164, AFUEWarmClima=151, WattUsage=10}, ApplianceTypes.HeatPump);
                case HeatpumpId.Vitocal200S:
                    return new ApplianceStub("Vitocal 200-S", new HeatingUnitDataSheet()
                    { AFUE = 118, AFUEColdClima = 100, AFUEWarmClima = 183, WattUsage = 9, InternalTempControl = "2" }, ApplianceTypes.HeatPump);
                case HeatpumpId.Compress5000:
                    return new ApplianceStub("Vitocal 200-S", new HeatingUnitDataSheet()
                    { AFUE = 133, AFUEColdClima = 135, AFUEWarmClima = 132, WattUsage = 43, InternalTempControl = "7" }, ApplianceTypes.HeatPump);
                case HeatpumpId.Vitocal350A:
                    return new ApplianceStub("Vitocal 350-A", new HeatingUnitDataSheet()
                    { AFUE = 112, AFUEColdClima = 98, AFUEWarmClima = 138, WattUsage = 10, InternalTempControl = "3" }, ApplianceTypes.HeatPump);
                case HeatpumpId.Compress6000AW5:
                    return new ApplianceStub("Compress6000 AW-5", new HeatingUnitDataSheet()
                    { AFUE = 139, AFUEColdClima = 130, AFUEWarmClima = 164, WattUsage = 4}, ApplianceTypes.HeatPump);
                default:
                    return new Appliance();
            }
        }
        public Appliance GetSolarPanel(SolarPanelId id)
        {
            switch (id)
            {
                case SolarPanelId.LogasolSKNWater:
                    return new ApplianceStub("LogasolSKN", new SolarCollectorDataSheet()
                    { Area = 2.25f, Efficency = 60, Asol = 2.25f, N0 = 0.766f, a1 = 3.22f,
                        a2 = 0.015f, IAM = 0.92f }, ApplianceTypes.SolarPanel);
                case SolarPanelId.FKC25Water:
                    return new ApplianceStub("FKC25", new SolarCollectorDataSheet()
                    { Area = 2.25f,Efficency = 61,Asol = 2.25f,N0 = 0.766f,a1 = 3.22f,
                      a2 = 0.015f,IAM = 0.92f}, ApplianceTypes.SolarPanel);
                case SolarPanelId.Vitosol300T:
                    return new ApplianceStub("Vitosol 300-T", new SolarCollectorDataSheet()
                    { Area = 3.19f, Efficency = 72 }, ApplianceTypes.SolarPanel);
                case SolarPanelId.LogasolSKN40:
                    return new ApplianceStub("Logasol SKN 4.0", new SolarCollectorDataSheet()
                    { Area = 2.25f, Efficency = 61 }, ApplianceTypes.SolarPanel);
                case SolarPanelId.Vitosol200T:
                    return new ApplianceStub("Vitosol 200-T", new SolarCollectorDataSheet()
                    { Area = 1.33f, Efficency = 67.5f, isRoomHeater = true}, ApplianceTypes.SolarPanel);
                case SolarPanelId.Vitosol200TSP2A:
                    return new ApplianceStub("Vitosol 200-T SP2A", new SolarCollectorDataSheet()
                    { Area = 3.19f, Efficency = 67.4f, isRoomHeater = true }, ApplianceTypes.SolarPanel);
                default:
                    return new Appliance();
            }
        }
        public Appliance GetContainer(ContainerId id)
        {
            switch (id)
            {
                case ContainerId.ClassBHighVolume:
                    return new ApplianceStub("SomeContiner", new ContainerDataSheet()
                        { Volume = 500, Classification = "B",StandingLoss=80}, ApplianceTypes.Container);
                case ContainerId.BST500:
                    return new ApplianceStub("BST 500", new ContainerDataSheet()
                    { Volume = 481, Classification = "C", StandingLoss = 100 }, ApplianceTypes.Container);
                case ContainerId.SM500:
                    return new ApplianceStub("SM 500", new ContainerDataSheet()
                    { Volume = 500, Classification = "B", StandingLoss = 80 }, ApplianceTypes.Container);
                case ContainerId.Vitocell140E400l:
                    return new ApplianceStub("Vitocell 140-E 400l", new ContainerDataSheet()
                    { Volume = 400, Classification = "B", StandingLoss = 75 }, ApplianceTypes.Container);
                case ContainerId.Vitocell140E950l:
                    return new ApplianceStub("Vitocell 140-E 950l", new ContainerDataSheet()
                    { Volume = 950, Classification = "C"}, ApplianceTypes.Container);
                case ContainerId.Vitocell300B:
                    return new ApplianceStub("Vitocell 300-B", new ContainerDataSheet()
                    { Volume = 300, Classification = "C", StandingLoss = 80 }, ApplianceTypes.Container);
                case ContainerId.BST50080:
                    return new ApplianceStub("BST 500/80", new ContainerDataSheet()
                    { Volume = 489.3f, Classification = "E", StandingLoss = 163 }, ApplianceTypes.Container);
                case ContainerId.SW750:
                    return new ApplianceStub("BST 500/80", new ContainerDataSheet()
                    { Volume = 741f, Classification = "E", StandingLoss = 179 }, ApplianceTypes.Container);
                case ContainerId.CERA110L:
                    return new ApplianceStub("BST 500/80", new ContainerDataSheet()
                    { Volume = 93.0f, Classification = "C", StandingLoss = 50 }, ApplianceTypes.Container);
                default:
                    return new Appliance();
            }
        }
        public Appliance GetTempControl(TempControlId id)
        {
            switch (id)
            {
                case TempControlId.FB100:
                    return new ApplianceStub("FB100", new TemperatureControllerDataSheet()
                        { Class = "5" }, ApplianceTypes.TemperatureController);
                case TempControlId.CW400:
                    return new ApplianceStub("CW400", new TemperatureControllerDataSheet()
                    { Class = "6" }, ApplianceTypes.TemperatureController);
                case TempControlId.Vitotronic200:
                    return new ApplianceStub("Vitotronic 200", new TemperatureControllerDataSheet()
                    { Class = "3" }, ApplianceTypes.TemperatureController);

                default:
                    return new Appliance();
            }
        }

        public Appliance GetCHP(CHPId id)
        {
            switch (id)
            {
                case CHPId.Vitobloc200:
                    return new ApplianceStub("Vitobloc 200", new HeatingUnitDataSheet()
                        { AFUE=140, WattUsage=39, InternalTempControl = "3"}, ApplianceTypes.CHP);
                default:
                    return new Appliance();
            }
        }
    }
}