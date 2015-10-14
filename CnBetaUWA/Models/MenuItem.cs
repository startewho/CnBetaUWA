using System;
using System.Runtime.Serialization;
using MVVMSidekick.ViewModels;

namespace CnBetaUWA.Models
{
    [DataContract]
    public class MenuItem : BindableBase<MenuItem>
    {

        public string Icon
        {
            get { return _IconLocator(this).Value; }
            set { _IconLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property string Icon Setup        
        protected Property<string> _Icon = new Property<string> { LocatorFunc = _IconLocator };
        static Func<BindableBase, ValueContainer<string>> _IconLocator = RegisterContainerLocator("Icon", model => model.Initialize("Icon", ref model._Icon, ref _IconLocator, _IconDefaultValueFactory));
        static Func<string> _IconDefaultValueFactory = () => default(string);
        #endregion


        public string Title
        {
            get { return _TitleLocator(this).Value; }
            set { _TitleLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property string Title Setup        
        protected Property<string> _Title = new Property<string> { LocatorFunc = _TitleLocator };
        static Func<BindableBase, ValueContainer<string>> _TitleLocator = RegisterContainerLocator("Title", model => model.Initialize("Title", ref model._Title, ref _TitleLocator, _TitleDefaultValueFactory));
        static Func<string> _TitleDefaultValueFactory = () => default(string);
        #endregion


        public Type PageType
        {
            get { return _PageTypeLocator(this).Value; }
            set { _PageTypeLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property Type PageType Setup        
        protected Property<Type> _PageType = new Property<Type> { LocatorFunc = _PageTypeLocator };
        static Func<BindableBase, ValueContainer<Type>> _PageTypeLocator = RegisterContainerLocator("PageType", model => model.Initialize("PageType", ref model._PageType, ref _PageTypeLocator, _PageTypeDefaultValueFactory));
        static Func<Type> _PageTypeDefaultValueFactory = () => default(Type);
        #endregion

    }
}
