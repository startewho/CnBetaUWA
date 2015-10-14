using System;
using CnBetaUWA.ViewModels;
using MVVMSidekick.Views;

namespace CnBetaUWA.Startups
{
    internal static partial class StartupFunctions
    {
        static Action NewsPageConfig =
           CreateAndAddToAllConfig(ConfigNewsPage);

        public static void ConfigNewsPage()
        {
            ViewModelLocator<NewsPage_Model>
                .Instance
                .Register(context =>
                    new NewsPage_Model())
                .GetViewMapper()
                .MapToDefault<NewsPage>();

        }
    }
}
