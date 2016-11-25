using VVSAssistant.Models;

namespace VVSAssistant.Tests.FunctionsTests.CalculationTests.Strategies
{
    class PackageFactory
    {
        public PackagedSolution GetPackage(PackagedSolutionId id)
        {
            switch (id)
            {
                case PackagedSolutionId.PrimaryBoilerOHeatPump:
                    return new PackageStub(BoilerId.LoganoSB150, ContainerId.ClassBHighVolume, 
                        BoilerId.Cerapur, SolarPanelId.LogasolSKN, 1,
                        0, ContainerId.ClassBHighVolume, TempControlId.FB100);
                case PackagedSolutionId.PrimaryBoilerSame:
                    return new PackageStub(BoilerId.LoganoSB150, ContainerId.ClassBHighVolume, 
                        BoilerId.Cerapur, SolarPanelId.LogasolSKN, 1,
                        0,ContainerId.ClassBHighVolume, TempControlId.FB100);
                case PackagedSolutionId.PrimaryBoilerNulls:
                    return new PackageStub(BoilerId.LoganoSB150, 0, 0,0,1,0,0,0);
                case PackagedSolutionId.WaterHeatingEuroACUSBT1003:
                    return new PackageStub(BoilerId.EuroPurACUWater, ContainerId.ClassBHighVolume,
                        SolarPanelId.FKC25Water,1 , SolarStationId.SBT1003);
                case PackagedSolutionId.WaterHeatingEuroACUSBT653:
                    return new PackageStub(BoilerId.EuroPurACUWater, ContainerId.ClassBHighVolume,
                        SolarPanelId.FKC25Water,1, SolarStationId.SBT653);
                case PackagedSolutionId.WaterHeatingCondens9000SBT353:
                    return new PackageStub(BoilerId.Condens9000Water, ContainerId.ClassBHighVolume,
                        SolarPanelId.FKC25Water,1, SolarStationId.SBT353);
                case PackagedSolutionId.WaterHeatingEuroSolarSBT353:
                    return new PackageStub(BoilerId.EuroPurUnitSolarWater, ContainerId.ClassBHighVolume,
                        SolarPanelId.FKC25Water,1, SolarStationId.SBT353);
                case PackagedSolutionId.PrimaryWaterBoilerOSolar:
                    return new PackageStub(BoilerId.EuroPurACUWater, ContainerId.ClassBHighVolume,0,0,0);
                case PackagedSolutionId.PrimaryWaterBoilerNull:
                    return new PackageStub(BoilerId.EuroPurACUWater, 0,0, 0,0);
                case PackagedSolutionId.PrimaryBoilerWHeatPump:
                    return new PackageStub(BoilerId.LoganoSB150, ContainerId.ClassBHighVolume,
                        0, SolarPanelId.LogasolSKN, 1,
                        HeatpumpId.Compress7000, ContainerId.ClassBHighVolume, TempControlId.FB100);
                case PackagedSolutionId.PirmaryBoilerW3Solar:
                    return new PackageStub(BoilerId.EuroPurUnitSolarWater, ContainerId.ClassBHighVolume, 0,
                        SolarPanelId.LogasolSKNWater, 3, 0, 0, 0);
                case PackagedSolutionId.PrimaryBoilerW1Solar:
                    return new PackageStub(BoilerId.EuroPurUnitSolarWater, ContainerId.ClassBHighVolume, 0,
                        SolarPanelId.LogasolSKNWater, 1, 0, 0, 0);
                case PackagedSolutionId.PrimaryHeatPump6Solars:
                    return new PackageStub(HeatpumpId.Compress7000, ContainerId.BST50080 ,BoilerId.LoganoSB150,
                        SolarPanelId.LogasolSKNWater, 6, ContainerId.SW750, TempControlId.CW400);
                default:
                    return null;
            }
        }
    }
    public enum PackagedSolutionId
    {
        PrimaryBoilerOHeatPump = 1, PrimaryBoilerSame, PrimaryBoilerNulls, WaterHeatingEuroACUSBT1003,
        WaterHeatingEuroACUSBT653, WaterHeatingCondens9000SBT353, WaterHeatingEuroSolarSBT353,
        PrimaryWaterBoilerOSolar, PrimaryWaterBoilerNull, PrimaryBoilerWHeatPump, PirmaryBoilerW3Solar,
        PrimaryBoilerW1Solar, PrimaryHeatPump6Solars       
    }
}