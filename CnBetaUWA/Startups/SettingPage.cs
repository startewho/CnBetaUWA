using MVVMSidekick.Views;
using CnBetaUWA;
using CnBetaUWA.ViewModels;
using System;


namespace MVVMSidekick.Startups
{
    internal static partial class StartupFunctions
    {
        static Action SettingPageConfig =
           CreateAndAddToAllConfig(ConfigSettingPage);

        public static void ConfigSettingPage()
        {
            ViewModelLocator<SettingPage_Model>
                .Instance
                .Register(context =>
                    new SettingPage_Model())
                .GetViewMapper()
                .MapToDefault<SettingPage>();

        }
    }
}
