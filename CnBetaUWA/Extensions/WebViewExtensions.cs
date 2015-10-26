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
using Q42.WinRT.Data;
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

        private static  void OnUriSourcePropertyChanged(DependencyObject sender,
            DependencyPropertyChangedEventArgs e)
        {
            var webView = sender as WebView;
            if (webView == null)
                throw new NotSupportedException();

            if (e.NewValue != null)
            {
           
                webView.Settings.IsJavaScriptEnabled = true;
               
                Uri url = webView.BuildLocalStreamUri("NewsContent", e.NewValue.ToString());

              
                webView.NavigateToLocalStreamUri(url,new StreamUriWinRtResolver());
               
                
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

    public sealed class StreamUriWinRtResolver : IUriToStreamResolver
    {
        // IUriToStreamResolver 接口只有一个需要实现的方法
        // 根据 uri 返回对应的内容流
        public IAsyncOperation<IInputStream> UriToStreamAsync(Uri uri)
        {
            string path = uri.AbsolutePath;
            return GetContent(path).AsAsyncOperation();
        }

        // 根据 uri 返回对应的内容流
        private static async Task<IInputStream> GetContent(string path)
        {
          
            if (path.StartsWith("/LocalCache/HtmlCache/"))
            {
                if (path.Contains("article")||path.Contains("topics"))
                {
                    //缓存图片
                    path = path.Replace("/LocalCache/HtmlCache/", "");
                    path = string.Format("http://static.cnbetacdn.com/{0}", path);
                    var webfile = await WebDataCache.GetAsync(new Uri(path));
                    return await webfile.OpenAsync(FileAccessMode.Read);
                }

                // 获取本地数据
                path = string.Format("ms-appdata://{0}", path);
                var filedata = await StorageFile.GetFileFromApplicationUriAsync(new Uri(path));
                return await filedata.OpenAsync(FileAccessMode.Read);
            }

          

            return null;


        }
    }

}
