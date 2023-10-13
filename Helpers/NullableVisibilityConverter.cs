using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace DSP.Helpers
{
    [ValueConversion(typeof(double?), typeof(Visibility))]
    public class NullableVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
