using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using VVSAssistant.Models;

namespace VVSAssistant.ValueConverters
{
    public class ApplianceTypeColorConverter : IValueConverter
    {
        private static readonly Dictionary<ApplianceTypes, string> TypeNameAssocation = new Dictionary<ApplianceTypes, string>()
        {
            {ApplianceTypes.Boiler, "Kedel" },
            {ApplianceTypes.CHP, "Kraftvarmeværk"},
            {ApplianceTypes.Container, "Beholder"},
            {ApplianceTypes.HeatPump, "Varmepumpe"},
            {ApplianceTypes.LowTempHeatPump, "Lavtemperatursvarmepumpe"},
            {ApplianceTypes.SolarPanel, "Solfanger"},
            {ApplianceTypes.TemperatureController, "Temperaturregulering"},
            {ApplianceTypes.SolarStation, "Solstation"},
            {ApplianceTypes.WaterHeater, "Vandvarmer"}
        };
        
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var appliance = (Appliance) value;
            var applianceType = appliance.Type;
            return (int)applianceType % 2 == 0 ? null : new SolidColorBrush(Color.FromRgb(211, 211, 211));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
