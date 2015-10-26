using System;
using CnBetaUWA;
using CnBetaUWA.ViewModels;
using MVVMSidekick.Views;



namespace CnBetaUWA.Startups
{
    internal static partial class StartupFunctions
    {
        static Action Top10PageConfig =
           CreateAndAddToAllConfig(ConfigTop10Page);

        public static void ConfigTop10Page()
        {
            ViewModelLocator<Top10Page_Model>
                .Instance
                .Register(context =>
                    new Top10Page_Model())
                .GetViewMapper()
                .MapToDefault<Top10Page>();

        }
    }
}
