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
            SolidColorBrush color;

            switch(applianceType)
            {
                case ApplianceTypes.HeatPump:
                    color = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                    break;
                case ApplianceTypes.Boiler:
                    color = new SolidColorBrush(Color.FromRgb(240, 255, 255));
                    break;
                case ApplianceTypes.TemperatureController:
                    color = new SolidColorBrush(Color.FromRgb(225, 255, 255));
                    break;
                case ApplianceTypes.SolarPanel:
                    color = new SolidColorBrush(Color.FromRgb(210, 255, 255));
                    break;
                case ApplianceTypes.Container:
                    color = new SolidColorBrush(Color.FromRgb(195, 255, 255));
                    break;
                case ApplianceTypes.LowTempHeatPump:
                    color = new SolidColorBrush(Color.FromRgb(180, 255, 255));
                    break;
                case ApplianceTypes.CHP:
                    color = new SolidColorBrush(Color.FromRgb(165, 255, 255));
                    break;
                case ApplianceTypes.SolarStation:
                    color = new SolidColorBrush(Color.FromRgb(150, 255, 255));
                    break;
                case ApplianceTypes.WaterHeater:
                    color = new SolidColorBrush(Color.FromRgb(135, 255, 255));
                    break;
                default:
                    color = new SolidColorBrush(Color.FromRgb(255,255,255));
                    break;
            }
            return color;
            //(int)applianceType % 2 == 0 ? null : new SolidColorBrush(Color.FromRgb(211, 211, 211));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
