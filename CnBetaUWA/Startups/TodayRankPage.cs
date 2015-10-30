using System;
using CnBetaUWA;
using CnBetaUWA.ViewModels;
using MVVMSidekick.Views;


namespace MVVMSidekick.Startups
{
    internal static partial class StartupFunctions
    {
        static Action TodayRankPageConfig =
           CreateAndAddToAllConfig(ConfigTodayRankPage);

        public static void ConfigTodayRankPage()
        {
            ViewModelLocator<TodayRankPage_Model>
                .Instance
                .Register(context =>
                    new TodayRankPage_Model())
                .GetViewMapper()
                .MapToDefault<TodayRankPage>();

        }
    }
}
