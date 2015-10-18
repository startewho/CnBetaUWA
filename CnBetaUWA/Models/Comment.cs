using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVVMSidekick.ViewModels;
namespace CnBetaUWA.Models
{
    public  class Comment:ViewModelBase<Comment>
    {

        public int AgainstCounter
        {
            get { return _AgainstCounterLocator(this).Value; }
            set { _AgainstCounterLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property int AgainstCounter Setup        
        protected Property<int> _AgainstCounter = new Property<int> { LocatorFunc = _AgainstCounterLocator };
        static Func<BindableBase, ValueContainer<int>> _AgainstCounterLocator = RegisterContainerLocator<int>("AgainstCounter", model => model.Initialize("AgainstCounter", ref model._AgainstCounter, ref _AgainstCounterLocator, _AgainstCounterDefaultValueFactory));
        static Func<int> _AgainstCounterDefaultValueFactory = () => default(int);
        #endregion


        public int SupportCounter
        {
            get { return _SupportCounterLocator(this).Value; }
            set { _SupportCounterLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property int SupportCounter Setup        
        protected Property<int> _SupportCounter = new Property<int> { LocatorFunc = _SupportCounterLocator };
        static Func<BindableBase, ValueContainer<int>> _SupportCounterLocator = RegisterContainerLocator<int>("SupportCounter", model => model.Initialize("SupportCounter", ref model._SupportCounter, ref _SupportCounterLocator, _SupportCounterDefaultValueFactory));
        static Func<int> _SupportCounterDefaultValueFactory = () => default(int);
        #endregion

      

        public string Content { get; set; }

        public string CreatTime { get; set; }

        public string UserName { get; set; }


    }
}
