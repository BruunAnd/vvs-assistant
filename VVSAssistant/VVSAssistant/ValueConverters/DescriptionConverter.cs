using System;
using System.Globalization;
using System.Windows.Data;

namespace VVSAssistant.ValueConverters
{
    internal class DescriptionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var description = value as string;
            description = description?.Replace(',', '\n').Insert(0, " ");
            return description;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
