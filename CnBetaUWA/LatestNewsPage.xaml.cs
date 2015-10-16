
using CnBetaUWA.ViewModels;
using System.Reactive;
using System.Reactive.Linq;
using MVVMSidekick.ViewModels;
using MVVMSidekick.Views;
using MVVMSidekick.Reactive;
using MVVMSidekick.Services;
using MVVMSidekick.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using CnBetaUWA.Controls;

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

      
    }
}
