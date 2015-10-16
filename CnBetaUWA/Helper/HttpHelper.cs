using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Networking;
using Windows.UI.Popups;
using Windows.Web.Http;
using Windows.Web.Http.Filters;
using Windows.Web.Http.Headers;

namespace CnBetaUWA.Helper
{
    public class HttpHelper
    {
        private static readonly string UerAgent =
            "Mozilla/5.0 (Linux; U; Android 4.2.2; en-cn; MEmu Build/JDQ39E) AppleWebKit/534.30 (KHTML, like Gecko) Version/4.0 Mobile Safari/534.30";
       // private static readonly string HostName="http://www.cnbeta.com/";

        private static void CreateHttpClient(ref HttpClient httpClient)
        {
            httpClient?.Dispose();
            httpClient = new HttpClient();
           
            httpClient.DefaultRequestHeaders.UserAgent.Add(new HttpProductInfoHeaderValue("ie", UerAgent));
        }
      
        public static async Task<string> PostAsync(string posturi, string poststr,
            IEnumerable<KeyValuePair<string, string>> body)
        {
            HttpClient httpClient = null;
            CreateHttpClient(ref httpClient);
            var postData = new HttpFormUrlEncodedContent(body);
            string responseString;
            using (var response = await httpClient.PostAsync(new Uri(posturi), postData))
            {
                responseString = await response.Content.ReadAsStringAsync();
            }
            return responseString;
        }

        public static async Task<string> GetAsyn(string posturl)
        {
            HttpClient httpClient = null;
            CreateHttpClient(ref httpClient);
            var posturi=new Uri(posturl);
           // httpClient.DefaultRequestHeaders.Host =new HostName(posturi.Host);
            var response = await httpClient.GetAsync(posturi);
            string responseString = await response.Content.ReadAsStringAsync();
            return responseString;
        }


        internal static string SerializeHeaders(HttpResponseMessage response)
        {
            StringBuilder output = new StringBuilder();

            // We cast the StatusCode to an int so we display the numeric value (e.g., "200") rather than the
            // name of the enum (e.g., "OK") which would often be redundant with the ReasonPhrase.
            output.Append(((int)response.StatusCode) + " " + response.ReasonPhrase + "\r\n");

            SerializeHeaderCollection(response.Headers, output);
            SerializeHeaderCollection(response.Content.Headers, output);
            output.Append("\r\n");
            return output.ToString();
        }

        private static void SerializeHeaderCollection(
            IEnumerable<KeyValuePair<string, string>> headers,
            StringBuilder output)
        {
            foreach (var header in headers)
            {
                output.Append(header.Key + ": " + header.Value + "\r\n");
            }
        }

        public static string GetCoookie(Uri hostUri)
        {
            HttpBaseProtocolFilter filter = new HttpBaseProtocolFilter();
            HttpCookieCollection cookieCollection = filter.CookieManager.GetCookies(hostUri);
            return cookieCollection.Aggregate("", (current, cookie) => current + cookie.ToString());
        }

        public static void ClearCookies(Uri hostUri)
        {
            var filter = new HttpBaseProtocolFilter();
            var cookieCollection = filter.CookieManager.GetCookies(hostUri);
            foreach (var item in cookieCollection)
            {
                filter.CookieManager.DeleteCookie(item);
            }
        }

        public static int NowToTimestamp()
        {
            return ToTimestamp(DateTime.Now);
        }

        public static DateTime ToDateTime( string str)
        {
            var dtStart = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            var lTime = long.Parse(str + "0000000");
            var toNow = new TimeSpan(lTime);
            var dtResult = dtStart.Add(toNow);
            return dtResult;
        }

        public static int ToTimestamp(DateTime value)
        {
            TimeSpan span = (value - new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime());
            return (int) span.TotalSeconds;
        }


}
}
