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
using System.IO;
using System.Runtime.Serialization;
using Windows.Storage;
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
          
            GetData();
           
            return base.OnBindedViewLoad(view);

        }




        public List<TopicType> TopicTypes
        {
            get { return _TopicTypesLocator(this).Value; }
            set { _TopicTypesLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property List<TopicType> TopicTypes Setup        
        protected Property<List<TopicType>> _TopicTypes = new Property<List<TopicType>> { LocatorFunc = _TopicTypesLocator };
        static Func<BindableBase, ValueContainer<List<TopicType>>> _TopicTypesLocator = RegisterContainerLocator<List<TopicType>>("TopicTypes", model => model.Initialize("TopicTypes", ref model._TopicTypes, ref _TopicTypesLocator, _TopicTypesDefaultValueFactory));
        static Func<List<TopicType>> _TopicTypesDefaultValueFactory = () => default(List<TopicType>);
        #endregion

        private async void GetData()
        {
            var jsontext=await IOHelper.GetTextFromStorage(new Uri("ms-appx:///Data/CnbetaAllTopics.json"));
            TopicTypes = ModelHelper.JsonToTopicTypes(jsontext).ToList();
        }

    }

}

