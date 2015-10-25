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
   
    public sealed class ExtendSplitView : SplitView
    {
        public ExtendSplitView()
        {
            this.DefaultStyleKey = typeof(ExtendSplitView);
            this.SizeChanged += ExtendSplitView_SizeChanged;
        }

        private void ExtendSplitView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var width = e.NewSize.Width;
            if (width>MinBottomWidth)
            {
                VisualStateManager.GoToState(this, "ClosedCompactLeft", true);
            }
        }

        public double MinBottomWidth
        {
            get { return (double)GetValue(MinBottomWidthProperty); }
            set { SetValue(MinBottomWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MinBottomWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinBottomWidthProperty =
            DependencyProperty.Register("MinBottomWidth", typeof(double), typeof(ExtendSplitView), new PropertyMetadata(0));

        
        public Grid BottomGrid
        {
            get { return (Grid)GetValue(BottomGridProperty); }
            set { SetValue(BottomGridProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BottomGrid.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BottomGridProperty =
            DependencyProperty.Register("BottomGrid", typeof(Grid), typeof(ExtendSplitView), new PropertyMetadata(0));


        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            VisualStateManager.GoToState(this, "Closed", true);
        }


    }
}
