using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace CnBetaUWA.Controls
{

    [TemplatePart(Name = "contentControl", Type = typeof(ContentPresenter))]
    [TemplatePart(Name = "contentGrid", Type = typeof(Grid))]
    [TemplatePart(Name = "OpenContent", Type = typeof(Storyboard))]
    [TemplatePart(Name = "CloseContent", Type = typeof(Storyboard))]
    public sealed class SwipeableControl : Control
    {
        public SwipeableControl()
        {
            DefaultStyleKey = typeof(SwipeableControl);
            this.Unloaded += SwipeableControl_Unloaded;
        }

        private void SwipeableControl_Unloaded(object sender, RoutedEventArgs e)
        {
            if (contentGrid != null)
            {
                contentGrid.ManipulationDelta -= ContentGridManipulationDelta;
                contentGrid.ManipulationCompleted -= ContentGridManipulationCompleted;
            }
        }

        private ContentPresenter contentControl;
        private Grid contentGrid;
        private Storyboard OpenStoryboard;
        private Storyboard CloseStoryboard;
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            contentGrid = GetTemplateChild("contentGrid") as Grid;
            contentControl = GetTemplateChild("contentControl") as ContentPresenter;
            CloseStoryboard = GetTemplateChild("CloseContent") as Storyboard;
            OpenStoryboard = GetTemplateChild("OpenContent") as Storyboard;
            if (contentGrid != null)
            {
                contentGrid.ManipulationMode=ManipulationModes.TranslateX;
                contentGrid.ManipulationDelta += ContentGridManipulationDelta;
                contentGrid.ManipulationCompleted += ContentGridManipulationCompleted;
            }
        }


        public bool IsSwipeablePaneOpen
        {
            get { return (bool)GetValue(IsSwipeablePaneOpenProperty); }
            set { SetValue(IsSwipeablePaneOpenProperty, value); }
        }

        public static readonly DependencyProperty IsSwipeablePaneOpenProperty =
            DependencyProperty.Register("IsSwipeablePaneOpen", typeof(bool), typeof(SwipeableControl), new PropertyMetadata(false, OnIsSwipeablePaneOpenChanged));

        private static void OnIsSwipeablePaneOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var swipeableControl = (SwipeableControl)d;
            var isPaneOpen = (bool) e.NewValue;
            if (isPaneOpen)
            {
                swipeableControl.OpenContentGrid();
            }
            else
            {
                swipeableControl.CloseContentGrid();
            }
        }


        public double PanAreaInitialTranslateX
        {
            get { return (double)GetValue(PanAreaInitialTranslateXProperty); }
            set { SetValue(PanAreaInitialTranslateXProperty, value); }
        }

        public static readonly DependencyProperty PanAreaInitialTranslateXProperty =
            DependencyProperty.Register("PanAreaInitialTranslateX", typeof(double), typeof(SwipeableControl), new PropertyMetadata(0d));




        public Control Content
        {
            get { return (Control)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Content.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register("Content", typeof(Control), typeof(SwipeableControl), new PropertyMetadata(0));

        private  void CloseContentGrid()
        {
            CloseStoryboard.Begin();
          
        }

        private  void OpenContentGrid()
        {
            OpenStoryboard.Begin();
            //RefreshComment(this, EventArgs.Empty);
           
        }
        private void ContentGridManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            var x = e.Velocities.Linear.X;
            if (x <= -0.1)
            {
                OpenContentGrid();
            }
            else if (x > -0.1 && x < 0.1)
            {
                var transform = contentGrid.RenderTransform as CompositeTransform;
                if (transform != null && Math.Abs(transform.TranslateX) > 150)
                {
                    OpenContentGrid();
                }
                else
                {
                    CloseContentGrid();
                }
            }
            else
            {
                CloseContentGrid();
            }
        }

        private void ContentGridManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            var compositeTransform = contentGrid.RenderTransform as CompositeTransform;
            if (compositeTransform != null)
            {
                var x = compositeTransform.TranslateX + e.Delta.Translation.X;
                if (x < -300)
                {
                    x = -300;
                }
                compositeTransform.TranslateX = x;
            }
        }
    }
}
