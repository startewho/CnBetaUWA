
using CnBetaUWA.ViewModels;
using MVVMSidekick.Views;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace CnBetaUWA
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TopicsPage : MVVMPage,IRefresh
    {

        public TopicsPage()
            : this(null)
        {

        }
        public TopicsPage(TopicsPage_Model model)
            : base(model)
        {
            this.InitializeComponent();
            this.RegisterPropertyChangedCallback(ViewModelProperty, (_, __) =>
            {
                StrongTypeViewModel = this.ViewModel as TopicsPage_Model;
            });
            StrongTypeViewModel = this.ViewModel as TopicsPage_Model;
        }


        public TopicsPage_Model StrongTypeViewModel
        {
            get { return (TopicsPage_Model)GetValue(StrongTypeViewModelProperty); }
            set { SetValue(StrongTypeViewModelProperty, value); }
        }

        public static readonly DependencyProperty StrongTypeViewModelProperty =
                    DependencyProperty.Register("StrongTypeViewModel", typeof(TopicsPage_Model), typeof(TopicsPage), new PropertyMetadata(null));




        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
        }

        public void Reresh()
        {
            StrongTypeViewModel.Reresh();
        }
    }
}
