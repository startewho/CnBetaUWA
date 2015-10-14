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

namespace CnBetaUWA.ViewModels
{

    [DataContract]
    public class LatestNewsPage_Model : ViewModelBase<LatestNewsPage_Model>
    {
        // If you have install the code sniplets, use "propvm + [tab] +[tab]" create a property。
        // 如果您已经安装了 MVVMSidekick 代码片段，请用 propvm +tab +tab 输入属性

        public LatestNewsPage_Model()
        {
            DataSourceCollection=new IncrementalLoadingCollection<IncrementalNewsSource, NewsModel>(CnBetaHelper.TypeAll,20);
            DataSourceCollection.OnLoadMoreStarted += DataSourceCollection_OnLoadMoreStarted;
            PropScribe();
        }


        private void PropScribe()
        {
            //GetValueContainer(vm => vm.DataSourceCollection.Count).GetNewValueObservable().Subscribe(e =>
            //{
            //    var count = e.EventArgs;
               
            //}).DisposeWith(this);
        }

        private async  void Reresh()
        {
          var addedcount=await DataSourceCollection.AttachToEnd();
        }
        

        private void DataSourceCollection_OnLoadMoreStarted(uint count)
        {
         //   throw new NotImplementedException();
        }

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




        public IncrementalLoadingCollection<IncrementalNewsSource,NewsModel> DataSourceCollection
        {
            get { return _DataSourceCollectionLocator(this).Value; }
            set { _DataSourceCollectionLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property IncrementalLoadingCollection<NewIncrementalSource,NewsModel> DataSourceCollection Setup        
        protected Property<IncrementalLoadingCollection<IncrementalNewsSource,NewsModel>> _DataSourceCollection = new Property<IncrementalLoadingCollection<IncrementalNewsSource,NewsModel>> { LocatorFunc = _DataSourceCollectionLocator };
        static Func<BindableBase, ValueContainer<IncrementalLoadingCollection<IncrementalNewsSource,NewsModel>>> _DataSourceCollectionLocator = RegisterContainerLocator<IncrementalLoadingCollection<IncrementalNewsSource,NewsModel>>("DataSourceCollection", model => model.Initialize("DataSourceCollection", ref model._DataSourceCollection, ref _DataSourceCollectionLocator, _DataSourceCollectionDefaultValueFactory));
        static Func<IncrementalLoadingCollection<IncrementalNewsSource,NewsModel>> _DataSourceCollectionDefaultValueFactory = () => default(IncrementalLoadingCollection<IncrementalNewsSource,NewsModel>);
        #endregion


        public CommandModel<ReactiveCommand, String> CommandRereshDataSourceCollection
        {
            get { return _CommandRereshDataSourceCollectionLocator(this).Value; }
            set { _CommandRereshDataSourceCollectionLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property CommandModel<ReactiveCommand, String> CommandRereshDataSourceCollection Setup        

        protected Property<CommandModel<ReactiveCommand, String>> _CommandRereshDataSourceCollection = new Property<CommandModel<ReactiveCommand, String>> { LocatorFunc = _CommandRereshDataSourceCollectionLocator };
        static Func<BindableBase, ValueContainer<CommandModel<ReactiveCommand, String>>> _CommandRereshDataSourceCollectionLocator = RegisterContainerLocator<CommandModel<ReactiveCommand, String>>("CommandRereshDataSourceCollection", model => model.Initialize("CommandRereshDataSourceCollection", ref model._CommandRereshDataSourceCollection, ref _CommandRereshDataSourceCollectionLocator, _CommandRereshDataSourceCollectionDefaultValueFactory));
        static Func<BindableBase, CommandModel<ReactiveCommand, String>> _CommandRereshDataSourceCollectionDefaultValueFactory =
            model =>
            {
                var resource = "CommandRereshDataSourceCollection";           // Command resource  
                var commandId = "CommandRereshDataSourceCollection";
                var vm = CastToCurrentType(model);
                var cmd = new ReactiveCommand(canExecute: true) { ViewModel = model }; //New Command Core

                cmd.DoExecuteUIBusyTask(
                        vm,
                        async e =>
                        {
                            
                            vm.Reresh();
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

