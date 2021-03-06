﻿using System.Reactive;
using System.Reactive.Linq;
using MVVMSidekick.ViewModels;
using MVVMSidekick.Views;
using MVVMSidekick.Reactive;
using MVVMSidekick.Services;
using MVVMSidekick.Commands;
using CnBetaUWA;
using CnBetaUWA.ViewModels;
using System;
using System.Net;
using System.Windows;


namespace MVVMSidekick.Startups
{
    internal static partial class StartupFunctions
    {
        static Action SettingThemePageConfig =
           CreateAndAddToAllConfig(ConfigSettingThemePage);

        public static void ConfigSettingThemePage()
        {
            ViewModelLocator<SettingThemePage_Model>
                .Instance
                .Register(context =>
                    new SettingThemePage_Model())
                .GetViewMapper()
                .MapToDefault<SettingThemePage>();

        }
    }
}
