using System;
using CnBetaUWA.ViewModels;
using MVVMSidekick.Views;

namespace CnBetaUWA.Startups
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
