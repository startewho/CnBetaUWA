using MVVMSidekick.ViewModels;
using MVVMSidekick.Views;
using MVVMSidekick.Reactive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Windows.UI.Xaml.Controls;
using CnBetaUWA.DataSource;
using CnBetaUWA.Extensions;
using CnBetaUWA.Helper;
using CnBetaUWA.Models;
using Q42.WinRT.Storage;
namespace CnBetaUWA.ViewModels
{

    [DataContract]
    public class LatestNewsPage_Model : ViewModelBase<LatestNewsPage_Model>
    {
        // If you have install the code sniplets, use "propvm + [tab] +[tab]" create a property。
        // 如果您已经安装了 MVVMSidekick 代码片段，请用 propvm +tab +tab 输入属性

        public LatestNewsPage_Model()
        {
            Title = "最新资讯";
           // PageType = typeof (BlankPage);
         
            PropScribe();
        }

        private bool _isLoaded;
        private readonly StorageHelper<IEnumerable<News>> _storageHelper = new StorageHelper<IEnumerable<News>>(Windows.Storage.ApplicationData.Current.LocalFolder);
        protected override Task OnBindedViewLoad(IView view)
        {
            if (_isLoaded) return base.OnBindedViewLoad(view);
            LoadAction();
            _isLoaded = true;
            return base.OnBindedViewLoad(view);
        }

        protected override Task OnBindedViewUnload(IView view)
        {
            SaveAction();
            return  base.OnBindedViewUnload(view);
           
        }

        private async void SaveAction()
        {
          await _storageHelper.SaveAsync(NewsSourceCollection.Take(100), nameof(NewsSourceCollection));
         
       }


        private async void LoadAction()
        {
            var cachenews=await _storageHelper.LoadAsync(nameof(NewsSourceCollection));
            if (cachenews != null&&cachenews.Any())
            {
               
                NewsSourceCollection = new IncrementalLoadingCollection<IncrementalNewsSource, News>(CnBetaHelper.TypeAll, cachenews);
                //NewsSourceCollection.AddRange(cachenews);
                //Reresh();
            }
            else
            {
                NewsSourceCollection = new IncrementalLoadingCollection<IncrementalNewsSource, News>(CnBetaHelper.TypeAll, cachenews);
                
            }
            NewsSourceCollection.OnLoadMoreStarted += DataSourceCollection_OnLoadMoreStarted;
        }

        private void PropScribe()
        {
            //GetValueContainer(vm => vm.NewsSourceCollection.Count).GetNewValueObservable().Subscribe(e =>
            //{
            //    var count = e.EventArgs;
               
            //}).DisposeWith(this);
        }

        private async  void Reresh()
        {
            var addedcount=await NewsSourceCollection.AttachToEnd();
            //Message = addedcount == 0 ? DateTime.Now+"没有更新,等会再点吧" : DateTime.Now + "更新了" + addedcount;
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


      

        public IncrementalLoadingCollection<IncrementalNewsSource,News> NewsSourceCollection
        {
            get { return _NewsSourceCollectionLocator(this).Value; }
            set { _NewsSourceCollectionLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property IncrementalLoadingCollection<NewIncrementalSource,NewsModel> NewsSourceCollection Setup        
        protected Property<IncrementalLoadingCollection<IncrementalNewsSource,News>> _NewsSourceCollection = new Property<IncrementalLoadingCollection<IncrementalNewsSource,News>> { LocatorFunc = _NewsSourceCollectionLocator };
        static Func<BindableBase, ValueContainer<IncrementalLoadingCollection<IncrementalNewsSource,News>>> _NewsSourceCollectionLocator = RegisterContainerLocator<IncrementalLoadingCollection<IncrementalNewsSource,News>>("NewsSourceCollection", model => model.Initialize("NewsSourceCollection", ref model._NewsSourceCollection, ref _NewsSourceCollectionLocator, _NewsSourceCollectionDefaultValueFactory));
        static Func<IncrementalLoadingCollection<IncrementalNewsSource,News>> _NewsSourceCollectionDefaultValueFactory = () => default(IncrementalLoadingCollection<IncrementalNewsSource,News>);
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
                            var view = vm.StageManager.CurrentBindingView as LatestNewsPage;
                            var scrooviewer = view.GetFirstDescendantOfType<ListView>();
                            vm.Reresh();
                            await scrooviewer.ScrollToIndex(0);
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

