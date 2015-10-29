using System;
using MVVMSidekick.ViewModels;
namespace CnBetaUWA.Models
{
   public class NewsComment:ViewModelBase<NewsComment>
   {
       private string _userName;
       private bool _isShow;
       private NewsComment _pidComment;


        public int Support
        {
            get { return _SupportLocator(this).Value; }
            set { _SupportLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property int Support Setup        
        protected Property<int> _Support = new Property<int> { LocatorFunc = _SupportLocator };
        static Func<BindableBase, ValueContainer<int>> _SupportLocator = RegisterContainerLocator<int>("Support", model => model.Initialize("Support", ref model._Support, ref _SupportLocator, _SupportDefaultValueFactory));
        static Func<int> _SupportDefaultValueFactory = () => default(int);
        #endregion



        public int Against
        {
            get { return _AgainstLocator(this).Value; }
            set { _AgainstLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property int Against Setup        
        protected Property<int> _Against = new Property<int> { LocatorFunc = _AgainstLocator };
        static Func<BindableBase, ValueContainer<int>> _AgainstLocator = RegisterContainerLocator<int>("Against", model => model.Initialize("Against", ref model._Against, ref _AgainstLocator, _AgainstDefaultValueFactory));
        static Func<int> _AgainstDefaultValueFactory = () => default(int);
        #endregion

        public bool IsSupported
        {
            get { return _IsSupportedLocator(this).Value; }
            set { _IsSupportedLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property bool IsSupported Setup        
        protected Property<bool> _IsSupported = new Property<bool> { LocatorFunc = _IsSupportedLocator };
        static Func<BindableBase, ValueContainer<bool>> _IsSupportedLocator = RegisterContainerLocator<bool>("IsSupported", model => model.Initialize("IsSupported", ref model._IsSupported, ref _IsSupportedLocator, _IsSupportedDefaultValueFactory));
        static Func<bool> _IsSupportedDefaultValueFactory = () => default(bool);
        #endregion



        public bool IsAgainsted
        {
            get { return _IsAgainstedLocator(this).Value; }
            set { _IsAgainstedLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property bool IsAgainsted Setup        
        protected Property<bool> _IsAgainsted = new Property<bool> { LocatorFunc = _IsAgainstedLocator };
        static Func<BindableBase, ValueContainer<bool>> _IsAgainstedLocator = RegisterContainerLocator<bool>("IsAgainsted", model => model.Initialize("IsAgainsted", ref model._IsAgainsted, ref _IsAgainstedLocator, _IsAgainstedDefaultValueFactory));
        static Func<bool> _IsAgainstedDefaultValueFactory = () => default(bool);
        #endregion



        public int Tid { get; set; }

       public int Pid { get; set; }

     

       public string Content { get; set; }
       public string CreatTime { get; set; }

       public NewsComment PidComment
       {
           get { return _pidComment; }
           set { _pidComment = value; }
       }

       public string UserName
        {
            get
            {
                return _userName==string.Empty ? "匿名用户" : _userName;
            }

            set
            {
                _userName = value;
            }
        }

       public bool IsShow
       {
           get { return Pid>0; }
           set { _isShow = value; }
       }
    }
}
