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
using Windows.Storage;
using CnBetaUWA.Helper;

namespace CnBetaUWA.ViewModels
{

    [DataContract]
    public class SettingCachePage_Model : ViewModelBase<SettingCachePage_Model>
    {
        // If you have install the code sniplets, use "propvm + [tab] +[tab]" create a property。
        // 如果您已经安装了 MVVMSidekick 代码片段，请用 propvm +tab +tab 输入属性
        public SettingCachePage_Model()
        {
            InitData();
        }


        private async void InitData()
        {
            var cachefolder = ApplicationData.Current.LocalFolder;
            var size = await IOHelper.GetFolderSize(cachefolder);
            CacheSize = size/1024/1024;

        }


        public double CacheSize
        {
            get { return _CacheSizeLocator(this).Value; }
            set { _CacheSizeLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property double CacheSize Setup        
        protected Property<double> _CacheSize = new Property<double> { LocatorFunc = _CacheSizeLocator };
        static Func<BindableBase, ValueContainer<double>> _CacheSizeLocator = RegisterContainerLocator<double>("CacheSize", model => model.Initialize("CacheSize", ref model._CacheSize, ref _CacheSizeLocator, _CacheSizeDefaultValueFactory));
        static Func<double> _CacheSizeDefaultValueFactory = () => default(double);
        #endregion


    }

}

