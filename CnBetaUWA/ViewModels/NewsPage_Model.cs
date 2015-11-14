using System;
using System.Linq;
using System.Reactive.Linq;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;
using CnBetaUWA.DataBase;
using CnBetaUWA.DataSource;
using CnBetaUWA.Extensions;
using CnBetaUWA.Helper;
using CnBetaUWA.Models;
using MVVMSidekick.Reactive;
using MVVMSidekick.Utilities;
using MVVMSidekick.ViewModels;
using MVVMSidekick.Views;
using MyToolkit.Controls;
using Q42.WinRT.Storage;

namespace CnBetaUWA.ViewModels
{

    [DataContract]
    public class NewsPage_Model : ViewModelBase<NewsPage_Model>
    {
        // If you have install the code sniplets, use "propvm + [tab] +[tab]" create a property。
        // 如果您已经安装了 MVVMSidekick 代码片段，请用 propvm +tab +tab 输入属性

        private bool _isLoaded;
        public NewsPage_Model()
        {

        }
        public NewsPage_Model(News model)
        {
            NewsVm = model;
         
        }

        public News NewsVm { get; set; }

        private void InitComentsData()
        {
            CommentsSource = new IncrementalPageLoadingCollection<IncrementalNewsCommentPageSource, NewsComment>(NewsVm.Sid.ToString(), 1, 10);
            CommentsSource.OnLoadMoreCompleted += CommentsSource_OnLoadMoreCompleted;
          
        }

        private void CommentsSource_OnLoadMoreCompleted(int count)
        {
            //string message = string.Format("已经加载了{0}条评论", count);
            //MessageHelper.ShowMessage(this,message);

           // var list = CommentsSource;
            if (CommentsSource == null || !CommentsSource.Any()) return;
            foreach (var comment in CommentsSource)
            {
                if (!comment.IsShow) continue;
                var corentitem = CommentsSource.FirstOrDefault(item => item.Tid == comment.Pid);
                if (corentitem == null) continue;
                var parrentcomment = new NewsComment
                {
                    UserName = corentitem.UserName,
                    Content = corentitem.Content,
                };
                comment.PidComment = parrentcomment;
            }
        }

        private async void GetContent(int sid)
        {
            var nightmode = SettingsHelper.Get<bool>(CnBetaHelper.SettingNightMode);

            SetNightMode(nightmode);

            var querynews=NewsDataTable.Query(sid);
            if (querynews?.Content != null)
            {
                TotalContent = querynews.Content;
                IsBookmarketed = querynews.IsBookmarketed;
            }

            else
            {
                var content = await CnBetaHelper.GetNewsContent(sid);

                if (content == null) return;
                NewsContent = ModelHelper.JsonToNewsContent(content);
                var html = await IOHelper.GetTextFromStorage(new Uri("ms-appx:///AppData/ContentTemplate.html"));
                html = html.Replace("#Date", NewsVm.CreatTime);
                html = html.Replace("#Source", NewsContent.Source);
                html = html.Replace("#Author", NewsContent.Author);
                //  html = html.Replace("#Topic", NewsVm.TopictLogoPicture);
                html = html.Replace("HomeText", NewsContent.HomeText);
                html = html.Replace("BodyText", NewsContent.BodyText);
                html = Regex.Replace(html, "(<a.+?)(<img[^>]+>)((</a>)+)", "$2");
                // html = html.Replace("http://static.cnbetacdn.com/", "");
                TotalContent = html;
                NewsVm.Content = html;
                IsBookmarketed = false;
                NewsDataTable.Add(NewsVm);
            }
    
        }
        protected override Task OnBindedViewLoad(IView view)
        {
            if (_isLoaded) return base.OnBindedViewLoad(view);
            GetContent(NewsVm.Sid);
            SubscribeCommand();
            _isLoaded = true;
            return base.OnBindedViewLoad(view);
          
        }

        protected override Task OnBindedViewUnload(IView view)
        {
            SettingsHelper.Set(CnBetaHelper.SettingNightMode, IsNightMode);
            NewsDataTable.Add(NewsVm);
            NewsVm = null;
            TotalContent = null;
            LocalEventRouter = null;
           // CommentsSource.OnLoadMoreCompleted -= CommentsSource_OnLoadMoreCompleted;
            return base.OnBindedViewUnload(view);
        }

        private void SubscribeCommand()
        {

            LocalEventRouter.GetEventChannel<Object>()
              .Where(x => x.EventName == "SupportCommentByEventRouter")
              .Subscribe(
                  async e =>
                  {
                      var comment = e.EventData as NewsComment;
                     
                      if (comment != null && comment.IsSupported != true)
                      {
                          var result = await CnBetaHelper.GetCommentAction(comment.Tid, 1, "support");
                          var susses = ModelHelper.JsonToCommentAction(result);
                          if (susses)
                          {
                              comment.Support += 1;
                              comment.IsSupported = true;
                          }
                         
                      }

                      await TaskExHelper.Yield();

                  }
              ).DisposeWith(this);


            LocalEventRouter.GetEventChannel<Object>()
             .Where(x => x.EventName == "AgainstCommentByEventRouter")
             .Subscribe(
                 async e =>
                 {
                     var comment = e.EventData as NewsComment;
                     if (comment != null && comment.IsAgainsted !=true)
                     {
                         var result = await CnBetaHelper.GetCommentAction(NewsVm.Sid, comment.Tid, "against");
                         var susses = ModelHelper.JsonToCommentAction(result);
                         if (susses)
                         {
                             comment.Against += 1;
                             comment.IsAgainsted = true;
                         }
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

        public IncrementalPageLoadingCollection<IncrementalNewsCommentPageSource,NewsComment> CommentsSource
        {
            get { return _CommentsSourceLocator(this).Value; }
            set { _CommentsSourceLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property IncrementalPageLoadingCollection<IncrementalNewsCommentPageSource,NewsComment> CommentsSource Setup        
        protected Property<IncrementalPageLoadingCollection<IncrementalNewsCommentPageSource,NewsComment>> _CommentsSource = new Property<IncrementalPageLoadingCollection<IncrementalNewsCommentPageSource,NewsComment>> { LocatorFunc = _CommentsSourceLocator };
        static Func<BindableBase, ValueContainer<IncrementalPageLoadingCollection<IncrementalNewsCommentPageSource,NewsComment>>> _CommentsSourceLocator = RegisterContainerLocator<IncrementalPageLoadingCollection<IncrementalNewsCommentPageSource,NewsComment>>("CommentsSource", model => model.Initialize("CommentsSource", ref model._CommentsSource, ref _CommentsSourceLocator, _CommentsSourceDefaultValueFactory));
        static Func<IncrementalPageLoadingCollection<IncrementalNewsCommentPageSource,NewsComment>> _CommentsSourceDefaultValueFactory = () => default(IncrementalPageLoadingCollection<IncrementalNewsCommentPageSource,NewsComment>);
        #endregion



        public NewsContent NewsContent
        {
            get { return _NewsContentLocator(this).Value; }
            set { _NewsContentLocator(this).SetValueAndTryNotify(value); }
        }


        #region Property NewsContent NewsContent Setup        
        protected Property<NewsContent> _NewsContent = new Property<NewsContent> { LocatorFunc = _NewsContentLocator };
        static Func<BindableBase, ValueContainer<NewsContent>> _NewsContentLocator = RegisterContainerLocator<NewsContent>("NewsContent", model => model.Initialize("NewsContent", ref model._NewsContent, ref _NewsContentLocator, _NewsContentDefaultValueFactory));
        static Func<NewsContent> _NewsContentDefaultValueFactory = () => default(NewsContent);
        #endregion


        public string TotalContent
        {
            get { return _TotalContentLocator(this).Value; }
            set { _TotalContentLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property string TotalContent Setup        
        protected Property<string> _TotalContent = new Property<string> { LocatorFunc = _TotalContentLocator };
        static Func<BindableBase, ValueContainer<string>> _TotalContentLocator = RegisterContainerLocator("TotalContent", model => model.Initialize("TotalContent", ref model._TotalContent, ref _TotalContentLocator, _TotalContentDefaultValueFactory));
        static Func<string> _TotalContentDefaultValueFactory = () => default(string);
        #endregion

        
        public bool IsCommentPanelOpen
        {
            get { return _IsCommentPanelOpenLocator(this).Value; }
            set { _IsCommentPanelOpenLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property bool IsCommentPanelOpen Setup        
        protected Property<bool> _IsCommentPanelOpen = new Property<bool> { LocatorFunc = _IsCommentPanelOpenLocator };
        static Func<BindableBase, ValueContainer<bool>> _IsCommentPanelOpenLocator = RegisterContainerLocator<bool>("IsCommentPanelOpen", model => model.Initialize("IsCommentPanelOpen", ref model._IsCommentPanelOpen, ref _IsCommentPanelOpenLocator, _IsCommentPanelOpenDefaultValueFactory));
        static Func<bool> _IsCommentPanelOpenDefaultValueFactory = () => default(bool);
        #endregion


        public bool IsBookmarketed
        {
            get { return _IsBookmarketedLocator(this).Value; }
            set
            {
                _IsBookmarketedLocator(this).SetValueAndTryNotify(value);
                NewsVm.IsBookmarketed = value;
            }
        }
        #region Property bool IsBookmarketed Setup        
        protected Property<bool> _IsBookmarketed = new Property<bool> { LocatorFunc = _IsBookmarketedLocator };
        static Func<BindableBase, ValueContainer<bool>> _IsBookmarketedLocator = RegisterContainerLocator<bool>("IsBookmarketed", model => model.Initialize("IsBookmarketed", ref model._IsBookmarketed, ref _IsBookmarketedLocator, _IsBookmarketedDefaultValueFactory));
        static Func<bool> _IsBookmarketedDefaultValueFactory = () => default(bool);
        #endregion

        
        public bool IsNightMode
        {
            get { return _IsNightModeLocator(this).Value; }
            set
            {
                _IsNightModeLocator(this).SetValueAndTryNotify(value);
                BackGroundColor = value ? Colors.OldLace : Colors.WhiteSmoke;

            }
        }
        #region Property bool IsNightMode Setup        
        protected Property<bool> _IsNightMode = new Property<bool> { LocatorFunc = _IsNightModeLocator };
        static Func<BindableBase, ValueContainer<bool>> _IsNightModeLocator = RegisterContainerLocator<bool>("IsNightMode", model => model.Initialize("IsNightMode", ref model._IsNightMode, ref _IsNightModeLocator, _IsNightModeDefaultValueFactory));
        static Func<bool> _IsNightModeDefaultValueFactory = () => default(bool);
        #endregion

        public CommandModel<ReactiveCommand, String> CommandChangeWebViewFontSize
        {
            get { return _CommandChangeWebViewFontSizeLocator(this).Value; }
            set { _CommandChangeWebViewFontSizeLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property CommandModel<ReactiveCommand, String> CommandChangeWebViewFontSize Setup        

        protected Property<CommandModel<ReactiveCommand, String>> _CommandChangeWebViewFontSize = new Property<CommandModel<ReactiveCommand, String>> { LocatorFunc = _CommandChangeWebViewFontSizeLocator };
        static Func<BindableBase, ValueContainer<CommandModel<ReactiveCommand, String>>> _CommandChangeWebViewFontSizeLocator = RegisterContainerLocator<CommandModel<ReactiveCommand, String>>("CommandChangeWebViewFontSize", model => model.Initialize("CommandChangeWebViewFontSize", ref model._CommandChangeWebViewFontSize, ref _CommandChangeWebViewFontSizeLocator, _CommandChangeWebViewFontSizeDefaultValueFactory));
        static Func<BindableBase, CommandModel<ReactiveCommand, String>> _CommandChangeWebViewFontSizeDefaultValueFactory =
            model =>
            {
                var resource = "CommandChangeWebViewFontSize";           // Command resource  
                var commandId = "CommandChangeWebViewFontSize";
                var vm = CastToCurrentType(model);
                var cmd = new ReactiveCommand(canExecute: true) { ViewModel = model }; //New Command Core

                cmd.DoExecuteUIBusyTask(
                        vm,
                        async e =>
                        {
                            var view = vm.StageManager.CurrentBindingView as NewsPage;
                            var webview = view.GetFirstDescendantOfType<HtmlControl>();

                            var args = e.EventArgs.Parameter as RangeBaseValueChangedEventArgs;
                            if (args!=null)
                            {
                                //await webview.InvokeScriptAsync("changeFontSize", new[] { string.Format("{0:0%}", args.NewValue * 0.5) });
                                webview.FontSize = 16*args.NewValue*0.5;
                            }
                            
                            //Todo: Add ChangeWebViewFontSize logic here, or
                            await TaskExHelper.Yield();
                        })
                    .DoNotifyDefaultEventRouter(vm, commandId)
                    .Subscribe()
                    .DisposeWith(vm);

                var cmdmdl = cmd.CreateCommandModel(resource);

                cmdmdl.ListenToIsUIBusy(
                    model: vm,
                    canExecuteWhenBusy: false);
                return cmdmdl;
            };

        #endregion



        public CommandModel<ReactiveCommand, String> CommandDetailBack
        {
            get { return _CommandDetailBackLocator(this).Value; }
            set { _CommandDetailBackLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property CommandModel<ReactiveCommand, String> CommandDetailBack Setup        

        protected Property<CommandModel<ReactiveCommand, String>> _CommandDetailBack = new Property<CommandModel<ReactiveCommand, String>> { LocatorFunc = _CommandDetailBackLocator };
        static Func<BindableBase, ValueContainer<CommandModel<ReactiveCommand, String>>> _CommandDetailBackLocator = RegisterContainerLocator("CommandDetailBack", model => model.Initialize("CommandDetailBack", ref model._CommandDetailBack, ref _CommandDetailBackLocator, _CommandDetailBackDefaultValueFactory));
        static Func<BindableBase, CommandModel<ReactiveCommand, String>> _CommandDetailBackDefaultValueFactory =
            model =>
            {
                var resource = "CommandDetailBack";           // Command resource  
                var commandId = "CommandDetailBack";
                var vm = CastToCurrentType(model);
                var cmd = new ReactiveCommand(true) { ViewModel = model }; //New Command Core

                cmd.DoExecuteUIBusyTask(
                        vm,
                        async e =>
                        {
                            var stage = vm.StageManager.DefaultStage;
                            if (stage.CanGoBack)
                            {
                                stage.Frame.GoBack();
                            }
                           
                            //Todo: Add DetailBack logic here, or
                            await TaskExHelper.Yield();
                        })
                    .DoNotifyDefaultEventRouter(vm, commandId)
                    .Subscribe()
                    .DisposeWith(vm);

                var cmdmdl = cmd.CreateCommandModel(resource);

                cmdmdl.ListenToIsUIBusy(vm, false);
                return cmdmdl;
            };

        #endregion

       
        public CommandModel<ReactiveCommand, String> CommandNaviToCommentsPage
        {
            get { return _CommandNaviToCommentsPageLocator(this).Value; }
            set { _CommandNaviToCommentsPageLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property CommandModel<ReactiveCommand, String> CommandNaviToCommentsPage Setup        

        protected Property<CommandModel<ReactiveCommand, String>> _CommandNaviToCommentsPage = new Property<CommandModel<ReactiveCommand, String>> { LocatorFunc = _CommandNaviToCommentsPageLocator };
        static Func<BindableBase, ValueContainer<CommandModel<ReactiveCommand, String>>> _CommandNaviToCommentsPageLocator = RegisterContainerLocator("CommandNaviToCommentsPage", model => model.Initialize("CommandNaviToCommentsPage", ref model._CommandNaviToCommentsPage, ref _CommandNaviToCommentsPageLocator, _CommandNaviToCommentsPageDefaultValueFactory));
        static Func<BindableBase, CommandModel<ReactiveCommand, String>> _CommandNaviToCommentsPageDefaultValueFactory =
            model =>
            {
                var resource = "CommandNaviToCommentsPage";           // Command resource  
                var commandId = "CommandNaviToCommentsPage";
                var vm = CastToCurrentType(model);
                var cmd = new ReactiveCommand(true) { ViewModel = model }; //New Command Core

                cmd.DoExecuteUIBusyTask(
                        vm,
                        async e =>
                        {
                            vm.IsCommentPanelOpen = !vm.IsCommentPanelOpen;
                            if (vm.IsCommentPanelOpen)
                            {
                                vm.InitComentsData();
                            }
                            
                            //await vm.StageManager.DefaultStage.Show(new CommentsPage_Model());
                            //Todo: Add NaviToCommentsPage logic here, or
                            await TaskExHelper.Yield();
                        })
                    .DoNotifyDefaultEventRouter(vm, commandId)
                    .Subscribe()
                    .DisposeWith(vm);

                var cmdmdl = cmd.CreateCommandModel(resource);

                cmdmdl.ListenToIsUIBusy(vm, false);
                return cmdmdl;
            };

        #endregion


        public CommandModel<ReactiveCommand, String> CommandWebViewNavigate
        {
            get { return _CommandWebViewNavigateLocator(this).Value; }
            set { _CommandWebViewNavigateLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property CommandModel<ReactiveCommand, String> CommandWebViewNavigate Setup        

        protected Property<CommandModel<ReactiveCommand, String>> _CommandWebViewNavigate = new Property<CommandModel<ReactiveCommand, String>> { LocatorFunc = _CommandWebViewNavigateLocator };
        static Func<BindableBase, ValueContainer<CommandModel<ReactiveCommand, String>>> _CommandWebViewNavigateLocator = RegisterContainerLocator<CommandModel<ReactiveCommand, String>>("CommandWebViewNavigate", model => model.Initialize("CommandWebViewNavigate", ref model._CommandWebViewNavigate, ref _CommandWebViewNavigateLocator, _CommandWebViewNavigateDefaultValueFactory));
        static Func<BindableBase, CommandModel<ReactiveCommand, String>> _CommandWebViewNavigateDefaultValueFactory =
            model =>
            {
                var resource = "CommandWebViewNavigate";           // Command resource  
                var commandId = "CommandWebViewNavigate";
                var vm = CastToCurrentType(model);
                var cmd = new ReactiveCommand(canExecute: true) { ViewModel = model }; //New Command Core

                cmd.DoExecuteUIBusyTask(
                        vm,
                        async e =>
                        {
                            //Todo: Add WebViewNavigate logic here, or
                            await MVVMSidekick.Utilities.TaskExHelper.Yield();
                        })
                    .DoNotifyDefaultEventRouter(vm, commandId)
                    .Subscribe()
                    .DisposeWith(vm);

                var cmdmdl = cmd.CreateCommandModel(resource);

                cmdmdl.ListenToIsUIBusy(
                    model: vm,
                    canExecuteWhenBusy: false);
                return cmdmdl;
            };

        #endregion


        private void SetNightMode(bool nightmode)
        {
            IsNightMode = nightmode;
            BackGroundColor = nightmode ? Colors.OldLace: Colors.WhiteSmoke;
         
        }


        public Color BackGroundColor
        {
            get { return _BackGroundColorLocator(this).Value; }
            set { _BackGroundColorLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property Color BackGroundColor Setup        
        protected Property<Color> _BackGroundColor = new Property<Color> { LocatorFunc = _BackGroundColorLocator };
        static Func<BindableBase, ValueContainer<Color>> _BackGroundColorLocator = RegisterContainerLocator<Color>("BackGroundColor", model => model.Initialize("BackGroundColor", ref model._BackGroundColor, ref _BackGroundColorLocator, _BackGroundColorDefaultValueFactory));
        static Func<Color> _BackGroundColorDefaultValueFactory = () => default(Color);
        #endregion

    }

}

