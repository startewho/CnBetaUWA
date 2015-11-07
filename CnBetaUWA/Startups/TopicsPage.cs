using MVVMSidekick.Views;
using CnBetaUWA;
using CnBetaUWA.ViewModels;
using System;


namespace MVVMSidekick.Startups
{
    internal static partial class StartupFunctions
    {
        static Action TopicsPageConfig =
           CreateAndAddToAllConfig(ConfigTopicsPage);

        public static void ConfigTopicsPage()
        {
            ViewModelLocator<TopicsPage_Model>
                .Instance
                .Register(context =>
                    new TopicsPage_Model())
                .GetViewMapper()
                .MapToDefault<TopicsPage>();

        }
    }
}
