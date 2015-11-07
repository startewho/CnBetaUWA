using MVVMSidekick.ViewModels;
using MVVMSidekick.Reactive;
using System;
using System.Runtime.Serialization;
using CnBetaUWA.Controls;
using CnBetaUWA.Extensions;

namespace CnBetaUWA.ViewModels
{

    [DataContract]
    public class SettingPage_Model : ViewModelBase<SettingPage_Model>
    {
        // If you have install the code sniplets, use "propvm + [tab] +[tab]" create a property。
        // 如果您已经安装了 MVVMSidekick 代码片段，请用 propvm +tab +tab 输入属性

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



        public CommandModel<ReactiveCommand, String> CommandNavigateToDetailSettingPage
        {
            get { return _CommandNavigateToDetailSettingPageLocator(this).Value; }
            set { _CommandNavigateToDetailSettingPageLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property CommandModel<ReactiveCommand, String> CommandNavigateToDetailSettingPage Setup        

        protected Property<CommandModel<ReactiveCommand, String>> _CommandNavigateToDetailSettingPage = new Property<CommandModel<ReactiveCommand, String>> { LocatorFunc = _CommandNavigateToDetailSettingPageLocator };
        static Func<BindableBase, ValueContainer<CommandModel<ReactiveCommand, String>>> _CommandNavigateToDetailSettingPageLocator = RegisterContainerLocator<CommandModel<ReactiveCommand, String>>("CommandNavigateToDetailSettingPage", model => model.Initialize("CommandNavigateToDetailSettingPage", ref model._CommandNavigateToDetailSettingPage, ref _CommandNavigateToDetailSettingPageLocator, _CommandNavigateToDetailSettingPageDefaultValueFactory));
        static Func<BindableBase, CommandModel<ReactiveCommand, String>> _CommandNavigateToDetailSettingPageDefaultValueFactory =
            model =>
            {
                var resource = "CommandNavigateToDetailSettingPage";           // Command resource  
                var commandId = "CommandNavigateToDetailSettingPage";
                var vm = CastToCurrentType(model);
                var cmd = new ReactiveCommand(canExecute: true) { ViewModel = model }; //New Command Core

                cmd.DoExecuteUIBusyTask(
                        vm,
                        async e =>
                        {


                           
                            var pageviewer = vm.StageManager.CurrentBindingView as SettingPage;
                            var masterdetail = pageviewer.GetFirstDescendantOfType<MasterDetailView>();
                            var tag =Convert.ToInt32(e.EventArgs.Parameter);
                            switch (tag)
                            {
                                case 1:
                                    masterdetail?.DetailFrameNavigateTo(typeof (TopicTypesMangePage),
                                        new TopicTypesMangePage_Model(), true);
                                    break;
                                case 2:
                                    masterdetail?.DetailFrameNavigateTo(typeof (TopicTypesMangePage),
                                        new TopicTypesMangePage_Model(), true);
                                    break;
                                default:
                                    break;
                            }
                            //Todo: Add NavigateToDetailSettingPage logic here, or
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

