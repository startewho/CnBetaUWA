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
using CnBetaUWA.DataBase;
using CnBetaUWA.Extensions;
using CnBetaUWA.Helper;
using CnBetaUWA.Models;
using MVVMSidekick.Utilities;

namespace CnBetaUWA.ViewModels
{

    [DataContract]
    public class SettingBookmarketPage_Model : ViewModelBase<SettingBookmarketPage_Model>
    {
        private bool _isLoaded;
        // If you have install the code sniplets, use "propvm + [tab] +[tab]" create a property。
        // 如果您已经安装了 MVVMSidekick 代码片段，请用 propvm +tab +tab 输入属性
        public SettingBookmarketPage_Model()
        {
            MarkedNewses = new ObservableCollection<News>();
            var markednews = NewsDataTable.QueryBookmarketNewes(true);
            if (markednews != null)
            {
                MarkedNewses.AddRange(markednews);
            }

        }
        protected override Task OnBindedViewLoad(IView view)
        {
            if (_isLoaded) return base.OnBindedViewLoad(view);
            SubscribeCommand();
            _isLoaded = true;
            return base.OnBindedViewLoad(view);

        }


        public ObservableCollection<News> MarkedNewses
        {
            get { return _MarkedNewsesLocator(this).Value; }
            set { _MarkedNewsesLocator(this).SetValueAndTryNotify(value); }
        }

        #region Property ObservableCollection<News> MarkedNewses Setup        

        protected Property<ObservableCollection<News>> _MarkedNewses = new Property<ObservableCollection<News>>
        {
            LocatorFunc = _MarkedNewsesLocator
        };

        private static Func<BindableBase, ValueContainer<ObservableCollection<News>>> _MarkedNewsesLocator =
            RegisterContainerLocator<ObservableCollection<News>>("MarkedNewses",
                model =>
                    model.Initialize("MarkedNewses", ref model._MarkedNewses, ref _MarkedNewsesLocator,
                        _MarkedNewsesDefaultValueFactory));

        private static Func<ObservableCollection<News>> _MarkedNewsesDefaultValueFactory =
            () => default(ObservableCollection<News>);

        #endregion


        private void SubscribeCommand()
        {

            LocalEventRouter.GetEventChannel<Object>()
                .Where(x => x.EventName == "DeleteSelectedItem")
                .Subscribe(
                    async e =>
                    {
                        var news = e.EventData as News;

                        if (news!=null)
                        {
                            MarkedNewses.Remove(news);
                            news.IsBookmarketed = false;
                            NewsDataTable.Add(news);
                        }

                        await TaskExHelper.Yield();

                    }
                ).DisposeWith(this);


           
        }
    }
}

