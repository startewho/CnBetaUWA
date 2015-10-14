using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace CnBetaUWA.Converters
{
    /// <summary>
    /// Winrt中Setter不能Binding,需要找办法解决
    /// </summary>
    public  sealed class ListItemBackgroudConverter:IValueConverter
    {
        public SolidColorBrush OddColorBrush { get; set; }
        public SolidColorBrush EvenColorBrush { get; set; }
        public object Convert(object value, Type targetType, object parameter, string language)
       {
           var item = value as ListViewItem;
           if (item != null)
           {
                var listView = ItemsControl.ItemsControlFromItemContainer(item);
               if (listView != null)
               {
                   var index = listView.IndexFromContainer(item);
                   return index % 2 == 0 ? EvenColorBrush : OddColorBrush;
               }
           }

           return null;
       }

       public object ConvertBack(object value, Type targetType, object parameter, string language)
       {
           throw new NotImplementedException();
       }
    }
}
