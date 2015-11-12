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
using Windows.UI;
using CnBetaUWA.Extensions;
using CnBetaUWA.Helper;
using Q42.WinRT.Storage;

namespace CnBetaUWA.ViewModels
{

    [DataContract]
    public class SettingThemePage_Model : ViewModelBase<SettingThemePage_Model>
    {
        // If you have install the code sniplets, use "propvm + [tab] +[tab]" create a property。
        // 如果您已经安装了 MVVMSidekick 代码片段，请用 propvm +tab +tab 输入属性
        public SettingThemePage_Model()
        {
            AccentColors = AccentColorsKeyNameDefault;
            InitData();
            PropScribe();
        }


        private void InitData()
        {
            var colorstirng = SettingsHelper.Get<string>(CnBetaHelper.SettingAccentColor);
            AccentColor=colorstirng.GetColorFromString();
        }

        private void PropScribe()
        {
            GetValueContainer(vm => vm.AccentColor).GetNewValueObservable().Subscribe(e =>
            {
                var color = e.EventArgs;
                AppViewHelper.SetAppView(color);
                SettingsHelper.Set(CnBetaHelper.SettingAccentColor, color.ToString());
            }).DisposeWith(this);
        }

        public List<Color> AccentColors
        {
            get { return _AccentColorsLocator(this).Value; }
            set
            {
                _AccentColorsLocator(this).SetValueAndTryNotify(value);

            }
        }
        #region Property List<Color> AccentColors Setup        
        protected Property<List<Color>> _AccentColors = new Property<List<Color>> { LocatorFunc = _AccentColorsLocator };
        static Func<BindableBase, ValueContainer<List<Color>>> _AccentColorsLocator = RegisterContainerLocator<List<Color>>("AccentColors", model => model.Initialize("AccentColors", ref model._AccentColors, ref _AccentColorsLocator, _AccentColorsDefaultValueFactory));
        static Func<List<Color>> _AccentColorsDefaultValueFactory = () => default(List<Color>);
        #endregion

        public Color AccentColor
        {
            get { return _AccentColorLocator(this).Value; }
            set
            {
                _AccentColorLocator(this).SetValueAndTryNotify(value);

            }
        }
        #region Property Color AccentColor Setup        
        protected Property<Color> _AccentColor = new Property<Color> { LocatorFunc = _AccentColorLocator };
        static Func<BindableBase, ValueContainer<Color>> _AccentColorLocator = RegisterContainerLocator<Color>("AccentColor", model => model.Initialize("AccentColor", ref model._AccentColor, ref _AccentColorLocator, _AccentColorDefaultValueFactory));
        static Func<Color> _AccentColorDefaultValueFactory = () => default(Color);
        #endregion


        private static readonly List<Color> AccentColorsKeyNameDefault = new List<Color>()
                {
                    Color.FromArgb(255, 0xff, 0x88, 0x00),
                    Color.FromArgb(255, 241, 13, 162),
                    Color.FromArgb(255, 240, 67, 98),
                    Color.FromArgb(255, 239, 95, 65),
                    Color.FromArgb(255, 46, 204, 113),
                    Color.FromArgb(255, 52, 152, 219),
                    Color.FromArgb(255, 155, 89, 182),
                    Color.FromArgb(255, 52, 73, 94),
                    Color.FromArgb(255, 22, 160, 133),
                    Color.FromArgb(255, 39, 174, 96),
                    Color.FromArgb(255, 41, 128, 185),
                    Color.FromArgb(255, 142, 68, 173),
                    Color.FromArgb(255, 44, 62, 80),
                    Color.FromArgb(255, 241, 196, 15),
                    Color.FromArgb(255, 230, 126, 34),
                    Color.FromArgb(255, 231, 76, 60),
                    Color.FromArgb(255, 243, 156, 18),
                    Color.FromArgb(255, 211, 84, 0),
                    Color.FromArgb(255, 192, 57, 43)
                };

    }

}

