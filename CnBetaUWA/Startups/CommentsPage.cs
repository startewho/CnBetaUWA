using System;
using CnBetaUWA.ViewModels;
using MVVMSidekick.Views;


namespace CnBetaUWA.Startups
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
