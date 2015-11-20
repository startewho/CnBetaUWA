using MVVMSidekick.ViewModels;
using MVVMSidekick.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using CnBetaUWA.DataSource;
using CnBetaUWA.Helper;
using CnBetaUWA.Models;
using Q42.WinRT.Storage;
using CnBetaUWA.Extensions;
namespace CnBetaUWA.ViewModels
{

    [DataContract]
    public class SettingTopicTypesMangePage_Model : ViewModelBase<SettingTopicTypesMangePage_Model>
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

     
        protected override Task OnBindedViewLoad(IView view)
        {
            if (!_isLoaded)
            {
                TopicTypes = new IncrementalPageLoadingCollection<IncrementalTopicTypePageSource, TopicType>("", 0, 20);

                _isLoaded = true;
            }
          
            return base.OnBindedViewLoad(view);
        }

        private bool _isLoaded;
      
        protected override Task OnBindedViewUnload(IView view)
        {

            SaveSetting();
            return base.OnBindedViewUnload(view);
        }

        private void SaveSetting()
        {
            var lists = new List<TopicType>();
            var jsontopics = SettingsHelper.Get<string>(CnBetaHelper.SettingSelectedTotics, null);
            var settingtopics = SerializerHelper.JsonDeserialize<List<TopicType>>(jsontopics);
            settingtopics.AddRange(TopicTypes.Where(item => item.IsSelected));
            foreach (var settingtopic in settingtopics)
            {
                foreach (var topicType in TopicTypes.Where(topicType => settingtopic.Id == topicType.Id))
                {
                    settingtopic.IsSelected = topicType.IsSelected;
                }
                if (settingtopic.IsSelected)
                {
                    lists.Add(settingtopic);
                }
            }


            var newjsontopics = SerializerHelper.ToJson(lists.Distinct(item => item.Id).Take(5));
            SettingsHelper.Set(CnBetaHelper.SettingSelectedTotics, newjsontopics);

        }

        public IncrementalPageLoadingCollection<IncrementalTopicTypePageSource,TopicType> TopicTypes
        {
            get { return _TopicTypesLocator(this).Value; }
            set { _TopicTypesLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property IncrementalPageLoadingCollection<IncrementalTopicTypePageSource,TopicType> TopicTypes Setup        
        protected Property<IncrementalPageLoadingCollection<IncrementalTopicTypePageSource,TopicType>> _TopicTypes = new Property<IncrementalPageLoadingCollection<IncrementalTopicTypePageSource,TopicType>> { LocatorFunc = _TopicTypesLocator };
        static Func<BindableBase, ValueContainer<IncrementalPageLoadingCollection<IncrementalTopicTypePageSource,TopicType>>> _TopicTypesLocator = RegisterContainerLocator<IncrementalPageLoadingCollection<IncrementalTopicTypePageSource,TopicType>>("TopicTypes", model => model.Initialize("TopicTypes", ref model._TopicTypes, ref _TopicTypesLocator, _TopicTypesDefaultValueFactory));
        static Func<IncrementalPageLoadingCollection<IncrementalTopicTypePageSource,TopicType>> _TopicTypesDefaultValueFactory = () => default(IncrementalPageLoadingCollection<IncrementalTopicTypePageSource,TopicType>);
        #endregion



        public string FliterText
        {
            get { return _FliterTextLocator(this).Value; }
            set
            {
               _FliterTextLocator(this).SetValueAndTryNotify(value);
                SaveSetting();
                TopicTypes = new IncrementalPageLoadingCollection<IncrementalTopicTypePageSource, TopicType>(value, 0, 20);

            }
        }
        #region Property string FliterText Setup        
        protected Property<string> _FliterText = new Property<string> { LocatorFunc = _FliterTextLocator };
        static Func<BindableBase, ValueContainer<string>> _FliterTextLocator = RegisterContainerLocator<string>("FliterText", model => model.Initialize("FliterText", ref model._FliterText, ref _FliterTextLocator, _FliterTextDefaultValueFactory));
        static Func<string> _FliterTextDefaultValueFactory = () => "";
        #endregion


    }

}

