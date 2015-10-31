using System.Reactive.Linq;
using MVVMSidekick.ViewModels;
using MVVMSidekick.Views;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Windows.UI.Xaml;
using CnBetaUWA.Controls;
using CnBetaUWA.Extensions;
using CnBetaUWA.Models;
using MVVMSidekick.EventRouting;
using MVVMSidekick.Utilities;

namespace CnBetaUWA.ViewModels
{

    [DataContract]
    public class MainPage_Model : ViewModelBase<MainPage_Model>
    {
        // If you have install the code sniplets, use "propvm + [tab] +[tab]" create a property propcmd for command
        // 如果您已经安装了 MVVMSidekick 代码片段，请用 propvm +tab +tab 输入属性 propcmd 输入命令

        public MainPage_Model()
        {
            IsSplitViewPaneOpen = !IsSplitViewPaneOpen;

            _isLoaded = false;

            MenuItems.Add(new MenuItem { Icon = "\uE10F", Title = "主页", PageType = typeof(LatestNewsPage) });
            MenuItems.Add(new MenuItem { Icon = "\uE898", Title = "本月Top10", PageType = typeof(Top10Page) });
            MenuItems.Add(new MenuItem { Icon = "\uE163", Title = "今日排行", PageType = typeof(TodayRankPage) });
            MenuItems.Add(new MenuItem { Icon = "\uE923", Title = "频道", PageType = typeof(TopicsPage) });
          
            //MenuItems.Add(new MenuItem { Icon = "\uE779", Title = "专栏", PageType = typeof(LatestNewsPage) });
            //MenuItems.Add(new MenuItem { Icon = "\uE713", Title = "设置", PageType = typeof(LatestNewsPage) });
            SelectedMenuItem = MenuItems.First(item => item.Title == "今日排行");

        }


        public string Message
        {
            get { return _MessageLocator(this).Value; }
            set { _MessageLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property string Message Setup        
        protected Property<string> _Message = new Property<string> { LocatorFunc = _MessageLocator };
        static Func<BindableBase, ValueContainer<string>> _MessageLocator = RegisterContainerLocator<string>("Message", model => model.Initialize("Message", ref model._Message, ref _MessageLocator, _MessageDefaultValueFactory));
        static Func<string> _MessageDefaultValueFactory = () => default(string);
        #endregion



        private bool _isLoaded;


        protected override Task OnBindedViewLoad(IView view)
        {
            if (_isLoaded) return base.OnBindedViewLoad(view);

            SubscribeCommand();
            _isLoaded = true;
            return base.OnBindedViewLoad(view);
        }

        private void SubscribeCommand()
        {
            EventRouter.Instance.GetEventChannel<Object>()
                .Where(x => x.EventName == "NavToNewsDetailByEventRouter")
                .Subscribe(
                    async e =>
                    {
                        await TaskExHelper.Yield();
                        var pageviewer = e.Sender as FrameworkElement;
                        var masterdetail = pageviewer.GetFirstAncestorOfType<MasterDetailView>();
                        var news = e.EventData as News;
                        if (news != null)
                        {
                            masterdetail?.DetailFrameNavigateTo(typeof (NewsPage), new NewsPage_Model(news), true);
                            // var item=new NewsPage_Model(value);
                            //await StageManager.DefaultStage.Show(item);
                            //StageManager.DefaultStage.Frame.Navigate(typeof(PostDetailPage),item);
                        }

                    }
                ).DisposeWith(this);


            EventRouter.Instance.GetEventChannel<Object>()
               .Where(x => x.EventName == "ToastMessageByEventRouter")
               .Subscribe(
                   async e =>
                   {
                       var message = e.EventData as string;
                       var mainpage = this.StageManager.CurrentBindingView as MainPage;
                       var notifbar = mainpage.GetFirstDescendantOfType<NotificationBar>();
                       notifbar.ShowMessage(message);
                       if (message!=string.Empty)
                       {
                           Message = message;
                       }
                       await TaskExHelper.Yield();
                     

                   }
               ).DisposeWith(this);

            //EventRouter.Instance.GetEventChannel<Object>()
            //      .Where(x => x.EventName == "NavToAuthorDetailByEventRouter")
            //      .Subscribe(
            //          async e =>
            //          {
            //              await TaskExHelper.Yield();
            //              var item = e.EventData as Author;
            //              if (item != null)
            //              {
            //                  item.AuthorPostList = new IncrementalLoadingCollection<AuthorPostSource, PostDetail>(item.Id.ToString(), AppStrings.PageSize);
            //                  await StageManager.DefaultStage.Show(new AuthorPage_Model(item));

            //                //StageManager.DefaultStage.Frame.Navigate(typeof(PostDetailPage),item);
            //            }

            //          }
            //      ).DisposeWith(this);


        }



        private ObservableCollection<MenuItem> menuItems = new ObservableCollection<MenuItem>();



        public bool IsSplitViewPaneOpen

        {
            get { return _IsSplitViewPaneOpenLocator(this).Value; }
            set { _IsSplitViewPaneOpenLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property bool IsSplitViewPaneOpen Setup        
        protected Property<bool> _IsSplitViewPaneOpen = new Property<bool> { LocatorFunc = _IsSplitViewPaneOpenLocator };
        static Func<BindableBase, ValueContainer<bool>> _IsSplitViewPaneOpenLocator = RegisterContainerLocator("IsSplitViewPaneOpen", model => model.Initialize("IsSplitViewPaneOpen", ref model._IsSplitViewPaneOpen, ref _IsSplitViewPaneOpenLocator, _IsSplitViewPaneOpenDefaultValueFactory));
        static Func<bool> _IsSplitViewPaneOpenDefaultValueFactory = () => default(bool);
        #endregion


        public MenuItem SelectedMenuItem
        {
            get { return _SelectedMenuItemLocator(this).Value; }
            set
            {
                if (value != null)
                {
                    _SelectedMenuItemLocator(this).SetValueAndTryNotify(value);
                    _IsSplitViewPaneOpenLocator(this).SetValueAndTryNotify(false);
                    _SelectedPageTypeLocator(this).SetValueAndTryNotify(value.PageType);
                }



            }
        }
        #region Property MenuItem SelectedMenuItem Setup        
        protected Property<MenuItem> _SelectedMenuItem = new Property<MenuItem> { LocatorFunc = _SelectedMenuItemLocator };
        static Func<BindableBase, ValueContainer<MenuItem>> _SelectedMenuItemLocator = RegisterContainerLocator("SelectedMenuItem", model => model.Initialize("SelectedMenuItem", ref model._SelectedMenuItem, ref _SelectedMenuItemLocator, _SelectedMenuItemDefaultValueFactory));
        static Func<MenuItem> _SelectedMenuItemDefaultValueFactory = () => default(MenuItem);
        #endregion


        public Type SelectedPageType
        {
            get
            {
                if (SelectedMenuItem != null)
                {
                    return SelectedMenuItem.PageType;
                }
                return null;
            }
            set
            {


                SelectedMenuItem = menuItems.FirstOrDefault(m => m.PageType == value);

            }
        }
        #region Property Type SelectedPageType Setup        
        protected Property<Type> _SelectedPageType = new Property<Type> { LocatorFunc = _SelectedPageTypeLocator };
        static Func<BindableBase, ValueContainer<Type>> _SelectedPageTypeLocator = RegisterContainerLocator("SelectedPageType", model => model.Initialize("SelectedPageType", ref model._SelectedPageType, ref _SelectedPageTypeLocator, _SelectedPageTypeDefaultValueFactory));
        static Func<Type> _SelectedPageTypeDefaultValueFactory = () => default(Type);
        #endregion


        public ObservableCollection<MenuItem> MenuItems
        {
            get { return menuItems; }
        }


    }

}

