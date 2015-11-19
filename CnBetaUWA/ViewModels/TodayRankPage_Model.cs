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
using ImageLib.Helpers;
using Q42.WinRT.Storage;

namespace CnBetaUWA.ViewModels
{

    [DataContract]
    public class TodayRankPage_Model : ViewModelBase<TodayRankPage_Model>
    {
        public TodayRankPage_Model()
        {
            InitData();
            SelectedTopic = TopicColletion[0];
            SelectedTopic.IsSelected = true;
            LoadAction(SelectedTopic);

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

            var topics = new List<TopicType>
                {
                    new TopicType
                    {

                        Name="评论最多",
                        NamePre = "comments",

                    },
                    new TopicType
                    {
                        Name="支持最多",
                        NamePre = "dig",
                    }
                    ,new TopicType
                    {
                        Name="阅读最多",
                        NamePre = "counter",
                    }
                };

            foreach (var topicType in topics)
            {
                TopicColletion.Add(new Topic(topicType));
            }

        }

        private void PropScribe()
        {
            //当SelectedTopic变化时,缓存当前页面数据,并加载新页面
            GetValueContainer(vm => vm.SelectedTopic).GetEventObservable().Subscribe(e =>
            {
                var oldtopic = e.EventArgs.OldValue;
                var newtopic = e.EventArgs.NewValue;
                SaveAction(oldtopic);
                oldtopic.IsSelected = false;
                newtopic.IsSelected = true;
                LoadAction(newtopic);
            }).DisposeWith(this);

        }

        protected override Task OnBindedViewLoad(IView view)
        {
          
            PropScribe();
            return base.OnBindedViewLoad(view);
        }

        protected override Task OnBindedViewUnload(IView view)
        {
            SaveAction(SelectedTopic);
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
                            var view = vm.StageManager.CurrentBindingView as TodayRankPage;
                            var scrooviewer = view.GetDescendantsOfType<ListView>().First(item => item.Visibility == Visibility.Visible && item.ItemsSource != null);
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
            await _storageHelper.SaveAsync(selectTopic.StaticNewesCollection.Take(100), selectTopic.CurrentTopicType.NamePre);
         }


        private async void LoadAction(Topic selectTopic)
        {
            var cachenews = await _storageHelper.LoadAsync(selectTopic.CurrentTopicType.NamePre);

            selectTopic.InitTodayRankNewsSourceColletion(cachenews);
            Reresh();
           
        }

        public async void Reresh()
        {
            var jsonttext =await CnBetaHelper.GetTodayRankNews(CnBetaHelper.TypeTodayRank, SelectedTopic.CurrentTopicType.NamePre);
            if (jsonttext == null) return;
            var listnewses= ModelHelper.JsonToNewses(jsonttext);
            if (listnewses==null) return;
            if (SelectedTopic.StaticNewesCollection.Count > 0 && SelectedTopic.StaticNewesCollection.First().Sid >= listnewses.First().Sid) return;
            SelectedTopic.StaticNewesCollection.Clear();
            SelectedTopic.StaticNewesCollection.AddRange(listnewses);
            

            //Message = addedcount == 0 ? DateTime.Now+"没有更新,等会再点吧" : DateTime.Now + "更新了" + addedcount;
        }

        private void NewsSourceCollection_OnLoadMoreStarted(uint count)
        {
            // throw new NotImplementedException();
        }

    }

}

