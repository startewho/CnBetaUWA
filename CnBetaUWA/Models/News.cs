using System;
using System.Collections.Generic;
using MVVMSidekick.ViewModels;
using Newtonsoft.Json;

namespace CnBetaUWA.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class News:ViewModelBase<News>
    {
        [JsonProperty]

        public string Title { get; set; }
        [JsonProperty]

        public string Summary { get; set; }

        [JsonProperty]
        public string CreatTime { get; set; }

        [JsonProperty]
        public string ThumbPicture { get; set; }

        [JsonProperty]
        public int Sid { get; set; }
        [JsonProperty]
        public int ViewCount { get; set; }

        [JsonProperty]
        public int CommentsCount { get; set; }

        [JsonProperty]
        public int TopicId { get; set; }
        [JsonProperty]
        public string TopictLogoPicture { get; set; }

        [JsonProperty]
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


        [JsonProperty]
        public List<NewsComment> NewsComments
        {
            get { return _NewsCommentsLocator(this).Value; }
            set { _NewsCommentsLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property List<NewsComment> NewsComments Setup        
        protected Property<List<NewsComment>> _NewsComments = new Property<List<NewsComment>> { LocatorFunc = _NewsCommentsLocator };
        static Func<BindableBase, ValueContainer<List<NewsComment>>> _NewsCommentsLocator = RegisterContainerLocator<List<NewsComment>>("NewsComments", model => model.Initialize("NewsComments", ref model._NewsComments, ref _NewsCommentsLocator, _NewsCommentsDefaultValueFactory));
        static Func<List<NewsComment>> _NewsCommentsDefaultValueFactory = () => default(List<NewsComment>);
        #endregion

       
    }
}
