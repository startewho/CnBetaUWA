using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using CnBetaUWA.Helper;

namespace CnBetaUWA.Extensions
{
    public static class WebViewExtensions
    {

        #region UriSource

        public static string GetUriSource(WebView view)
        {
            return (string)view.GetValue(UriSourceProperty);
        }

        public static void SetUriSource(WebView view, string value)
        {
            view.SetValue(UriSourceProperty, value);
        }

        public static readonly DependencyProperty UriSourceProperty =
            DependencyProperty.RegisterAttached(
            "UriSource", typeof(string), typeof(WebViewExtensions),
            new PropertyMetadata(null, OnUriSourcePropertyChanged));

        private static async void OnUriSourcePropertyChanged(DependencyObject sender,
            DependencyPropertyChangedEventArgs e)
        {
            var webView = sender as WebView;
            if (webView == null)
                throw new NotSupportedException();

            if (e.NewValue != null)
            {
           
                webView.Settings.IsJavaScriptEnabled = true;
                var url = e.NewValue as string;
                var html =await HttpHelper.GetAsyn(url);
                 html= HrefAddHost(html);
                webView.NavigateToString(html);
                
            }

        }

        #endregion


        #region HtmlText



        public static string GetHtmlText(DependencyObject obj)
        {
            return (string)obj.GetValue(HtmlTextProperty);
        }

        public static void SetHtmlText(DependencyObject obj, string value)
        {
            obj.SetValue(HtmlTextProperty, value);
        }

        // Using a DependencyProperty as the backing store for HtmlText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HtmlTextProperty =
            DependencyProperty.RegisterAttached("HtmlText", typeof(string), typeof(WebViewExtensions), new PropertyMetadata(null,OnHtmlTextSourcePropertyChanged));

        private static void OnHtmlTextSourcePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var webView = sender as WebView;
            if (webView == null)
                throw new NotSupportedException();

            if (e.NewValue != null)
            {
                webView.Settings.IsJavaScriptEnabled = true;
                webView.Settings.IsIndexedDBEnabled = true;
                webView.NavigateToString(e.NewValue.ToString());
            }
        }

        #endregion


         public static string HrefAddHost(string originaltext)
        {

            return originaltext;
        }
      
    }

}
