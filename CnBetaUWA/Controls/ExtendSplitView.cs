using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace CnBetaUWA.Controls
{
   
    public sealed class ExtendSplitView : SplitView
    {
        public ExtendSplitView()
        {
            DefaultStyleKey = typeof(ExtendSplitView);
            SizeChanged += ExtendSplitView_SizeChanged;
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
        

        public bool IsOpenBottomPane
        {
            get { return (bool)GetValue(IsOpenBottomPaneProperty); }
            set { SetValue(IsOpenBottomPaneProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsOpenBottomPane.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsOpenBottomPaneProperty =
            DependencyProperty.Register("IsOpenBottomPane", typeof(bool), typeof(ExtendSplitView), new PropertyMetadata(true,IsOpenBottomPanePropertyChanged));

        private static void IsOpenBottomPanePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var extendSplitViwer = d as ExtendSplitView;
            var isopen = (bool) e.NewValue;
            extendSplitViwer?.OpenBottomPane(isopen);
        }


        private void OpenBottomPane(bool isopen)
        {
            BottomPane.Visibility = isopen ? Visibility.Visible : Visibility.Collapsed;
        }

        public FrameworkElement BottomPane
        {
            get { return (FrameworkElement)GetValue(BottomPaneProperty); }
            set { SetValue(BottomPaneProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BottomPane.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BottomPaneProperty =
            DependencyProperty.Register("BottomPane", typeof(FrameworkElement), typeof(ExtendSplitView), new PropertyMetadata(0));



        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            VisualStateManager.GoToState(this, "Closed", true);
        }

       
    }
}
