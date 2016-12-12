using System;
using System.Collections.Generic;
using System.Linq;
using VVSAssistant.Models;
using VVSAssistant.Models.DataSheets;

namespace VVSAssistant.Functions.Calculation
{
    internal class PackageDataManager
    {
        private readonly PackagedSolution _package;

        public PackageDataManager(PackagedSolution package)
        {
            _package = package;
        }

        /// <summary>
        /// Retrives the Primary Heating Appliance
        /// </summary>
        public HeatingUnitDataSheet PrimaryUnit => _package?.PrimaryHeatingUnitInstance.Appliance.DataSheet as HeatingUnitDataSheet;
        public ApplianceTypes PrimaryUnitType => _package.PrimaryHeatingUnitInstance.Appliance.Type;

        /// <summary>
        /// Returns the first SolarPanel in the Appliance list that satisfies the predicate.
        /// The working assumption is that only one type of solarPanel is 
        /// permitted in each packaged solution.
        /// </summary>
        public SolarCollectorDataSheet SolarPanelData(Predicate<ApplianceInstance> solarPanelData)
        {
            var solarPanel = _package.ApplianceInstances.FirstOrDefault(item =>
            {
                var solarCollectorDataSheet = item?.Appliance?.DataSheet as SolarCollectorDataSheet;
                return solarCollectorDataSheet != null && item.Appliance?.Type == ApplianceTypes.SolarPanel && solarPanelData.Invoke(item);
            });
            return solarPanel?.Appliance?.DataSheet as SolarCollectorDataSheet ?? null;
        }

        /// <summary>
        /// Returns the first SolarStation in the Appliance list.
        /// The working assumption is that a packaged solution will
        /// only contain one solar station.
        /// </summary>
        public SolarStationDataSheet SolarStationData
        {
            get
            {
                return _package.ApplianceInstances.FirstOrDefault(item => item?.Appliance?.Type == ApplianceTypes.SolarStation)?.Appliance?.DataSheet as SolarStationDataSheet;
            }
        }
        /// <summary>
        /// Selects SolarPanels from Appliances based on the given predicate
        /// and calculates the combined area of the solar panels.
        /// </summary>
        /// <returns>Area of the solar panels which furfill the predicate</returns>
        public float SolarPanelArea(Predicate<ApplianceInstance> panelHeatingUse)
        {
            var solarPanels = _package.ApplianceInstances.Where(item =>
            {
                var solarCollectorDataSheet = item?.Appliance?.DataSheet as SolarCollectorDataSheet;
                return solarCollectorDataSheet != null && item.Appliance?.Type == ApplianceTypes.SolarPanel && panelHeatingUse.Invoke(item);
            });
            return solarPanels.Select(item => item.Appliance.DataSheet).OfType<SolarCollectorDataSheet>().Sum(solarCollectorDataSheet => solarCollectorDataSheet.Area);
        }

        /// <summary/>
        /// Selects SolarContainers from SolarContainers list in packaged solution
        /// based on the given predicate.
        /// And calculates the aggregated volume of the containers.
        /// <returns>Aggregated volume of all solar contains that furfill the predicate</returns>
        public float SolarContainerVolume(Predicate<ContainerDataSheet> containerTypes)
        {
            var containers = _package.SolarContainerInstances.Where(item =>
            {
                var containerDataSheet = item?.Appliance?.DataSheet as ContainerDataSheet;
                return containerDataSheet != null && containerTypes.Invoke(containerDataSheet);
            });
            return containers.Sum(item => ((ContainerDataSheet) item.Appliance.DataSheet).Volume);
        }

        /// <summary>
        /// Returns the number of Solar Contains in the package that furfill
        /// the given predicate
        /// </summary>
        public int NumSolarContainers(Predicate<ContainerDataSheet> containerTypes)
        {
            var ans = _package.SolarContainerInstances.Count(item =>
            {
                var containerDataSheet = item?.Appliance?.DataSheet as ContainerDataSheet;
                return containerDataSheet != null && containerTypes.Invoke(containerDataSheet);
            });
            return ans;
        }

        /// <summary>
        /// Returns An IEnumerable of Solar Containers from the SolarContainers list
        /// in PackagedSolution, based on the given predicate
        /// </summary>
        public IEnumerable<ApplianceInstance> SolarContainers(Predicate<ContainerDataSheet> containerUsage)
        {
            var solarContainers = _package.SolarContainerInstances.Where(item =>
            {
                var containerDataSheet = item?.Appliance?.DataSheet as ContainerDataSheet;
                return containerDataSheet != null && containerUsage.Invoke(containerDataSheet);
            });
            return solarContainers;
        }
        /// <summary>
        /// Returns the first HeatPump in the Appliances list in PackagedSolution.
        /// Working assumption is that a packaged solution cannot contain more than
        /// one supplementary HeatPump.
        /// </summary>
        public HeatingUnitDataSheet SupplementaryHeatPump => _package.ApplianceInstances.FirstOrDefault(item => item?.Appliance?.Type == ApplianceTypes.HeatPump &&
            item != _package.PrimaryHeatingUnitInstance)?.Appliance?.DataSheet as HeatingUnitDataSheet;

        /// <summary>
        /// Returns the first Boiler in the Appliances list in PackagedSolution.
        /// Working assumption is that a packaged solution connot contain more 
        /// than one supplementary Boiler.
        /// </summary>
        public HeatingUnitDataSheet SupplementaryBoiler => _package.ApplianceInstances.FirstOrDefault(item => item?.Appliance?.Type == ApplianceTypes.Boiler &&
            item != _package.PrimaryHeatingUnitInstance)?.Appliance?.DataSheet as HeatingUnitDataSheet;

        /// <summary>
        /// Returns true when there exists an container in the packaged solution
        /// which is not associated with the Solar Heating system.
        /// </summary>
        public bool HasNonSolarContainer()
        {
            var containers = _package.ApplianceInstances.Where(item => item?.Appliance?.Type == ApplianceTypes.Container);
            var solarContainers = _package.SolarContainerInstances.Where(item => item?.Appliance?.Type == ApplianceTypes.Container);
            return containers.Count() > solarContainers.Count();
        }

        /// <summary>
        /// Returns the Primary HeatingUnit internal temp controller bonus
        /// if it has one. If one does not exsist the return value is the
        /// classification bonus of the first found TempController in the
        /// appliance list in packaged solution.
        /// </summary>
        public float TempControllerBonus
        {
            get
            {
                var classification = PrimaryUnit.InternalTempControl ?? 
                        ((_package?.ApplianceInstances?.FirstOrDefault(item =>
                            item?.Appliance?.Type == ApplianceTypes.TemperatureController)?
                                .Appliance.DataSheet as TemperatureControllerDataSheet)?.Class ?? "0");
                return TemperatureControllerDataSheet.ClassBonus[classification];
            }
        }
        /// <summary>
        /// Returns the Solar Container Energy class bonus
        /// </summary>
        public float SolarContainerClass => ContainerDataSheet.ClassificationClass[
        ((ContainerDataSheet) _package?.SolarContainerInstances.FirstOrDefault()?.Appliance?.DataSheet)?.Classification ?? "0"];

        public CalculationType CalculationStrategyType(PackagedSolution package, EEICalculationResult result)
        {
            var primaryType = package.PrimaryHeatingUnitInstance.Appliance.Type;
            switch (primaryType)
            {
                case ApplianceTypes.HeatPump:
                    return CalculationType.PrimaryHeatPump;
                case ApplianceTypes.Boiler:
                    return Math.Abs(result.WaterHeatingEffciency - default(float)) > 0 ? CalculationType.PrimaryWaterBoiler : CalculationType.PrimaryBoiler;
                case ApplianceTypes.LowTempHeatPump:
                    return CalculationType.PrimaryLowTempHeatPump;
                case ApplianceTypes.CHP:
                    return CalculationType.PrimaryCHP;
                default:
                    return 0;
            }
        }
    }
}
