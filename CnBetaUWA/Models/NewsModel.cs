using System;
using System.Collections.Generic;
using MVVMSidekick.ViewModels;

namespace CnBetaUWA.Models
{
    public class NewsModel:ViewModelBase<NewsModel>
    {
        public string Title { get; set; }

        public string Summary { get; set; }
        public string CreatTime { get; set; }

        public string ThumbPicture { get; set; }

        public int Sid { get; set; }
        public int ViewCount { get; set; }

        public int CommentsCount { get; set; }

        public int TopicId { get; set; }
        public string TopictLogoPicture { get; set; }


        public NewsContent NewsContent
        {
            get { return _NewsContentLocator(this).Value; }
            set { _NewsContentLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property NewsContent NewsContent Setup        
        protected Property<NewsContent> _NewsContent = new Property<NewsContent> { LocatorFunc = _NewsContentLocator };
        static Func<BindableBase, ValueContainer<NewsContent>> _NewsContentLocator = RegisterContainerLocator<NewsContent>("NewsContent", model => model.Initialize("NewsContent", ref model._NewsContent, ref _NewsContentLocator, _NewsContentDefaultValueFactory));
        static Func<NewsContent> _NewsContentDefaultValueFactory = () => default(NewsContent);
        #endregion

      

        public List<NewsComment> NewsComments { get; set; }
    }
}
