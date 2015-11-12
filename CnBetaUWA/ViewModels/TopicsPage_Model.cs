using MVVMSidekick.ViewModels;
using MVVMSidekick.Views;
using MVVMSidekick.Reactive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using CnBetaUWA.Extensions;
using CnBetaUWA.Helper;
using CnBetaUWA.Models;
using Q42.WinRT.Storage;

namespace CnBetaUWA.ViewModels
{

    [DataContract]
    public class TopicsPage_Model : ViewModelBase<TopicsPage_Model>
    {
        // If you have install the code sniplets, use "propvm + [tab] +[tab]" create a property。
        // 如果您已经安装了 MVVMSidekick 代码片段，请用 propvm +tab +tab 输入属性
        public TopicsPage_Model()
        {
            InitData();
            PropScribe();
        }

        private readonly StorageHelper<IEnumerable<News>> _storageHelper = new StorageHelper<IEnumerable<News>>(Windows.Storage.ApplicationData.Current.LocalFolder);
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

        private void InitData()
        {
            TopicColletion = new ObservableCollection<Topic>();

            var jsontopics = SettingsHelper.Get<string>(CnBetaHelper.SettingSelectedTotics, null);

            var topics = SerializerHelper.JsonDeserialize<List<TopicType>>(jsontopics);

            if (topics != null)
            {
                foreach (var topicType in topics)
                {
                    TopicColletion.Add(new Topic(topicType));
                }
                if (TopicColletion.Any())
                {
                    SelectedTopic = TopicColletion[0];
                    SelectedTopic.IsSelected = true;
                    LoadAction(SelectedTopic);
                }


            }

        }

        private void PropScribe()
        {
            //当SelectedTopic变化时,缓存当前页面数据,并加载新页面
            GetValueContainer(vm => vm.SelectedTopic).GetEventObservable().Subscribe(e =>
            {
                var oldtopic = e.EventArgs.OldValue;
                var newtopic = e.EventArgs.NewValue;
              //  SaveAction(oldtopic);
                oldtopic.IsSelected = false;
                newtopic.IsSelected = true;
               LoadAction(newtopic);
            }).DisposeWith(this);

        }

        protected override Task OnBindedViewLoad(IView view)
        {

           return base.OnBindedViewLoad(view);
        }

        protected override Task OnBindedViewUnload(IView view)
        {
            if (SelectedTopic!=null)
            {
                SaveAction(SelectedTopic);
            }
        
            return base.OnBindedViewUnload(view);

        }




        public ObservableCollection<Topic> TopicColletion
        {
            get { return _TopicColletionLocator(this).Value; }
            set { _TopicColletionLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property ObservableCollection<Topic> TopicColletion Setup        
        protected Property<ObservableCollection<Topic>> _TopicColletion = new Property<ObservableCollection<Topic>> { LocatorFunc = _TopicColletionLocator };
        static Func<BindableBase, ValueContainer<ObservableCollection<Topic>>> _TopicColletionLocator = RegisterContainerLocator<ObservableCollection<Topic>>("TopicColletion", model => model.Initialize("TopicColletion", ref model._TopicColletion, ref _TopicColletionLocator, _TopicColletionDefaultValueFactory));
        static Func<ObservableCollection<Topic>> _TopicColletionDefaultValueFactory = () => default(ObservableCollection<Topic>);
        #endregion


        public Topic SelectedTopic
        {
            get { return _SelectedTopicLocator(this).Value; }
            set
            {
               _SelectedTopicLocator(this).SetValueAndTryNotify(value);
             }
        }
        #region Property Topic SelectedTopic Setup        
        protected Property<Topic> _SelectedTopic = new Property<Topic> { LocatorFunc = _SelectedTopicLocator };
        static Func<BindableBase, ValueContainer<Topic>> _SelectedTopicLocator = RegisterContainerLocator<Topic>("SelectedTopic", model => model.Initialize("SelectedTopic", ref model._SelectedTopic, ref _SelectedTopicLocator, _SelectedTopicDefaultValueFactory));
        static Func<Topic> _SelectedTopicDefaultValueFactory = () => default(Topic);
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
                            var view = vm.StageManager.CurrentBindingView as TopicsPage;
                            var scrooviewer = view.GetDescendantsOfType<ListView>().First(item=>item.Visibility==Visibility.Visible&&item.ItemsSource!=null);
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


        private async void SaveAction(Topic selectTopic)
        {
            await _storageHelper.SaveAsync(selectTopic.NewsSourceCollection.Take(100), selectTopic.CurrentTopicType.Id.ToString());
            selectTopic.NewsSourceCollection.OnLoadMoreStarted -= NewsSourceCollection_OnLoadMoreStarted;
        }


        private async void LoadAction(Topic selectTopic)
        {
            var cachenews = await _storageHelper.LoadAsync(selectTopic.CurrentTopicType.Id.ToString());
            if (cachenews != null && cachenews.Any())
            {
                selectTopic.InitNewsSourceColletion( cachenews);
              
            }
            else
            {
                selectTopic.InitNewsSourceColletion( cachenews);
            }

            selectTopic.NewsSourceCollection.OnLoadMoreStarted += NewsSourceCollection_OnLoadMoreStarted;
            
        }

        private  void Reresh()
        {
            var cachenews = SelectedTopic.NewsSourceCollection;
            SelectedTopic.NewsSourceCollection.OnLoadMoreStarted -= NewsSourceCollection_OnLoadMoreStarted;
            SelectedTopic.InitNewsSourceColletion(cachenews);
            SelectedTopic.NewsSourceCollection.OnLoadMoreStarted += NewsSourceCollection_OnLoadMoreStarted;
            //Message = addedcount == 0 ? DateTime.Now+"没有更新,等会再点吧" : DateTime.Now + "更新了" + addedcount;
        }

        private void NewsSourceCollection_OnLoadMoreStarted(uint count)
        {
          // throw new NotImplementedException();
        }
    }

}

