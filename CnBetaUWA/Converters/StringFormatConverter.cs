using System;
using Windows.UI.Xaml.Data;

namespace CnBetaUWA.Converters
{
    /// <summary>
    /// 由于Winrt的Binding不支持FormatProvider,故用转化器
    /// </summary>
    public class StringFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            // No format provided.
            if (parameter == null)
            {
                return value;
            }

            return String.Format((String)parameter, value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value;
        }
    }

}
