using MVVMSidekick.Views;
using CnBetaUWA;
using CnBetaUWA.ViewModels;
using System;


namespace MVVMSidekick.Startups
{
    internal static partial class StartupFunctions
    {
        static Action TopicTypesMangePageConfig =
           CreateAndAddToAllConfig(ConfigTopicTypesMangePage);

        public static void ConfigTopicTypesMangePage()
        {
            ViewModelLocator<SettingTopicTypesMangePage_Model>
                .Instance
                .Register(context =>
                    new SettingTopicTypesMangePage_Model())
                .GetViewMapper()
                .MapToDefault<SettingTopicTypesMangePage>();

        }
    }
}
