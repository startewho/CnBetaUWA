using System;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace CnBetaUWA.Controls
{
    [TemplatePart(Name = "MasterPresenter", Type = typeof(ContentPresenter))]
    [TemplatePart(Name = "DetailPresenter", Type = typeof(Border))]
    [TemplateVisualState(GroupName = "AdaptiveStates", Name = "NarrowState")]
    [TemplateVisualState(GroupName = "AdaptiveStates", Name = "FilledState")]
    public sealed class MasterDetailView : ContentControl
    {
        private ContentPresenter MasterPresenter;
        private Border DetailPresenter;
        private VisualStateGroup AdaptiveStates;
        private bool IsMasterHidden;
        private const string NarrowState = "NarrowState";

        public Frame DetailFrame { get; private set; }
       

        public MasterDetailView()
        {
            DefaultStyleKey = typeof(MasterDetailView);
            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Window.Current.CoreWindow.Dispatcher.AcceleratorKeyActivated += OnAcceleratorKeyActivated;
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            Window.Current.CoreWindow.Dispatcher.AcceleratorKeyActivated -= OnAcceleratorKeyActivated;
        }

        private void OnAcceleratorKeyActivated(CoreDispatcher sender, AcceleratorKeyEventArgs args)
        {
            if ((args.EventType == CoreAcceleratorKeyEventType.SystemKeyDown || args.EventType == CoreAcceleratorKeyEventType.KeyDown) && (args.VirtualKey == VirtualKey.Escape))
            {
                if (!DetailFrame.CanGoBack || CurrentState != MasterDetailState.Narrow) return;
                DetailFrame.GoBack();
                args.Handled = true;
            }
        }

        protected override void OnApplyTemplate()
        {
            MasterPresenter = (ContentPresenter)GetTemplateChild("MasterFrame");
            DetailPresenter = (Border)GetTemplateChild("DetailPresenter");
            AdaptiveStates = (VisualStateGroup)GetTemplateChild("AdaptiveStates");
            AdaptiveStates.CurrentStateChanged += OnCurrentStateChanged;

            if (DetailFrame != null)
            {
                DetailFrame.Navigated += OnNavigated;
                DetailPresenter.Child = DetailFrame;

                if (DetailFrame.CurrentSourcePageType == null)
                {
                    DetailFrame.Navigate(BlankPageType);
                }
                else
                {
                    DetailFrame.BackStack.Insert(0, new PageStackEntry(BlankPageType, null, null));
                }
            }
        }

        private void OnNavigated(object sender, NavigationEventArgs e)
        {
            if (CurrentState == MasterDetailState.Narrow && e.SourcePageType == BlankPageType)
            {
                DetailPresenter.Visibility = Visibility.Collapsed;
            }
            else
            {
                DetailPresenter.Visibility = Visibility.Visible;
            }

            if (CurrentState == MasterDetailState.Narrow && IsAnimated)
            {
                if (e.NavigationMode == NavigationMode.New && DetailFrame.BackStackDepth == 1)
                {
                    IsMasterHidden = true;

                    var anim = new DrillInThemeAnimation
                    {
                        EntranceTarget = new Border(),
                        ExitTarget = MasterPresenter
                    };

                    var board = new Storyboard();
                    board.Children.Add(anim);
                    board.Begin();
                }
                else if (e.NavigationMode == NavigationMode.Back && DetailFrame.BackStackDepth == 0)
                {
                    IsMasterHidden = false;

                    var anim = new DrillOutThemeAnimation
                    {
                        EntranceTarget = MasterPresenter,
                        ExitTarget = new Border()
                    };

                    var board = new Storyboard();
                    board.Children.Add(anim);
                    board.Begin();
                }
            }

            SetBackButtonVisibility();
        }

        private void SetBackButtonVisibility()
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                DetailFrame.CanGoBack && CurrentState == MasterDetailState.Narrow
                ? AppViewBackButtonVisibility.Visible
                : AppViewBackButtonVisibility.Collapsed;
        }
        #region Initialize
        //public void Initialize(string frameId, BootStrapper.BackButton backButton, BootStrapper.ExistingContent existingContent)
        //{
        //    var service = WindowWrapper.Current().NavigationServices.GetByFrameId(frameId);
        //    if (service == null)
        //    {
        //        service = BootStrapper.Current.NavigationServiceFactory(backButton, existingContent);
        //        service.FrameFacade.FrameId = frameId;
        //    }

        //    NavigationService = service as NavigationService;
        //    DetailFrame = NavigationService.Frame;
        //    DetailFrame.Name = nameof(DetailFrame);
        //}
        public void Initialize(string frameId)
        {
            if (DetailFrame==null)
            {
                DetailFrame=new Frame();
                
            }
        }

        #endregion

        private void OnCurrentStateChanged(object sender, VisualStateChangedEventArgs e)
        {
            ViewStateChanged?.Invoke(this, EventArgs.Empty);

            if (CurrentState == MasterDetailState.Filled && (IsAnimated || IsMasterHidden))
            {
                IsMasterHidden = false;

                var anim = new DrillOutThemeAnimation
                {
                    EntranceTarget = MasterPresenter,
                    ExitTarget = new Border()
                };

                var board = new Storyboard();
                board.Children.Add(anim);
                board.Begin();
            }

            if (CurrentState == MasterDetailState.Narrow && DetailFrame.CurrentSourcePageType == BlankPageType)
            {
                DetailPresenter.Visibility = Visibility.Collapsed;
            }
            else
            {
                DetailPresenter.Visibility = Visibility.Visible;
            }

            SetBackButtonVisibility();
        }

        public MasterDetailState CurrentState
        {
            get
            {
                return AdaptiveStates.CurrentState?.Name == NarrowState ? MasterDetailState.Narrow : MasterDetailState.Filled;
            }
        }

        public event EventHandler ViewStateChanged;

        #region BlankType
        public Type BlankPageType
        {
            get { return (Type)GetValue(BlankPageTypeProperty); }
            set { SetValue(BlankPageTypeProperty, value); }
        }

        public static readonly DependencyProperty BlankPageTypeProperty =
            DependencyProperty.Register("BlankPageType", typeof(Type), typeof(MasterDetailView), new PropertyMetadata(typeof(BlankPage)));
        #endregion

        #region StrokeThickness
        public double StrokeThickness
        {
            get { return (double)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }

        public static readonly DependencyProperty StrokeThicknessProperty =
            DependencyProperty.Register("StrokeThickness", typeof(double), typeof(MasterDetailView), new PropertyMetadata(1d));
        #endregion

        #region IsAnimated
        public bool IsAnimated
        {
            get { return (bool)GetValue(IsAnimatedProperty); }
            set { SetValue(IsAnimatedProperty, value); }
        }

        public static readonly DependencyProperty IsAnimatedProperty =
            DependencyProperty.Register("IsAnimated", typeof(bool), typeof(MasterDetailView), new PropertyMetadata(true));
        #endregion
    }

    public enum MasterDetailState
    {
        Narrow,
        Filled
    }
}
