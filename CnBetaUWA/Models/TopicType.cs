using System;
using MVVMSidekick.ViewModels;
using Newtonsoft.Json;
namespace CnBetaUWA.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public   class TopicType:ViewModelBase<TopicType>
    {
        [JsonProperty]
        public string NamePre { get; set; }

        [JsonProperty]
        public string Name { get; set; }

        [JsonProperty]
        public int Id { get; set; }

        [JsonProperty]
        public string LogoUrl { get; set; }

        [JsonProperty]
        public bool IsSelected
        {
            get { return _IsSelectedLocator(this).Value; }
            set { _IsSelectedLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property bool IsSelected Setup        
        protected Property<bool> _IsSelected = new Property<bool> { LocatorFunc = _IsSelectedLocator };
        static Func<BindableBase, ValueContainer<bool>> _IsSelectedLocator = RegisterContainerLocator<bool>("IsSelected", model => model.Initialize("IsSelected", ref model._IsSelected, ref _IsSelectedLocator, _IsSelectedDefaultValueFactory));
        static Func<bool> _IsSelectedDefaultValueFactory = () => default(bool);
        #endregion

    }
}
