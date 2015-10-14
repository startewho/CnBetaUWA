using System.Reactive;
using System.Reactive.Linq;
using MVVMSidekick.ViewModels;
using MVVMSidekick.Views;
using MVVMSidekick.Reactive;
using MVVMSidekick.Services;
using MVVMSidekick.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using CnBetaUWA.DataSource;
using CnBetaUWA.Helper;
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
            MenuItems.Add(new MenuItem { Icon = "\uE923", Title = "频道", PageType = typeof(LatestNewsPage) });
            MenuItems.Add(new MenuItem { Icon = "\uE779", Title = "专栏", PageType = typeof(LatestNewsPage) });
            MenuItems.Add(new MenuItem { Icon = "\uE713", Title = "设置", PageType = typeof(LatestNewsPage) });
            SelectedMenuItem = MenuItems.First(item => item.Title == "主页");

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
            //EventRouter.Instance.GetEventChannel<Object>()
            //    .Where(x => x.EventName == "NavToPostDetailByEventRouter")
            //    .Subscribe(
            //        async e =>
            //        {
            //            await TaskExHelper.Yield();
            //            var item = e.EventData as PostDetail;
            //            if (item != null)
            //            {
            //                await StageManager.DefaultStage.Show(new PostDetailPage_Model(item));

            //                //StageManager.DefaultStage.Frame.Navigate(typeof(PostDetailPage),item);
            //            }

            //        }
            //    ).DisposeWith(this);



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

