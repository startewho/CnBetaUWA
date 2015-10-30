using System.Reactive;
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
