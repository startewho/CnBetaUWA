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
using CnBetaUWA.Helper;
using CnBetaUWA.Models;
using ImageLib.Helpers;
using MVVMSidekick.Collections.CollectionView;

namespace CnBetaUWA.ViewModels
{

    [DataContract]
    public class Top10Page_Model : ViewModelBase<Top10Page_Model>
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


        protected  override Task OnBindedViewLoad(IView view)
        {
            InitData();
            return base.OnBindedViewLoad(view);
        }

         private async void InitData()
        {
            var jsontext = await CnBetaHelper.GetNews(CnBetaHelper.TypeTop10, null, 0);
            var lists = ModelHelper.JsonToNewses(jsontext).ToList();
            if (lists != null)
            {
                Top10NewsCollection = new ObservableCollection<News>();
                Top10NewsCollection.AddRange(lists);
                for (int i = 0; i < Top10NewsCollection.Count; i++)
                {
                    Top10NewsCollection[i].Index = i + 1;
                }
            }
        }


        public ObservableCollection<News> Top10NewsCollection
        {
            get { return _Top10NewsCollectionLocator(this).Value; }
            set { _Top10NewsCollectionLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property ObservableCollection<News> Top10NewsCollection Setup        
        protected Property<ObservableCollection<News>> _Top10NewsCollection = new Property<ObservableCollection<News>> { LocatorFunc = _Top10NewsCollectionLocator };
        static Func<BindableBase, ValueContainer<ObservableCollection<News>>> _Top10NewsCollectionLocator = RegisterContainerLocator<ObservableCollection<News>>("Top10NewsCollection", model => model.Initialize("Top10NewsCollection", ref model._Top10NewsCollection, ref _Top10NewsCollectionLocator, _Top10NewsCollectionDefaultValueFactory));
        static Func<ObservableCollection<News>> _Top10NewsCollectionDefaultValueFactory = () => default(ObservableCollection<News>);
        #endregion


    }

}

