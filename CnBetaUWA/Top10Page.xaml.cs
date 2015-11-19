
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
    public sealed partial class Top10Page : MVVMPage,IRefresh
    {

        public Top10Page()
            : this(null)
        {

        }
        public Top10Page(Top10Page_Model model)
            : base(model)
        {
            this.InitializeComponent();
            this.RegisterPropertyChangedCallback(ViewModelProperty, (_, __) =>
            {
                StrongTypeViewModel = this.ViewModel as Top10Page_Model;
            });
            StrongTypeViewModel = this.ViewModel as Top10Page_Model;
        }


        public Top10Page_Model StrongTypeViewModel
        {
            get { return (Top10Page_Model)GetValue(StrongTypeViewModelProperty); }
            set { SetValue(StrongTypeViewModelProperty, value); }
        }

        public static readonly DependencyProperty StrongTypeViewModelProperty =
                    DependencyProperty.Register("StrongTypeViewModel", typeof(Top10Page_Model), typeof(Top10Page), new PropertyMetadata(null));




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
           StrongTypeViewModel.CommandRereshDataSourceCollection.Execute(null);
        }
    }
}
