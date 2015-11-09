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

namespace CnBetaUWA.ViewModels
{

    [DataContract]
    public class TopicTypesMangePage_Model : ViewModelBase<TopicTypesMangePage_Model>
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
          TopicTypes=new IncrementalPageLoadingCollection<IncrementalTopicTypePageSource, TopicType>("",0,20);
           
            return base.OnBindedViewLoad(view);
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





    }

}

