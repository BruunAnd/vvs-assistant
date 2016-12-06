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
        private static readonly Dictionary<ApplianceTypes, string> _typeNameAssocation = new Dictionary<ApplianceTypes, string>()
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

        private static readonly Dictionary<ApplianceTypes, Type> _typeDataSheetTypeAssociation = new Dictionary<ApplianceTypes, Type>()
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

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var castedValue = (ApplianceTypes) value;
            return _typeNameAssocation.ContainsKey(castedValue) ? _typeNameAssocation[castedValue] : null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var castedValue = (string) value;
            return _typeNameAssocation.FirstOrDefault(v => v.Value.Equals(castedValue)).Key;
        }

        public DataSheet ConvertTypeToDataSheet(ApplianceTypes type)
        {
            if (_typeDataSheetTypeAssociation.ContainsKey(type))
                return Activator.CreateInstance(_typeDataSheetTypeAssociation[type]) as DataSheet;
            return null;
        }
    }
}
