using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace CnBetaUWA.Controls
{
    [TemplatePart(Name = "HorizontalTemplate", Type = typeof(Grid))]
    [TemplatePart(Name = "VerticalTemplate", Type = typeof(Grid))]
    public sealed class ExtendSplitView : SplitView
    {
        public ExtendSplitView()
        {
            this.DefaultStyleKey = typeof(ExtendSplitView);
        }



        private Grid _VerticalTemplateGrid;
        private Grid _HorizontalTemplateGrid;

        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Orientation.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register("Orientation", typeof(Orientation), typeof(ExtendSplitView), new PropertyMetadata(0,OrientationPropertyChanged));

        private static void OrientationPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var expandsplitview = d as ExtendSplitView;
            if (expandsplitview!=null&& expandsplitview._HorizontalTemplateGrid!=null&& expandsplitview._VerticalTemplateGrid!=null)
            {
                var orientation = e.NewValue is Orientation ? (Orientation)e.NewValue : Orientation.Vertical;
                switch (orientation)
                {
                    default:
                        expandsplitview._HorizontalTemplateGrid.Visibility = Visibility.Visible;
                        expandsplitview._VerticalTemplateGrid.Visibility = Visibility.Collapsed;
                        break;
                    case Orientation.Vertical:
                        expandsplitview._HorizontalTemplateGrid.Visibility = Visibility.Collapsed;
                        expandsplitview._VerticalTemplateGrid.Visibility = Visibility.Visible;
                        break;
               
                }
            }
          
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _HorizontalTemplateGrid = GetTemplateChild("HorizontalTemplate") as Grid;
            _VerticalTemplateGrid = GetTemplateChild("VerticalTemplate") as Grid;
        }
    }
}
