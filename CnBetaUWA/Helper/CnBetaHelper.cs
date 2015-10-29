using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;
// ReSharper disable InconsistentNaming

namespace CnBetaUWA.Helper
{
    public   class CnBetaHelper
    {
        public static string TypeAll = "Article.Lists";
        public static string TypeRecommend = "Article.TodayRank";
        public static string TypeTop10 = "Article.Top10";
        public static string TypeHotComment = "Article.RecommendComment";
        public static string TypeNewsContent = "Article.NewsContent";
        public static string TypeNewsComment = "Article.Comment";
        public static string TypeNewsCommentAction = "Article.DoCmt";
        public static string TypeRealtime = "realtime";
        
        public static string ApiUrl { get; } = "http://api.cnbeta.com/capi?";
        public static string HtmlPath = "/LocalCache/HtmlCache/{0}.html";
        public static string HtmlFolder = "HtmlCache/";



        public static string SettingNightMode = nameof(SettingNightMode);
        public static string SettingImageMode = nameof(SettingImageMode);
        public static string SettingFontSize = nameof(SettingFontSize);

        public static string GetRealTimeNewsUri()
        {
            var url = ApiUrl + "jsoncallback=jQuery18008753548712314047_" + HttpHelper.ToTimestamp(DateTime.Now)
                         + "s&type=" + TypeRealtime + "&&_=" + (HttpHelper.ToTimestamp(DateTime.Now) + 1);
            return url;
        }

        public static async Task<string> GetNews(string type,Artiletype? todaytype ,int endSid)
        {
            var query = "app_key=10000";

            //if (startSid != 0)
            //    query += "&start_sid=" + startSid;
            if (endSid != 0)
                query += "&end_sid=" + endSid;

            query += "&format=json&method=" + type;
            
            query += "&timestamp="+ HttpHelper.ToTimestamp(DateTime.Now);
            if (todaytype != null)
            {
                query += "&type=" + todaytype;
            }

            query += "&v=1.0";
            query += "&sign=" + ComputeMd5(query + "&mpuffgvbvbttn3Rc");
            var contentjson = await HttpHelper.GetAsyn(ApiUrl + query);
            return contentjson;
           
        }

        /// <summary>
        /// 取得最新News时,不需要endSid.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="todaytype"></param>
        /// <param name="startSid"></param>
        /// <param name="endSid"></param>
        /// <returns></returns>
        public static async Task<string> GetLastestNews(string type, Artiletype? todaytype, int startSid, int endSid)
        {
            var query = "app_key=10000";

            query += "&format=json&method=" + type;

            if (startSid != 0)
                query += "&start_sid=" + startSid;
            if (endSid != 0)
                query += "&end_sid=" + endSid;

            query += "&timestamp=" + HttpHelper.ToTimestamp(DateTime.Now);
            if (todaytype != null)
            {
                query += "&type=" + todaytype;
            }

            query += "&v=1.0";
            query += "&sign=" + ComputeMd5(query + "&mpuffgvbvbttn3Rc");
            var contentjson = await HttpHelper.GetAsyn(ApiUrl + query);
            return contentjson;

        }


        public static async Task<string> GetNewsByTopicId(string type, int topicid, int startSid, int endSid)
        {
            var query = "app_key=10000";
            query += "&format=json&method=" + type;

            if (startSid != 0)
                query += "&end_sid=" + startSid;
            if (endSid != 0)
                query += "&end_sid=" + endSid;

            query += "&timestamp=" + HttpHelper.ToTimestamp(DateTime.Now);
            query += "&type=" + topicid;
            query += "&v=1.0";
            query += "&sign=" + ComputeMd5(query + "&mpuffgvbvbttn3Rc");
            var contentjson = await HttpHelper.GetAsyn(ApiUrl + query);
            return contentjson;

        }

        public static async Task<string> GetHotComments()
        {
            var query = "app_key=10000&format=json&method=" + TypeHotComment+ "&timestamp=" +HttpHelper.NowToTimestamp();
            query += "&v=1.0";
            query += "&sign=" + ComputeMd5(query + "&mpuffgvbvbttn3Rc");
            var contentjson = await HttpHelper.GetAsyn(ApiUrl + query);
            return contentjson;
        }


        public static async Task<string> GetNewsContent(int sid)
        {
            var query = "app_key=10000&format=json&method=" + TypeNewsContent
                + "&sid=" + sid + "&timestamp=" + HttpHelper.NowToTimestamp();
            query += "&v=1.0";
            query += "&sign=" + ComputeMd5(query + "&mpuffgvbvbttn3Rc");
            var contentjson = await HttpHelper.GetAsyn(ApiUrl + query);
            return contentjson;
        }

        public static async Task<string> GetNewsComment(int sid,int pageIndex,int pageSize)
        {
            var query = "app_key=10000&format=json&method=" + TypeNewsComment+"&page="+pageIndex+"&pageSize="+pageSize
                + "&sid=" + sid + "&timestamp=" + HttpHelper.NowToTimestamp();
            query += "&v=1.0";
            query += "&sign=" + ComputeMd5(query + "&mpuffgvbvbttn3Rc");
            var contentjson = await HttpHelper.GetAsyn(ApiUrl + query);
            return contentjson;
        }

        public static async Task<string> GetCommentAction(int sid,int tid,string opertor)
        {
            var query = "app_key=10000&format=json&method=" + TypeNewsCommentAction+"&op="+opertor
                + "&sid=" + sid+"&tid="+tid + "&timestamp=" + HttpHelper.NowToTimestamp();
            query += "&v=1.0";
            query += "&mpuffgvbvbttn3Rc";
            query += "&sign=" + ComputeMd5(query );
            var contentjson = await HttpHelper.GetAsyn(ApiUrl + query);
            return contentjson;
        }



        public static string ComputeMd5(string str)
        {
            var alg = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Md5);
            IBuffer buff = CryptographicBuffer.ConvertStringToBinary(str, BinaryStringEncoding.Utf8);
            var hashed = alg.HashData(buff);
            var res = CryptographicBuffer.EncodeToHexString(hashed);
            return res;
        }

        public static string GetCurrentTimeMiles()
        {
            return DateTime.Now.Millisecond.ToString();
        }
      
    }

    public  enum Artiletype
    {
       comments,
       dig,
       counter
    } 
}
