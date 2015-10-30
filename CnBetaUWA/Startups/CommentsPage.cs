using System;
using CnBetaUWA;
using CnBetaUWA.ViewModels;
using MVVMSidekick.Views;


namespace MVVMSidekick.Startups
{
    internal static partial class StartupFunctions
    {
        static Action CommentsPageConfig =
           CreateAndAddToAllConfig(ConfigCommentsPage);

        public static void ConfigCommentsPage()
        {
            ViewModelLocator<CommentsPage_Model>
                .Instance
                .Register(context =>
                    new CommentsPage_Model())
                .GetViewMapper()
                .MapToDefault<CommentsPage>();

        }
    }
}
