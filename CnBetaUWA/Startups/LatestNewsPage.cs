using System;
using CnBetaUWA;
using CnBetaUWA.ViewModels;
using MVVMSidekick.Views;

namespace MVVMSidekick.Startups
{
    internal static partial class StartupFunctions
    {
        static Action LatestNewsConfig =
           CreateAndAddToAllConfig(ConfigLatestNews);

        public static void ConfigLatestNews()
        {
            ViewModelLocator<LatestNewsPage_Model>
                .Instance
                .Register(context =>
                    new LatestNewsPage_Model())
                .GetViewMapper()
                .MapToDefault<LatestNewsPage>();

        }
    }
}
