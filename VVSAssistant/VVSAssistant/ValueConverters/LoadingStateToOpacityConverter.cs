using System;
using System.Globalization;
using System.Windows.Data;

namespace VVSAssistant.ValueConverters
{
    public class LoadingStateToOpacityConverter :IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool) || (bool)value)
                return 0.25;
            return 1.0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
