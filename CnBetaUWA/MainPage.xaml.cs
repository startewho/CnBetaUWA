using MVVMSidekick.Views;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using CnBetaUWA.Extensions;
using CnBetaUWA.ViewModels;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace CnBetaUWA
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : MVVMPage
    {
        public MainPage()
        {
            this.InitializeComponent();
            this.RegisterPropertyChangedCallback(ViewModelProperty, (_, __) =>
            {
                StrongTypeViewModel = this.ViewModel as MainPage_Model;
            });
            StrongTypeViewModel = this.ViewModel as MainPage_Model;

            var transitions = new TransitionCollection();
            var transition = new NavigationThemeTransition();
            transitions.Add(transition);
            VMFrame.ContentTransitions = transitions;
        }


    public MainPage_Model StrongTypeViewModel
        {
            get { return (MainPage_Model)GetValue(StrongTypeViewModelProperty); }
            set { SetValue(StrongTypeViewModelProperty, value); }
        }

        public static readonly DependencyProperty StrongTypeViewModelProperty =
                    DependencyProperty.Register("StrongTypeViewModel", typeof(MainPage_Model), typeof(MainPage), new PropertyMetadata(null));

      

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
        }


        

        private void FrameworkElement_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            var listview = sender as ListBox;
            if (listview == null) return;

            var scrollviewr = listview.GetFirstDescendantOfType<ScrollViewer>();
            if (scrollviewr!=null)
            {
                scrollviewr.HorizontalScrollMode = ScrollMode.Disabled;
                scrollviewr.VerticalScrollMode = ScrollMode.Disabled;

            }
            var panel = (VirtualizingStackPanel)(listview.ItemsPanelRoot);

            if (panel == null) throw new ArgumentNullException(nameof(panel));

            var width = e.NewSize.Width;

            var items = panel.Children;
            foreach (var item in  items)
            {
                var listBoxItem = item as ListBoxItem;
                if (listBoxItem != null) listBoxItem.Width = width/items.Count;
            }
            //panel.it = width/5;


        }


        private void MenuItem_OnDoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            var view = VMFrame.Content as IRefresh;
            view?.Reresh();
        }
    }
}
