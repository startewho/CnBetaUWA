using System;
using System.Linq;
using System.Reflection;
using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace CnBetaUWA.Helper
{
    public  static class AppViewHelper
    {
        private const double DefaultTitleBarHeight = 40;

        public static double TitleBarHeight
        {
            get
            {
                return DefaultTitleBarHeight;
            }
        }

        public static double SetTitleBarHeight()
        {

            var titleBarInstance = GetTitleBarInstanceOnW10();
            if (titleBarInstance == null) return DefaultTitleBarHeight;
            if (titleBarInstance.Height == 0) return DefaultTitleBarHeight;
            return titleBarInstance.Height;

        }

        public static void SetTitleBar(bool extend)
        {
            var titleBarInstance = GetTitleBarInstanceOnW10();
            if (titleBarInstance == null) return;
            titleBarInstance.ExtendViewIntoTitleBar = extend;
        }

        public static dynamic GetTitleBarInstanceOnW10()
        {
            var coreAppView = CoreApplication.GetCurrentView();
            var allProperties = coreAppView.GetType().GetRuntimeProperties();
            var titleBar = allProperties.FirstOrDefault(x => x.Name == "TitleBar");
            dynamic titleBarInstance = titleBar?.GetMethod.Invoke(coreAppView, null);
            return titleBarInstance;
        }

        private static bool DoesPropertyExist(string prop, dynamic list)
        {
            foreach (dynamic property in list)
            {
                if (property.Name == prop)
                    return true;
            }
            return false;
        }

        public static void SetAppView(Color fgColor)
        {
            PropertyInfo titleBar;
            try
            {
                var v = ApplicationView.GetForCurrentView();
                var allProperties = v.GetType().GetRuntimeProperties();
                titleBar = allProperties.FirstOrDefault(x => x.Name == "TitleBar");
                if (titleBar == null) return;
                dynamic bb = titleBar.GetMethod.Invoke(v, null);
                if (bb != null)
                {
                    var appViewProperties = bb.GetType().DeclaredProperties;
                    bb.BackgroundColor = Color.FromArgb(0, 0, 0, 0);
                    bb.ForegroundColor = fgColor;
                    bb.ButtonForegroundColor = fgColor;
                    bb.ButtonBackgroundColor = Color.FromArgb(0, 0, 0, 0);

                    if (DoesPropertyExist("InactiveBackgroundColor", appViewProperties))
                        bb.InactiveBackgroundColor = Color.FromArgb(0, 0, 0, 0);
                    if (DoesPropertyExist("ButtonInactiveForegroundColor", appViewProperties))
                        bb.ButtonInactiveForegroundColor = bb.ButtonForegroundColor;
                    if (DoesPropertyExist("InactiveForegroundColor", appViewProperties))
                        bb.InactiveForegroundColor = bb.ButtonForegroundColor;
                    if (DoesPropertyExist("ButtonInactiveBackgroundColor", appViewProperties))
                        bb.ButtonInactiveBackgroundColor = bb.BackgroundColor;
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }


      public static void FrameFresh(Frame frame, ElementTheme currenttheme)
      {
          if (frame == null) throw new ArgumentNullException(nameof(frame));
          switch (currenttheme)
            {
                case ElementTheme.Dark:
                    frame.RequestedTheme = ElementTheme.Light;
                    break;
                case ElementTheme.Light:
                    frame.RequestedTheme = ElementTheme.Dark;
                    break;
                case ElementTheme.Default:
                    break;
                default:
                    frame.RequestedTheme = ElementTheme.Dark;
                    break;

            }
      }
    }
}
