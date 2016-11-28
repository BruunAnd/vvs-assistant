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
                        BoilerId.Cerapur, SolarPanelId.LogasolSKNWater, 1,
                        0, ContainerId.ClassBHighVolume, TempControlId.FB100);
                case PackagedSolutionId.PrimaryBoilerSame:
                    return new PackageStub(BoilerId.LoganoSB150, ContainerId.ClassBHighVolume, 
                        BoilerId.Cerapur, SolarPanelId.LogasolSKNWater, 1,
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
                        0, SolarPanelId.LogasolSKNWater, 1,
                        HeatpumpId.Compress7000, ContainerId.ClassBHighVolume, TempControlId.FB100);
                case PackagedSolutionId.PirmaryBoilerW3Solar:
                    return new PackageStub(BoilerId.EuroPurUnitSolarWater, ContainerId.ClassBHighVolume, 0,
                        SolarPanelId.LogasolSKNWater, 3, 0, 0, 0);
                case PackagedSolutionId.PrimaryBoilerW1Solar:
                    return new PackageStub(BoilerId.EuroPurUnitSolarWater, ContainerId.ClassBHighVolume, 0,
                        SolarPanelId.LogasolSKNWater, 1, 0, 0, 0);
                case PackagedSolutionId.PrimaryHeatPump6Solars:
                    return new PackageStub(HeatpumpId.Compress7000, ContainerId.BST50080 ,BoilerId.LoganoSB150,
                        SolarPanelId.LogasolSKN40, 6, ContainerId.SW750, TempControlId.CW400);
                case PackagedSolutionId.PrimaryPurUnitSolarWater:
                    return new PackageStub(BoilerId.EuroPurUnitSolarWater, 0,
                        SolarPanelId.LogasolSKNWater, 1, 0);
                case PackagedSolutionId.PrimaryPurUnitSolarWaterWStation:
                    return new PackageStub(BoilerId.EuroPurUnitSolarWater, 0,
                        SolarPanelId.LogasolSKNWater, 1, SolarStationId.SBT1003);
                case PackagedSolutionId.PrimaryCondens1Container:
                    return new PackageStub(BoilerId.Condens5000, ContainerId.ClassBHighVolume, 1, 
                        SolarPanelId.LogasolSKNWater, 1, SolarStationId.SBT1003);
                case PackagedSolutionId.PrimaryCondens3Container:
                    return new PackageStub(BoilerId.Condens5000, ContainerId.ClassBHighVolume, 3,
                        SolarPanelId.LogasolSKNWater, 1, SolarStationId.SBT1003);
                case PackagedSolutionId.PrimaryHeatPump2Solar:
                    return new PackageStub(HeatpumpId.Compress5000, ContainerId.BST500, BoilerId.LoganoSB150,
                        SolarPanelId.LogasolSKNWater, 2, ContainerId.SM500, TempControlId.CW400); 
                case PackagedSolutionId.PrimaryHeatPump4Solars:
                    return new PackageStub(HeatpumpId.Vitocal200S, ContainerId.Vitocell140E400l, BoilerId.Vitodens200,
                        SolarPanelId.Vitosol300T, 4, ContainerId.Vitocell300B, null);
                case PackagedSolutionId.PrimaryHeatPumpNoSolars:
                    return new PackageStub(HeatpumpId.Compress7000, null , BoilerId.LoganoSB150, null, 0, ContainerId.SW750, TempControlId.CW400);
                case PackagedSolutionId.PrimaryCHP4Solars:
                    return new PackageStub(CHPId.Vitobloc200, ContainerId.Vitocell140E400l, BoilerId.Vitodens200,
                        SolarPanelId.Vitosol300T, 4, null, null);
                case PackagedSolutionId.Brian1:
                    return new PackageStub(BoilerId.Vitoladens300W, ContainerId.Vitocell140E950l, null, SolarPanelId.Vitosol200T, 5, null, 0, 0);
                case PackagedSolutionId.Brian2:
                    return new PackageStub(HeatpumpId.Vitocal350A, ContainerId.Vitocell140E950l, null, SolarPanelId.Vitosol200TSP2A, 6, null, 0);
                case PackagedSolutionId.Brian3:
                    return new PackageStub(BoilerId.LoganoSB150, null, 0, null, 0, HeatpumpId.Compress6000AW5, null, null);
                case PackagedSolutionId.Brian5:
                    return new PackageStub(BoilerId.LoganoSB150, ContainerId.BST5006, SolarPanelId.LogasolSKN40, 4, SolarStationId.SBT1603);
                case PackagedSolutionId.WaterHeaterTest:
                    return new PackageStub(BoilerId.LoganoSB150, 0, WaterHeaterId.Compress3000, 
                        SolarPanelId.LogasolSKNWater, 1, SolarStationId.SBT1003, TempControlId.CW400);
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
        PrimaryBoilerW1Solar, PrimaryHeatPump6Solars, PrimaryPurUnitSolarWater, PrimaryPurUnitSolarWaterWStation,
        PrimaryCondens1Container, PrimaryCondens3Container, PrimaryHeatPump2Solar, PrimaryHeatPump4Solars, PrimaryHeatPumpNoSolars,
            PrimaryCHP4Solars, Brian1, Brian2, Brian3, Brian5, WaterHeaterTest
    }
}