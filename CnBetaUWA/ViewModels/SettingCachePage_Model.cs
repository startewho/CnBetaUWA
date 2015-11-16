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
using Windows.Storage;
using CnBetaUWA.Helper;
using CnBetaUWA.Models;
using Q42.WinRT.Data;
using Q42.WinRT.Storage;

namespace CnBetaUWA.ViewModels
{

    [DataContract]
    public class SettingCachePage_Model : ViewModelBase<SettingCachePage_Model>
    {
        // If you have install the code sniplets, use "propvm + [tab] +[tab]" create a property。
        // 如果您已经安装了 MVVMSidekick 代码片段，请用 propvm +tab +tab 输入属性

        private bool _isLoaded;

        public SettingCachePage_Model()
        {
            CacheSize = 0;
            
        }


        private async void InitData()
        {
            var cachefolder = ApplicationData.Current.LocalFolder;
            var size = await IOHelper.GetFolderSize(cachefolder);
            CacheSize = size/1024/1024;

        }
        protected override Task OnBindedViewLoad(IView view)
        {
            if (_isLoaded) return base.OnBindedViewLoad(view);
            InitData();
            _isLoaded = true;
            return base.OnBindedViewLoad(view);
        }


        public double CacheSize
        {
            get { return _CacheSizeLocator(this).Value; }
            set { _CacheSizeLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property double CacheSize Setup        
        protected Property<double> _CacheSize = new Property<double> { LocatorFunc = _CacheSizeLocator };
        static Func<BindableBase, ValueContainer<double>> _CacheSizeLocator = RegisterContainerLocator<double>("CacheSize", model => model.Initialize("CacheSize", ref model._CacheSize, ref _CacheSizeLocator, _CacheSizeDefaultValueFactory));
        static Func<double> _CacheSizeDefaultValueFactory = () => default(double);
        #endregion


        public CommandModel<ReactiveCommand, String> CommandDeleteCache
        {
            get { return _CommandDeleteCacheLocator(this).Value; }
            set { _CommandDeleteCacheLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property CommandModel<ReactiveCommand, String> CommandDeleteCache Setup        

        protected Property<CommandModel<ReactiveCommand, String>> _CommandDeleteCache = new Property<CommandModel<ReactiveCommand, String>> { LocatorFunc = _CommandDeleteCacheLocator };
        static Func<BindableBase, ValueContainer<CommandModel<ReactiveCommand, String>>> _CommandDeleteCacheLocator = RegisterContainerLocator<CommandModel<ReactiveCommand, String>>("CommandDeleteCache", model => model.Initialize("CommandDeleteCache", ref model._CommandDeleteCache, ref _CommandDeleteCacheLocator, _CommandDeleteCacheDefaultValueFactory));
        static Func<BindableBase, CommandModel<ReactiveCommand, String>> _CommandDeleteCacheDefaultValueFactory =
            model =>
            {
                var resource = "CommandDeleteCache";           // Command resource  
                var commandId = "CommandDeleteCache";
                var vm = CastToCurrentType(model);
                var cmd = new ReactiveCommand(canExecute: true) { ViewModel = model }; //New Command Core

                cmd.DoExecuteUIBusyTask(
                        vm,
                        async e =>
                        {
                            await WebDataCache.ClearAll();
                           
                            vm.InitData();
                            //Todo: Add DeleteCache logic here, or
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

