
using CnBetaUWA.ViewModels;
using MVVMSidekick.Views;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace CnBetaUWA
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LatestNewsPage : MVVMPage
    {
       
        public LatestNewsPage()
            : this(null)
        {

        }
        public LatestNewsPage(LatestNewsPage_Model model)
            : base(model)
        {
            this.InitializeComponent();

            this.RegisterPropertyChangedCallback(ViewModelProperty, (_, __) =>
            {
                StrongTypeViewModel = this.ViewModel as LatestNewsPage_Model;
            });
            StrongTypeViewModel = this.ViewModel as LatestNewsPage_Model;

          

        }


        public LatestNewsPage_Model StrongTypeViewModel
        {
            get { return (LatestNewsPage_Model)GetValue(StrongTypeViewModelProperty); }
            set { SetValue(StrongTypeViewModelProperty, value); }
        }

        public static readonly DependencyProperty StrongTypeViewModelProperty =
                    DependencyProperty.Register("StrongTypeViewModel", typeof(LatestNewsPage_Model), typeof(LatestNewsPage), new PropertyMetadata(null));




        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            
            //if (MasterDetail.DetailFrame == null)
            //{
            //    MasterDetail.Initialize("Main");
            //    DetailFrame = MasterDetail.DetailFrame;
            //   // DetailFrame.Navigated += DetailFrame_Navigated;

            //}

            //SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

        }


        //private void OnBackRequested(object sender, BackRequestedEventArgs e)
        //{
        //    if (MasterDetail.CurrentState == MasterDetailState.Narrow && DetailFrame.CanGoBack)
        //    {
        //        DetailFrame.GoBack();
        //        e.Handled = true;
        //    }
        //}


        private void LatestNewsPage_OnUnloaded(object sender, RoutedEventArgs e)
        {
           
        }

        //private void ListViewBase_OnContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
        //{
        //    var itemcount = sender.Items.Count;
        //    for (int i = itemcount-1; i > 0; i--)
        //    {
        //        var container = sender.ContainerFromIndex(i);
        //        if (container != null)
        //        {
        //            var listviewitem = sender.ContainerFromIndex(i+1);
        //        }
        //    }
        //}
    }
}
