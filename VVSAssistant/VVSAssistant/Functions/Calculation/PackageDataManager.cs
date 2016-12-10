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
        public HeatingUnitDataSheet PrimaryUnit => _package?.PrimaryHeatingUnit.DataSheet as HeatingUnitDataSheet;
        public ApplianceTypes PrimaryUnitType => _package.PrimaryHeatingUnit.Type;

        /// <summary>
        /// Returns the first SolarPanel in the Appliance list.
        /// The working assumption is that only one type of solarPanel is 
        /// permitted in each packaged solution.
        /// </summary>
        public SolarCollectorDataSheet SolarPanelData {
            get
            { return _package.Appliances.FirstOrDefault(item =>
                    item.Type == ApplianceTypes.SolarPanel)?.DataSheet
                    as SolarCollectorDataSheet;
            }}
        /// <summary>
        /// Returns the first SolarStation in the Appliance list.
        /// The working assumption is that a packaged solution will
        /// only contain one solar station.
        /// </summary>
        public SolarStationDataSheet SolarStationData
        {
            get
            {
                return _package.Appliances.FirstOrDefault(item =>
                    item.Type == ApplianceTypes.SolarStation)?.DataSheet
                    as SolarStationDataSheet;
            }
        }
        /// <summary>
        /// Selects SolarPanels from Appliances based on the given predicate
        /// and calculates the combined area of the solar panels.
        /// </summary>
        /// <returns>Area of the solar panels which furfill the predicate</returns>
        public float SolarPanelArea(Predicate<SolarCollectorDataSheet> panelHeatingUse)
        {
            var solarPanels = _package.Appliances.Where(item =>
            {
                var solarCollectorDataSheet = item?.DataSheet as SolarCollectorDataSheet;
                return solarCollectorDataSheet != null &&
                (item.Type == ApplianceTypes.SolarPanel &&
                panelHeatingUse.Invoke(solarCollectorDataSheet));
            });
            return solarPanels.Select(item => item.DataSheet).OfType<SolarCollectorDataSheet>().
                    Sum(solarCollectorDataSheet => solarCollectorDataSheet.Area);
        }
        /// <summary/>
        /// Selects SolarContainers from SolarContainers list in packaged solution
        /// based on the given predicate.
        /// And calculates the aggregated volume of the containers.
        /// <returns>Aggregated volume of all solar contains that furfill the predicate</returns>
        public float SolarContainerVolume(Predicate<ContainerDataSheet> containerTypes)
        {
            var containers = _package.SolarContainers.Where(item =>
            {
                var containerDataSheet = item?.DataSheet as ContainerDataSheet;
                return containerDataSheet != null && containerTypes.Invoke(containerDataSheet);
            });
            return containers.Sum(item => ((ContainerDataSheet) item.DataSheet).Volume);
        }
        /// <summary>
        /// Returns the number of Solar Contains in the package that furfill
        /// the given predicate
        /// </summary>
        public int NumSolarContainers(Predicate<ContainerDataSheet> containerTypes)
        {
            var ans = _package.SolarContainers.Count(item =>
            {
                var containerDataSheet = item?.DataSheet as ContainerDataSheet;
                return containerDataSheet != null &&
                       containerTypes.Invoke(containerDataSheet);
            });
            return ans;
        }
        /// <summary>
        /// Returns An IEnumerable of Solar Containers from the SolarContainers list
        /// in PackagedSolution, based on the given predicate
        /// </summary>
        public IEnumerable<Appliance> SolarContainers(Predicate<ContainerDataSheet> containerUsage)
        {
            var solarContainers = _package.SolarContainers.Where(item =>
            {
                var containerDataSheet = item?.DataSheet as ContainerDataSheet;
                return containerDataSheet != null &&
                containerUsage.Invoke(containerDataSheet);
            });
            return solarContainers;
        }
        /// <summary>
        /// Returns the first HeatPump in the Appliances list in PackagedSolution.
        /// Working assumption is that a packaged solution cannot contain more than
        /// one supplementary HeatPump.
        /// </summary>
        public HeatingUnitDataSheet SupplementaryHeatPump =>
            _package.Appliances.FirstOrDefault(item =>
                item.Type == ApplianceTypes.HeatPump &&
                item != _package.PrimaryHeatingUnit)?.DataSheet
                as HeatingUnitDataSheet;
        /// <summary>
        /// Returns the first Boiler in the Appliances list in PackagedSolution.
        /// Working assumption is that a packaged solution connot contain more 
        /// than one supplementary Boiler.
        /// </summary>
        public HeatingUnitDataSheet SupplementaryBoiler =>
            _package.Appliances.FirstOrDefault(item =>
                item.Type == ApplianceTypes.Boiler &&
                item != _package.PrimaryHeatingUnit)?.DataSheet
                as HeatingUnitDataSheet;
        /// <summary>
        /// Returns true when there exists an container in the packaged solution
        /// which is not associated with the Solar Heating system.
        /// </summary>
        public bool HasNonSolarContainer()
        {
            var containers = _package.Appliances.Where(item => item.Type == ApplianceTypes.Container);
            var solarContainers = _package.SolarContainers.Where(item => item.Type == ApplianceTypes.Container);
            if (containers.Count() <= solarContainers.Count())
            {
                return false;
            }
            else
                return true;

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
                        ((_package?.Appliances.FirstOrDefault(item =>
                            item?.Type == ApplianceTypes.TemperatureController)?
                                .DataSheet as TemperatureControllerDataSheet)?.Class ?? "0");
                return TemperatureControllerDataSheet.ClassBonus[classification];
            }
        }
        /// <summary>
        /// Returns the Solar Container Energy class bonus
        /// </summary>
        public float SolarContainerClass => ContainerDataSheet.ClassificationClass[
        ((ContainerDataSheet) _package?.SolarContainers.FirstOrDefault()?.DataSheet)?.Classification ?? "0"];

        public CalculationType CalculationStrategyType(PackagedSolution package, EEICalculationResult result)
        {
            var primaryType = package.PrimaryHeatingUnit.Type;
            var primaryData = PrimaryUnit;
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
