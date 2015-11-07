using System;
using System.Linq;
using System.Reactive.Linq;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;
using CnBetaUWA.DataSource;
using CnBetaUWA.Extensions;
using CnBetaUWA.Helper;
using CnBetaUWA.Models;
using MVVMSidekick.Reactive;
using MVVMSidekick.Utilities;
using MVVMSidekick.ViewModels;
using MVVMSidekick.Views;
using MyToolkit.Controls;

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
            Vm = model;
          
        }

        public News Vm { get; set; }

        private void InitComentsData()
        {
            CommentsSource = new IncrementalPageLoadingCollection<IncrementalNewsCommentPageSource, NewsComment>(Vm.Sid.ToString(), 1, 10);
            CommentsSource.OnLoadMoreCompleted += CommentsSource_OnLoadMoreCompleted;
          
        }

        private void CommentsSource_OnLoadMoreCompleted(int count)
        {
            string message = string.Format("已经加载了{0}条评论", count);
            MessageHelper.ShowMessage(this,message);

            var list = CommentsSource;
            if (CommentsSource != null)
            {
                foreach (var comment in list)
                {
                    if (!comment.IsShow) continue;
                    var parrentcomment = new NewsComment
                    {
                        UserName = list.First((item) => item.Tid == comment.Pid).UserName,
                        Content = list.First((item) => item.Tid == comment.Pid).Content
                    };
                    comment.PidComment = parrentcomment;
                }
            }
        }

        private async void GetContent(int sid)
        {
            var content = await CnBetaHelper.GetNewsContent(sid);
            if (content != null)
            {

                NewsContent = ModelHelper.JsonToNewsContent(content);
                var html = await IOHelper.GetTextFromStorage(new Uri("ms-appx:///Html/ContentTemplate.html"));

                html = html.Replace("#Date", Vm.CreatTime);
             
                html = html.Replace("#Source", NewsContent.Source);
                html = html.Replace("#Author", NewsContent.Author);
               //  html = html.Replace("#Topic", Vm.TopictLogoPicture);
                html = html.Replace("HomeText", NewsContent.HomeText);
                html = html.Replace("BodyText", NewsContent.BodyText);
                html= Regex.Replace(html, "(<a.+?)(<img[^>]+>)((</a>)+)", "$2");
             // html = html.Replace("http://static.cnbetacdn.com/", "");
                TotalContent = html;
               // await IOHelper.WriteTextToLocalCacheStorageFile(CnBetaHelper.HtmlFolder,Vm.Sid+".html", TotalContent);
               // ContentPath = string.Format(CnBetaHelper.HtmlPath, Vm.Sid);

            }
            


        }
        protected override Task OnBindedViewLoad(IView view)
        {
            if (_isLoaded) return base.OnBindedViewLoad(view);
            GetContent(Vm.Sid);
            SubscribeCommand();
            _isLoaded = true;
            return base.OnBindedViewLoad(view);
          
        }

        protected override Task OnBindedViewUnload(IView view)
        {
             Vm = null;
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
                         var result = await CnBetaHelper.GetCommentAction(Vm.Sid, comment.Tid, "against");
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


        public CommandModel<ReactiveCommand, String> CommandChangeWebViewNightMode
        {
            get { return _CommandChangeWebViewNightModeLocator(this).Value; }
            set { _CommandChangeWebViewNightModeLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property CommandModel<ReactiveCommand, String> CommandChangeWebViewNightMode Setup        

        protected Property<CommandModel<ReactiveCommand, String>> _CommandChangeWebViewNightMode = new Property<CommandModel<ReactiveCommand, String>> { LocatorFunc = _CommandChangeWebViewNightModeLocator };
        static Func<BindableBase, ValueContainer<CommandModel<ReactiveCommand, String>>> _CommandChangeWebViewNightModeLocator = RegisterContainerLocator<CommandModel<ReactiveCommand, String>>("CommandChangeWebViewNightMode", model => model.Initialize("CommandChangeWebViewNightMode", ref model._CommandChangeWebViewNightMode, ref _CommandChangeWebViewNightModeLocator, _CommandChangeWebViewNightModeDefaultValueFactory));
        static Func<BindableBase, CommandModel<ReactiveCommand, String>> _CommandChangeWebViewNightModeDefaultValueFactory =
            model =>
            {
                var resource = "CommandChangeWebViewNightMode";           // Command resource  
                var commandId = "CommandChangeWebViewNightMode";
                var vm = CastToCurrentType(model);
                var cmd = new ReactiveCommand(canExecute: true) { ViewModel = model }; //New Command Core

                cmd.DoExecuteUIBusyTask(
                        vm,
                        async e =>
                        {
                            var view = vm.StageManager.CurrentBindingView as NewsPage;
                            var htmlcontrol= view.GetFirstDescendantOfType<HtmlControl>();
                            var isnightmode = e.EventArgs.Parameter is bool && (bool)e.EventArgs.Parameter;
                            if (isnightmode)
                            { 
                                htmlcontrol.Background=new SolidColorBrush() {Color =Colors.AntiqueWhite};
                            }

                            else
                            {
                                htmlcontrol.Background = new SolidColorBrush() { Color = Colors.WhiteSmoke };
                            }

                            //var webview = view.GetFirstDescendantOfType<WebView>();
                            //var isnightmode = e.EventArgs.Parameter is bool && (bool) e.EventArgs.Parameter;
                            //if (isnightmode)
                            //{
                            //    await webview.InvokeScriptAsync("changeNigthMode", new[] { "true" });
                            //}
                            //else
                            //{
                            //    await webview.InvokeScriptAsync("changeNigthMode", new[] { "" });
                            //}
                            //Todo: Add ChangeWebViewNightMode logic here, or
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



    }

}

