using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace CnBetaUWA.Converters
{  
    /// <summary>
    /// ListViewItem隔行变色,OddColorBrush奇数背景颜色,EvenColorBrush偶数行背景色
    /// </summary>
    public   class ListViewItemStyleSelector: StyleSelector
    {
        public SolidColorBrush OddColorBrush { get; set; }
        public SolidColorBrush EvenColorBrush { get; set; }
        
        protected override Style SelectStyleCore(object item, DependencyObject container)
        {
            var style = new Style {TargetType = typeof (ListViewItem)};
            var backgroudSetter = new Setter {Property = Windows.UI.Xaml.Controls.Control.BackgroundProperty};

            var listview = ItemsControl.ItemsControlFromItemContainer(container) as ListView;
            if (listview != null)
            {
                var index = listview.IndexFromContainer(container);
                backgroudSetter.Value = index%2==0 ? EvenColorBrush : OddColorBrush ;
            }
            style.Setters.Add(backgroudSetter);

            return style;
        }
    }
}
