using System;
using Windows.UI.Xaml.Data;

namespace CnBetaUWA.Converters
{
    public class SliderValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var size = System.Convert.ToInt32(value);
           
            return string.Format("{0:0%}",size*0.5);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
