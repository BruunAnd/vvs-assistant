using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using VVSAssistant.Models;
using VVSAssistant.Models.DataSheets;

namespace VVSAssistant.ValueConverters
{
    public class ApplianceTypeConverter : IValueConverter
    {
        private static readonly Dictionary<ApplianceTypes, string> TypeNameAssocation = new Dictionary<ApplianceTypes, string>()
        {
            {ApplianceTypes.Boiler, "Kedel" },
            {ApplianceTypes.CHP, "Kraftvarmeværk"},
            {ApplianceTypes.Container, "Beholder"},
            {ApplianceTypes.HeatPump, "Varmepumpe"},
            {ApplianceTypes.LowTempHeatPump, "Lavtemperatursvarmepumpe"},
            {ApplianceTypes.SolarPanel, "Solpanel"},
            {ApplianceTypes.TemperatureController, "Temperaturregulering"},
            {ApplianceTypes.SolarStation, "Solstation"},
            {ApplianceTypes.WaterHeater, "Vandvarmer"}
        };

        private static readonly Dictionary<ApplianceTypes, Type> TypeDataSheetTypeAssociation = new Dictionary<ApplianceTypes, Type>()
        {
            {ApplianceTypes.Boiler, typeof(HeatingUnitDataSheet) },
            {ApplianceTypes.CHP, typeof(HeatingUnitDataSheet)},
            {ApplianceTypes.Container, typeof(ContainerDataSheet)},
            {ApplianceTypes.HeatPump, typeof(HeatingUnitDataSheet)},
            {ApplianceTypes.LowTempHeatPump, typeof(HeatingUnitDataSheet)},
            {ApplianceTypes.SolarPanel, typeof(SolarCollectorDataSheet)},
            {ApplianceTypes.TemperatureController, typeof(TemperatureControllerDataSheet)},
            {ApplianceTypes.SolarStation, typeof(SolarStationDataSheet)},
            {ApplianceTypes.WaterHeater, typeof(HeatingUnitDataSheet)}
        };

        public static List<string> AppliancesNames
        {
            get { return TypeNameAssocation.Select(v => v.Value).ToList(); }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var castedValue = (ApplianceTypes) value;
            return TypeNameAssocation.ContainsKey(castedValue) ? TypeNameAssocation[castedValue] : null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var castedValue = (string) value;
            return TypeNameAssocation.FirstOrDefault(v => v.Value.Equals(castedValue)).Key;
        }

        public DataSheet ConvertTypeToDataSheet(ApplianceTypes type)
        {
            if (TypeDataSheetTypeAssociation.ContainsKey(type))
                return Activator.CreateInstance(TypeDataSheetTypeAssociation[type]) as DataSheet;
            return null;
        }
    }
}
