using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VVSAssistant.Models;
using VVSAssistant.Models.DataSheets;

namespace VVSAssistant.Functions.Calculation
{
    class PackageDataManager
    {
        private PackagedSolution _package;
        public PackageDataManager(PackagedSolution package)
        {
            _package = package;
        }
        /// <summary>
        /// Retrives the Primary Heating Appliance
        /// </summary>
        public HeatingUnitDataSheet PrimaryUnit
        {
            get
            {
                return _package?.PrimaryHeatingUnit.DataSheet as HeatingUnitDataSheet;
            }
        }
        /// <summary>
        /// Returns the first SolarPanel in the Appliance list.
        /// The working assumption is that only one type of solarPanel is 
        /// permitted in each packaged solution.
        /// </summary>
        public SolarCollectorDataSheet SolarPanelData {
            get
            { return _package.Appliances.FirstOrDefault(item =>
                    item.Type == ApplianceTypes.SolarPanel)?.DataSheet
                    as SolarCollectorDataSheet; ; }}
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
        public float SolarPanelArea(Predicate<SolarCollectorDataSheet> PanelHeatingUse)
        {
            float ans = 0;
            var solarPanels = _package.Appliances.Where(item =>
            {
                var solarCollectorDataSheet = item?.DataSheet as SolarCollectorDataSheet;
                return solarCollectorDataSheet != null &&
                (item.Type == ApplianceTypes.SolarPanel &&
                PanelHeatingUse.Invoke(solarCollectorDataSheet));
            });
            foreach (var item in solarPanels)
            {
                var solarCollectorDataSheet = item?.DataSheet as SolarCollectorDataSheet;
                if (solarCollectorDataSheet != null)
                    ans += solarCollectorDataSheet.Area;
            }
            return ans;
        }
        /// <summary>
        /// Selects SolarContainers from SolarContainers list in packaged solution
        /// based on the given predicate.
        /// And calculates the aggregated volume of the containers.
        /// <returns>Aggregated volume of all solar contains that furfill the predicate</returns>
        public float SolarContainerVolume(Predicate<ContainerDataSheet> ContainerTypes)
        {
            float ans = 0;
            var containers = _package.SolarContainers.Where(item =>
            {
                var containerDataSheet = item?.DataSheet as ContainerDataSheet;
                return containerDataSheet != null &&
                ContainerTypes.Invoke(containerDataSheet);
            });
            foreach (var item in containers)
            {
                ans += (item.DataSheet as ContainerDataSheet).Volume;
            }
            return ans;
        }
        /// <summary>
        /// Returns the number of Solar Contains in the package that furfill
        /// the given predicate
        /// </summary>
        public int NumSolarContainers(Predicate<ContainerDataSheet> ContainerTypes)
        {
            int ans = 0;
            ans = _package.SolarContainers.Count(item =>
            {
            var containerDataSheet = item?.DataSheet as ContainerDataSheet;
                return containerDataSheet != null &&
                ContainerTypes.Invoke(containerDataSheet);
            });
            return ans;
        }
        /// <summary>
        /// Returns An IEnumerable of Solar Containers from the SolarContainers list
        /// in PackagedSolution, based on the given predicate
        /// </summary>
        public IEnumerable<Appliance> SolarContainers(Predicate<ContainerDataSheet> ContainerUsage)
        {
            var solarContainers = _package.SolarContainers.Where(item =>
            {
                var containerDataSheet = item?.DataSheet as ContainerDataSheet;
                return containerDataSheet != null &&
                ContainerUsage.Invoke(containerDataSheet);
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
            var solarContainers = _package.SolarContainers;
            foreach (var item in containers)
            {
                bool ans = false;
                foreach (var solar in solarContainers)
                {
                    if (item != solar)
                        ans = true;
                    else
                    {
                        ans = false;
                        break;
                    }
                }
                if (ans)
                    return true;
            }
            return false;
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
                string Classification = null;
                Classification = PrimaryUnit.InternalTempControl == null ?
                    (_package?.Appliances.FirstOrDefault(item =>
                        item?.Type == ApplianceTypes.TemperatureController)?
                        .DataSheet as TemperatureControllerDataSheet)?.Class ?? "0" :
                    PrimaryUnit.InternalTempControl;
                return TemperatureControllerDataSheet.ClassBonus[Classification];
            }
        }
        /// <summary>
        /// Returns the Solar Container Energy class bonus
        /// </summary>
        public float SolarContainerClass
        {
            get
            {
                return ContainerDataSheet.ClassificationClass[
                    (_package?.SolarContainers[0]?.DataSheet
                    as ContainerDataSheet).Classification ?? "0"];
            }
        }

    }
}
