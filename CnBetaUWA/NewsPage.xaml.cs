
using System;
using CnBetaUWA.ViewModels;
using MVVMSidekick.Views;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using CnBetaUWA.Extensions;
using MyToolkit.Controls;
using Q42.WinRT.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace CnBetaUWA
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NewsPage : MVVMPage
    {

        public NewsPage()
            : this(null)
        {

        }
        public NewsPage(NewsPage_Model model)
            : base(model)
        {
            this.InitializeComponent();
            this.RegisterPropertyChangedCallback(ViewModelProperty, (_, __) =>
            {
                StrongTypeViewModel = this.ViewModel as NewsPage_Model;
            });
            StrongTypeViewModel = this.ViewModel as NewsPage_Model;
        }


        public NewsPage_Model StrongTypeViewModel
        {
            get { return (NewsPage_Model)GetValue(StrongTypeViewModelProperty); }
            set { SetValue(StrongTypeViewModelProperty, value); }
        }

        public static readonly DependencyProperty StrongTypeViewModelProperty =
                    DependencyProperty.Register("StrongTypeViewModel", typeof(NewsPage_Model), typeof(NewsPage), new PropertyMetadata(null));




        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            StrongTypeViewModel= e.Parameter as NewsPage_Model;
            DataContext = StrongTypeViewModel;
            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
        }


        private void Webview_OnHtmlLoaded(object sender, EventArgs e)
        {
            var htmlcontrol = sender as HtmlControl;
            var imglist = htmlcontrol.GetDescendantsOfType<Image>();
            foreach (var image in imglist)
            {
                var imaguri = ImageExtensions.GetCacheUri(image);
                var newimg = new Image {Source = image.Source,Tag = image.Tag};
                ImageExtensions.SetCacheUri(newimg, imaguri);
                flipView.Items?.Add(newimg);
                image.Tapped += Image_Tapped;
            }
        }

        private void Image_Tapped(object sender, TappedRoutedEventArgs e)
        {
            webview.Opacity = 0.5;
            var img = sender as Image;
            if (img != null) flipView.SelectedIndex =(int)img.Tag;
            flipView.Visibility = Visibility.Visible;
            flipView.Opacity =1;
            e.Handled = true;
        }

        private void FlipView_OnTapped(object sender, TappedRoutedEventArgs e)
        {

            flipView.SelectedIndex = -1;
            flipView.Opacity = 1;
            flipView.Visibility = Visibility.Collapsed;
            webview.Opacity = 1;
            e.Handled = true;
        }
    }
}
