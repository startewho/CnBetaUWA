using System;
using System.Linq;
using System.Runtime.Serialization;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using CnBetaUWA.Extensions;
using CnBetaUWA.Helper;
using CnBetaUWA.Models;
using MVVMSidekick.Reactive;
using MVVMSidekick.Utilities;
using MVVMSidekick.ViewModels;

namespace CnBetaUWA.ViewModels
{

    [DataContract]
    public class NewsPage_Model : ViewModelBase<NewsPage_Model>
    {
        // If you have install the code sniplets, use "propvm + [tab] +[tab]" create a property。
        // 如果您已经安装了 MVVMSidekick 代码片段，请用 propvm +tab +tab 输入属性

        public NewsPage_Model()
        {
            
        }
        public NewsPage_Model(News model)
        {
            Vm = model;
            GetContent(model.Sid);
        }

        public News Vm { get; set; }


        private async void GetContent(int sid)
        {
            var content = await CnBetaHelper.GetNewsContent(sid);
            if (content != null)
            {

                Vm.NewsContent = ModelHelper.JsonToNewsContent(content);
                var html = await IOHelper.GetTextFromStorage(new Uri("ms-appx:///Html/ContentTemplate.html"));
                html = html.Replace("HomeText", Vm.NewsContent.HomeText);
                html = html.Replace("BodyText", Vm.NewsContent.BodyText);
                html = html.Replace("http://static.cnbetacdn.com/", "");
                TotalContent = html;
                await IOHelper.WriteTextToLocalCacheStorageFile(CnBetaHelper.HtmlFolder,Vm.Sid+".html", TotalContent);
                ContentPath = string.Format(CnBetaHelper.HtmlPath, Vm.Sid);
            }
            
        }




       
        public String Title
        {
            get { return _TitleLocator(this).Value; }
            set { _TitleLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property String Title Setup
        protected Property<String> _Title = new Property<String> { LocatorFunc = _TitleLocator };
        static Func<BindableBase, ValueContainer<String>> _TitleLocator = RegisterContainerLocator("Title", model => model.Initialize("Title", ref model._Title, ref _TitleLocator, _TitleDefaultValueFactory));
        static Func<BindableBase, String> _TitleDefaultValueFactory = m => m.GetType().Name;
        #endregion



        public string ContentPath
        {
            get { return _ContentPathLocator(this).Value; }
            set { _ContentPathLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property string ContentPath Setup        
        protected Property<string> _ContentPath = new Property<string> { LocatorFunc = _ContentPathLocator };
        static Func<BindableBase, ValueContainer<string>> _ContentPathLocator = RegisterContainerLocator<string>("ContentPath", model => model.Initialize("ContentPath", ref model._ContentPath, ref _ContentPathLocator, _ContentPathDefaultValueFactory));
        static Func<string> _ContentPathDefaultValueFactory = () => default(string);
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
                            var webview = view.GetFirstDescendantOfType<WebView>();
                            var isnightmode = e.EventArgs.Parameter is bool && (bool) e.EventArgs.Parameter;
                            if (isnightmode)
                            {
                                await webview.InvokeScriptAsync("changeNigthMode", new[] { "true" });
                            }
                            else
                            {
                                await webview.InvokeScriptAsync("changeNigthMode", new[] { "" });
                            }
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
                            var webview = view.GetFirstDescendantOfType<WebView>();
                            var args = e.EventArgs.Parameter as RangeBaseValueChangedEventArgs;
                            if (args!=null)
                            {
                                await webview.InvokeScriptAsync("changeFontSize", new[] { string.Format("{0:0%}", args.NewValue * 0.5) });
                            }
                            
                            //Todo: Add ChangeWebViewFontSize logic here, or
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


        public CommandModel<ReactiveCommand, String> CommandChangeWebViewSettings
        {
            get { return _CommandChangeWebViewSettingsLocator(this).Value; }
            set { _CommandChangeWebViewSettingsLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property CommandModel<ReactiveCommand, String> CommandChangeWebViewSettings Setup        

        protected Property<CommandModel<ReactiveCommand, String>> _CommandChangeWebViewSettings = new Property<CommandModel<ReactiveCommand, String>> { LocatorFunc = _CommandChangeWebViewSettingsLocator };
        static Func<BindableBase, ValueContainer<CommandModel<ReactiveCommand, String>>> _CommandChangeWebViewSettingsLocator = RegisterContainerLocator("CommandChangeWebViewSettings", model => model.Initialize("CommandChangeWebViewSettings", ref model._CommandChangeWebViewSettings, ref _CommandChangeWebViewSettingsLocator, _CommandChangeWebViewSettingsDefaultValueFactory));
        static Func<BindableBase, CommandModel<ReactiveCommand, String>> _CommandChangeWebViewSettingsDefaultValueFactory =
            model =>
            {
                var resource = "CommandChangeWebViewSettings";           // Command resource  
                var commandId = "CommandChangeWebViewSettings";
                var vm = CastToCurrentType(model);
                var cmd = new ReactiveCommand(true) { ViewModel = model }; //New Command Core

                cmd.DoExecuteUIBusyTask(
                        vm,
                        async e =>
                        {
                            var view = vm.StageManager.CurrentBindingView as NewsPage;
                            var webview = view.GetFirstDescendantOfType<WebView>();
                            var tag = e.EventArgs.Parameter as string;
                            switch (tag)
                            {
                                case "NightMode":
                                    await webview.InvokeScriptAsync("changeColor", new[] { "arg1", "arg2" });
                                    break;
                                case "FontSize":
                                    await webview.InvokeScriptAsync("changeFontSize", new[] { "120%" });
                                    break;
                                default:
                                    break;
                            }
                            //Todo: Add ChangeWebViewSettings logic here, or
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
                            var commentsjson = await CnBetaHelper.GetNewsComment(vm.Vm.Sid);
                            vm.Vm.NewsComments = ModelHelper.JsonToNewsComments(commentsjson).ToList();
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

