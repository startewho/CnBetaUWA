
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
    public sealed partial class SettingTopicTypesMangePage : MVVMPage
    {

        public SettingTopicTypesMangePage()
            : this(null)
        {

        }
        public SettingTopicTypesMangePage(SettingTopicTypesMangePage_Model model)
            : base(model)
        {
            this.InitializeComponent();
            this.RegisterPropertyChangedCallback(ViewModelProperty, (_, __) =>
            {
                StrongTypeViewModel = this.ViewModel as SettingTopicTypesMangePage_Model;
            });
            StrongTypeViewModel = this.ViewModel as SettingTopicTypesMangePage_Model;
        }


        public SettingTopicTypesMangePage_Model StrongTypeViewModel
        {
            get { return (SettingTopicTypesMangePage_Model)GetValue(StrongTypeViewModelProperty); }
            set { SetValue(StrongTypeViewModelProperty, value); }
        }

        public static readonly DependencyProperty StrongTypeViewModelProperty =
                    DependencyProperty.Register("StrongTypeViewModel", typeof(SettingTopicTypesMangePage_Model), typeof(SettingTopicTypesMangePage), new PropertyMetadata(null));




        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
        }

    }
}
