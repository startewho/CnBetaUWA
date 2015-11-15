using MVVMSidekick.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using CnBetaUWA.Controls;
using CnBetaUWA.Extensions;
using CnBetaUWA.Models;
using MVVMSidekick.EventRouting;
using MVVMSidekick.Utilities;
using MVVMSidekick.Views;

namespace CnBetaUWA.ViewModels
{

    [DataContract]
    public class SettingPage_Model : ViewModelBase<SettingPage_Model>
    {

        public SettingPage_Model()
        {
            MenuItems.Add(new MenuItem { Icon = "\uE175", Title = "频道设置", PageType = typeof(SettingTopicTypesMangePage) });
            MenuItems.Add(new MenuItem { Icon = "\uE748", Title = "主题设置", PageType = typeof(SettingThemePage) });
            MenuItems.Add(new MenuItem { Icon = "\uE208", Title = "收藏管理", PageType = typeof(SettingBookmarketPage) });
            MenuItems.Add(new MenuItem { Icon = "\uE74E", Title = "缓存管理", PageType = typeof(SettingCachePage) });
            MenuItems.Add(new MenuItem { Icon = "\uE730", Title = "关于软件", PageType = typeof(SettingAboutPage) });
        }
        // If you have install the code sniplets, use "propvm + [tab] +[tab]" create a property。
        // 如果您已经安装了 MVVMSidekick 代码片段，请用 propvm +tab +tab 输入属性
        private ObservableCollection<MenuItem> menuItems = new ObservableCollection<MenuItem>();
        public String Title
        {
            get { return _TitleLocator(this).Value; }
            set { _TitleLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property String Title Setup
        protected Property<String> _Title = new Property<String> { LocatorFunc = _TitleLocator };
        static Func<BindableBase, ValueContainer<String>> _TitleLocator = RegisterContainerLocator<String>("Title", model => model.Initialize("Title", ref model._Title, ref _TitleLocator, _TitleDefaultValueFactory));
        static Func<BindableBase, String> _TitleDefaultValueFactory = m => m.GetType().Name;
        #endregion

      

        public ObservableCollection<MenuItem> MenuItems
        {
            get { return menuItems; }
        }
    
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
                .Where(x => x.EventName == "NavToSettingDetailByEventRouter")
                .Subscribe(
                    async e =>
                    {
                        await TaskExHelper.Yield();
                        var pageviewer = e.Sender as FrameworkElement;
                        var masterdetail = pageviewer.GetFirstAncestorOfType<MasterDetailView>();
                        var menuitem = e.EventData as MenuItem;
                        if (menuitem != null)
                        {
                            masterdetail?.DetailFrameNavigateTo(menuitem.PageType, null, false);
                            // var item=new NewsPage_Model(value);
                            //await StageManager.DefaultStage.Show(item);
                            //StageManager.DefaultStage.Frame.Navigate(typeof(PostDetailPage),item);
                        }

                    }
                ).DisposeWith(this);
        }

    }

    }

