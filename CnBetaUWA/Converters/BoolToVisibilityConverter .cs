using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace CnBetaUWA.Converters
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (bool)value ? Visibility.Visible : Visibility.Collapsed;

        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value != null)
                return (value == (object) Visibility.Visible) ? true : false;

            return null;
        }
    }
}
