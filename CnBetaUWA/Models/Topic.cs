using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.NetworkOperators;
using CnBetaUWA.DataSource;
using CnBetaUWA.Helper;
using MVVMSidekick.ViewModels;

namespace CnBetaUWA.Models
{
    public class Topic:ViewModelBase<Topic>
    {
        public Topic(TopicType topicType)
        {
            CurrentTopicType = topicType;
        }

        public void InitNewsSourceColletion(int strat,int end, IEnumerable<News> cachenewses)
        {
            NewsSourceCollection=new IncrementalLoadingCollection<IncrementalNewsSource, News>(CnBetaHelper.TypeAll, CurrentTopicType.Id.ToString(), strat, end, cachenewses);
        }

        public TopicType CurrentTopicType { get; set; }

        public bool IsSelected
        {
            get { return _IsSelectedLocator(this).Value; }
            set { _IsSelectedLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property bool IsSelected Setup        
        protected Property<bool> _IsSelected = new Property<bool> { LocatorFunc = _IsSelectedLocator };
        static Func<BindableBase, ValueContainer<bool>> _IsSelectedLocator = RegisterContainerLocator("IsSelected", model => model.Initialize("IsSelected", ref model._IsSelected, ref _IsSelectedLocator, _IsSelectedDefaultValueFactory));
        static Func<bool> _IsSelectedDefaultValueFactory = () => false;
        #endregion

       
        public IncrementalLoadingCollection<IncrementalNewsSource,News> NewsSourceCollection
        {
            get { return _NewsSourceCollectionLocator(this).Value; }
            set { _NewsSourceCollectionLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property IncrementalLoadingCollection<IncrementalNewsSource,News> NewsSourceCollection Setup        
        protected Property<IncrementalLoadingCollection<IncrementalNewsSource,News>> _NewsSourceCollection = new Property<IncrementalLoadingCollection<IncrementalNewsSource,News>> { LocatorFunc = _NewsSourceCollectionLocator };
        static Func<BindableBase, ValueContainer<IncrementalLoadingCollection<IncrementalNewsSource,News>>> _NewsSourceCollectionLocator = RegisterContainerLocator<IncrementalLoadingCollection<IncrementalNewsSource,News>>("NewsSourceCollection", model => model.Initialize("NewsSourceCollection", ref model._NewsSourceCollection, ref _NewsSourceCollectionLocator, _NewsSourceCollectionDefaultValueFactory));
        static Func<IncrementalLoadingCollection<IncrementalNewsSource,News>> _NewsSourceCollectionDefaultValueFactory = () => default(IncrementalLoadingCollection<IncrementalNewsSource,News>);
        #endregion
        
    }
}
