﻿using System;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using CnBetaUWA.Extensions;

namespace CnBetaUWA.Controls
{
    [TemplatePart(Name = "MasterPresenter", Type = typeof(ContentPresenter))]
    [TemplatePart(Name = "DetailPresenter", Type = typeof(Border))]
    [TemplatePart(Name = "DetailFrame", Type = typeof(Frame))]
    [TemplateVisualState(GroupName = "AdaptiveStates", Name = "NarrowState")]
    [TemplateVisualState(GroupName = "AdaptiveStates", Name = "FilledState")]
    public sealed class MasterDetailView : ContentControl
    {
        private ContentPresenter MasterPresenter;
        private Border DetailPresenter;
        private VisualStateGroup AdaptiveStates;
        private bool IsMasterHidden;
        private const string NarrowState = "NarrowState";
        private static Frame DetailFrame;

        private ExtendSplitView _extendSplitView;

        private bool _firestLoad;


        public MasterDetailView()
        {
            DefaultStyleKey = typeof(MasterDetailView);
            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
        }

        #region KeyActivated

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
            MasterPresenter = (ContentPresenter) GetTemplateChild("MasterFrame");
            DetailPresenter = (Border) GetTemplateChild("DetailPresenter");
            AdaptiveStates = (VisualStateGroup) GetTemplateChild("AdaptiveStates");
            AdaptiveStates.CurrentStateChanged += OnCurrentStateChanged;
            DetailFrame = GetTemplateChild("DetailFrame") as Frame;

            if (DetailFrame == null) return;

            DetailFrame.Name = "DetailFrame";

            DetailPresenter.Child = DetailFrame;
            DetailFrame.Navigated += OnNavigated;
            
            if (DetailFrame.CurrentSourcePageType == null)
            {
                DetailFrame.Navigate(BlankPageType);
            }
            //else
            //{
            //    DetailFrame.BackStack.Insert(0, new PageStackEntry(BlankPageType, null, null));
            //}

            _extendSplitView= this.GetFirstAncestorOfType<ExtendSplitView>();

        }

        #endregion


        private void OnNavigated(object sender, NavigationEventArgs e)
        {
           

            switch (CurrentState)
            {
                case MasterDetailState.Narrow:
                    if (e.SourcePageType != BlankPageType)
                    {
                        switch (e.NavigationMode)
                        {
                            case NavigationMode.New:
                                IsMasterHidden = false;
                                MasterPresenterVisibility(IsMasterHidden);
                                DetailPresenterVisibility(!IsMasterHidden);
                                SetAnimation();
                                if (_extendSplitView != null)
                                {
                                    _extendSplitView.IsOpenBottomPane = false;
                                }
                                break;
                            case NavigationMode.Back:
                                IsMasterHidden = false;
                                DetailPresenterVisibility(true);
                                MasterPresenterVisibility(false);
                                SetAnimation();
                                break;
                            case NavigationMode.Forward:
                                break;
                            case NavigationMode.Refresh:
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                    else
                    {
                        IsMasterHidden = true;
                        MasterPresenterVisibility(IsMasterHidden);
                        DetailPresenterVisibility(!IsMasterHidden);
                        switch (DetailFrame.BackStackDepth)
                        {
                            case 0:
                                if (_extendSplitView != null)
                                {
                                    _extendSplitView.IsOpenBottomPane = true;
                                }
                                break;
                        }
                    }
                    break;


                case MasterDetailState.Filled:

                    IsMasterHidden = false;
                    MasterPresenterVisibility(true);
                    DetailPresenterVisibility(true);
                    SetAnimation();
                    break;
            }

            SetBackButtonVisibility();

            if (!_firestLoad)
            {
                SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;
                _firestLoad = true;
            }
        }


        private void SetAnimation()
        {
            var anim = new DrillInThemeAnimation
            {
                EntranceTarget = DetailPresenter,
                ExitTarget = new Border()
            };

            var board = new Storyboard();
            board.Children.Add(anim);
            board.Begin();
        }

        private void SetBackButtonVisibility()
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = DetailFrame.CanGoBack && CurrentState == MasterDetailState.Narrow ? AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;
        }


        private void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            if (CurrentState == MasterDetailState.Narrow && DetailFrame.CanGoBack)
            {
                DetailFrame.GoBack();
                e.Handled = true;
            }
            if (IsMasterHidden)
            {
                MasterPresenter.Visibility = Visibility.Visible;
            }
        }


        private void OnCurrentStateChanged(object sender, VisualStateChangedEventArgs e)
        {
            ViewStateChanged?.Invoke(this, EventArgs.Empty);
            
            switch (CurrentState)
            {
                case MasterDetailState.Narrow:
                  
                    if (DetailFrame.CurrentSourcePageType != BlankPageType)
                    {
                        DetailPresenter.Visibility = Visibility.Visible;
                        IsMasterHidden = true;
                        MasterPresenter.Visibility = Visibility.Collapsed;
                        _extendSplitView.IsOpenBottomPane = false;
                    }
                    else
                    {
                        DetailPresenter.Visibility = Visibility.Collapsed;
                        IsMasterHidden = false;
                        MasterPresenter.Visibility = Visibility.Visible;
                        _extendSplitView.IsOpenBottomPane = true;
                    }
                    break;
                case MasterDetailState.Filled:
                    IsMasterHidden = false;
                    MasterPresenterVisibility(true);
                    DetailPresenterVisibility(true);
                    SetAnimation();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }


            SetBackButtonVisibility();
        }

        private void MasterPresenterVisibility(bool visible)
        {
            MasterPresenter.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
        }

        private void DetailPresenterVisibility(bool visible)
        {
            DetailPresenter.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
        }

        public void DetailFrameNavigateTo(Type pagType, object param, bool clearFrame)
        {
            DetailFrame.Navigate(pagType, param);

            if (clearFrame && DetailFrame.BackStackDepth > 1)
            {
                DetailFrame.BackStack.RemoveAt(1);
            }
        }

        public MasterDetailState CurrentState
        {
            get { return AdaptiveStates.CurrentState?.Name == NarrowState ? MasterDetailState.Narrow : MasterDetailState.Filled; }
        }

        public event EventHandler ViewStateChanged;


        public Type PageType
        {
            get
            {
                if (PageTypeProperty != null)
                {
                    return (Type) GetValue(PageTypeProperty);
                }
                return null;
            }
            set
            {
                if (value != null)
                {
                    SetValue(PageTypeProperty, value);
                }
            }
        }

        // Using a DependencyProperty as the backing store for PageType.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PageTypeProperty = DependencyProperty.Register("PageType", typeof (Type), typeof (Page), new PropertyMetadata(null, OnPageTypeChanged));

        private static void OnPageTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var pagetype = e.NewValue as Type;
            if (pagetype != null && DetailFrame != null)
            {
                DetailFrame.SourcePageType = pagetype;
            }
        }

        #region BlankType

        public Type BlankPageType
        {
            get { return (Type) GetValue(BlankPageTypeProperty); }
            set { SetValue(BlankPageTypeProperty, value); }
        }

        public static readonly DependencyProperty BlankPageTypeProperty = DependencyProperty.Register("BlankPageType", typeof (Type), typeof (MasterDetailView), new PropertyMetadata(typeof (BlankPage)));

        #endregion

        #region StrokeThickness

        public double StrokeThickness
        {
            get { return (double) GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }

        public static readonly DependencyProperty StrokeThicknessProperty = DependencyProperty.Register("StrokeThickness", typeof (double), typeof (MasterDetailView), new PropertyMetadata(1d));

        #endregion

        #region IsAnimated

        public bool IsAnimated
        {
            get { return (bool) GetValue(IsAnimatedProperty); }
            set { SetValue(IsAnimatedProperty, value); }
        }

        public static readonly DependencyProperty IsAnimatedProperty = DependencyProperty.Register("IsAnimated", typeof (bool), typeof (MasterDetailView), new PropertyMetadata(true));

        #endregion
    }

    public enum MasterDetailState
    {
        Narrow,
        Filled
    }
}
