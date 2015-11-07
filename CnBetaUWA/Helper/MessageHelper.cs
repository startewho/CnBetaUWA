using MVVMSidekick.EventRouting;

namespace CnBetaUWA.Helper
{
     public static class MessageHelper
    {
         public static void ShowMessage(object sender, string message)
         {
             EventRouter.Instance.GetEventChannel(typeof (string))
                 .RaiseEvent(sender, "ToastMessageByEventRouter", message, true, true);
         }
    }
}
