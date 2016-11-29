using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using VVSAssistant.Models;

namespace VVSAssistant.ValueConverters
{
    public class ApplianceTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((ApplianceTypes) value)
            {
                case ApplianceTypes.HeatPump:
                    return "Varmepumpe";
                case ApplianceTypes.Boiler:
                    return "Kedel";
                case ApplianceTypes.TemperatureController:
                    return "Temperaturstyring";
                case ApplianceTypes.SolarPanel:
                    return "Solfanger";
                case ApplianceTypes.Container:
                    return "Beholder";
                case ApplianceTypes.LowTempHeatPump:
                    return "Lavtemperatur varmepumpe";
                case ApplianceTypes.CHP:
                    return "Kraftvarmeværk";
                case ApplianceTypes.SolarStation:
                    return "Solstation";
                case ApplianceTypes.WaterHeater:
                    return "Vandvarmer";
                default:
                    return "Ukendt";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
