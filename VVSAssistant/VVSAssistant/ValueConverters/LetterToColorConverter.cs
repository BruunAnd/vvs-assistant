using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace VVSAssistant.ValueConverters
{
    public class LetterToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                switch (value.ToString())
                {
                    case "A+++":
                        return new SolidColorBrush(Color.FromArgb(255, 0, 150, 69));
                    case "A++":
                        return new SolidColorBrush(Color.FromArgb(255, 80, 180, 74));
                    case "A+":
                        return new SolidColorBrush(Color.FromArgb(255, 157, 210, 79));
                    case "A":
                        return new SolidColorBrush(Color.FromArgb(255, 255, 255, 0));
                    case "B":
                        return new SolidColorBrush(Color.FromArgb(255, 255, 192, 0));
                    case "C":
                        return new SolidColorBrush(Color.FromArgb(255, 255, 102, 0));
                    case "D":
                    case "E":
                    case "F":
                    case "G":
                        return new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
                    default:
                        return new SolidColorBrush(Color.FromArgb(255,255,255,255));
                }
            }

            return new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
