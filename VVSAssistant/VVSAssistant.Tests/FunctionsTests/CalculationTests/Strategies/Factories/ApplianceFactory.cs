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
                case BoilerId.Condens5000:
                    return null;
                case BoilerId.LoganoSB150:
                    return new ApplianceStub("LoganoPlusSB105", new HeatingUnitDataSheet()
                            { AFUE = 91, WattUsage = 18, AFUEColdClima = 99.2f, AFUEWarmClima = 92.5f },
                        ApplianceTypes.HeatPump);
                case BoilerId.EuroPurACUWater:
                    return new ApplianceStub("EuroPurACUWater", new HeatingUnitDataSheet()
                    { AFUE = 91, WattUsage = 18, AFUEColdClima = 96.5f, AFUEWarmClima = 87.7f,
                      WaterHeatingEffiency=82, UseProfile = UseProfileType.XL, Vbu = 0},
                        ApplianceTypes.HeatPump);
                case BoilerId.Condens9000Water:
                    return new ApplianceStub("Condens9000Water", new HeatingUnitDataSheet()
                    { AFUE = 94, WattUsage = 29, AFUEColdClima = 98.7f, AFUEWarmClima = 88.5f,
                      WaterHeatingEffiency=82, UseProfile = UseProfileType.XL, Vbu=1},
                        ApplianceTypes.HeatPump);
                case BoilerId.EuroPurUnitSolarWater:
                     return new ApplianceStub("EuroPurUnitSolar", new HeatingUnitDataSheet()
                    { AFUE = 92, WattUsage = 13, AFUEColdClima = 98.1f, AFUEWarmClima = 87.8f,
                      WaterHeatingEffiency=85, UseProfile = UseProfileType.XL, Vbu=34},
                        ApplianceTypes.HeatPump);
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
                default:
                    return new Appliance();
            }
        }
        public Appliance GetSolarPanel(SolarPanelId id)
        {
            switch (id)
            {
                case SolarPanelId.LogasolSKN:
                    return new ApplianceStub("LogasolSKN", new SolarCollectorDataSheet()
                        { Area = 2.25f, Efficency = 60 }, ApplianceTypes.SolarPanel);
                case SolarPanelId.LogasolSKNWater:
                    return new ApplianceStub("LogasolSKN", new SolarCollectorDataSheet()
                    { Area = 2.25f, Efficency = 60, Asol = 2.25f, N0 = 0.766f, a1 = 3.22f,
                        a2 = 0.015f, IAM = 0.92f }, ApplianceTypes.SolarPanel);
                case SolarPanelId.FKC25Water:
                    return new ApplianceStub("FKC25", new SolarCollectorDataSheet()
                    { Area = 2.25f,Efficency = 61,Asol = 2.25f,N0 = 0.766f,a1 = 3.22f,
                      a2 = 0.015f,IAM = 0.92f}, ApplianceTypes.SolarPanel);
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
                default:
                    return new Appliance();
            }
        }
    }
}