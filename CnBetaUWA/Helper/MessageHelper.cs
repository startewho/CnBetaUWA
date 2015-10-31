using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
