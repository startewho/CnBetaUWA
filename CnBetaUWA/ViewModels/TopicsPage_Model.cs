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
using ImageLib.Helpers;
using Q42.WinRT.Storage;

namespace CnBetaUWA.ViewModels
{

    [DataContract]
    public class TopicsPage_Model : ViewModelBase<TopicsPage_Model>
    {
        // If you have install the code sniplets, use "propvm + [tab] +[tab]" create a property。
        // 如果您已经安装了 MVVMSidekick 代码片段，请用 propvm +tab +tab 输入属性

        private StorageHelper<IEnumerable<News>> _storageHelper = new StorageHelper<IEnumerable<News>>(StorageType.Local);
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

        protected  override Task OnBindedViewLoad(IView view)
        {

            TopicColletion = new ObservableCollection<Topic>();

            var jsontopics = SettingsHelper.Get<string>(CnBetaHelper.SettingSelectedTotics, null);

            var topics = SerializerHelper.JsonDeserialize<List<TopicType>>(jsontopics);

            if (topics!=null)
            {
                foreach (var topicType in topics)
                {
                    TopicColletion.Add(new Topic(topicType));
                }

                SelectedTopic = TopicColletion[0];
                SelectedTopic.IsSelected = true;
                LoadAction();
               // SelectedTopic.InitNewsSourceColletion();
                //  SelectedCollection.AddRange(topics);
            }
          
            return base.OnBindedViewLoad(view);
        }

        protected override Task OnBindedViewUnload(IView view)
        {
            SaveAction();
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
                value.IsSelected = true;
                LoadAction();
            }
        }
        #region Property Topic SelectedTopic Setup        
        protected Property<Topic> _SelectedTopic = new Property<Topic> { LocatorFunc = _SelectedTopicLocator };
        static Func<BindableBase, ValueContainer<Topic>> _SelectedTopicLocator = RegisterContainerLocator<Topic>("SelectedTopic", model => model.Initialize("SelectedTopic", ref model._SelectedTopic, ref _SelectedTopicLocator, _SelectedTopicDefaultValueFactory));
        static Func<Topic> _SelectedTopicDefaultValueFactory = () => default(Topic);
        #endregion

        private async void SaveAction()
        {
            await _storageHelper.SaveAsync(SelectedTopic.NewsSourceCollection.Take(100), SelectedTopic.CurrentTopicType.Id.ToString());
            SelectedTopic.NewsSourceCollection.OnLoadMoreStarted -= NewsSourceCollection_OnLoadMoreStarted;
        }


        private async void LoadAction()
        {
            var cachenews = await _storageHelper.LoadAsync(SelectedTopic.CurrentTopicType.Id.ToString());
            if (cachenews != null && cachenews.Any())
            {
              SelectedTopic.InitNewsSourceColletion(cachenews.First().Sid, cachenews.Last().Sid, cachenews);
              
            }
            else
            {
                SelectedTopic.InitNewsSourceColletion( 0, 0, cachenews);
            }

            SelectedTopic.NewsSourceCollection.OnLoadMoreStarted += NewsSourceCollection_OnLoadMoreStarted;
        }

        private void NewsSourceCollection_OnLoadMoreStarted(uint count)
        {
          // throw new NotImplementedException();
        }
    }

}

