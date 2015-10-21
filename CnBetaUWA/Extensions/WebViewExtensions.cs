using System;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Web;
using Windows.Web.Http;
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
               
                Uri url = webView.BuildLocalStreamUri("contentIdentifier", e.NewValue.ToString());

                webView.NavigateToLocalStreamUri(url,new StreamUriWinRTResolver());
               
                
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


    // 实现 IUriToStreamResolver 接口（用于将 url 以内容流的方式返回）

    public sealed class StreamUriWinRTResolver : IUriToStreamResolver
    {
        // IUriToStreamResolver 接口只有一个需要实现的方法
        // 根据 uri 返回对应的内容流
        public IAsyncOperation<IInputStream> UriToStreamAsync(Uri uri)
        {
            string path = uri.AbsolutePath;
            return GetContent(path).AsAsyncOperation();
        }

        // 根据 uri 返回对应的内容流
        private async Task<IInputStream> GetContent(string path)
        {
            string url;

            // 按需求修改 url 引用
            if (path.StartsWith("http"))
            {
                // http 方式获取内容数据
                var client = new HttpClient();
                var response = await client.GetAsync(new Uri(path), HttpCompletionOption.ResponseHeadersRead);
                return (await response.Content.ReadAsInputStreamAsync());
            }
            else if (path.StartsWith("/local/HtmlCache/"))
            {
                path = string.Format("ms-appdata://{0}", path);
                // 获取本地数据
                var fileRead = await StorageFile.GetFileFromApplicationUriAsync(new Uri(path));
                return await fileRead.OpenAsync(FileAccessMode.Read);
            }
            else
            {
                // 获取本地数据
                var fileRead = await StorageFile.GetFileFromApplicationUriAsync(new Uri(path, UriKind.Absolute));
                return await fileRead.OpenAsync(FileAccessMode.Read);
            }

        }
    }

}
