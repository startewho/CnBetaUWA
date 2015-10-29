using System.Collections.Generic;
using System.Linq;
using CnBetaUWA.Models;
using Newtonsoft.Json.Linq;

namespace CnBetaUWA.Helper
{
    public static class ModelHelper
    {
        public static NewsContent JsonToNewsContent(string jsontext)
        {
            var postlist = JObject.Parse(jsontext);
            var selectToken = postlist.SelectToken("result");
            var newscotent = new NewsContent
            {
                Sid =(int)selectToken["sid"],
                Author =(string)selectToken["aid"],
                InputTime = (string)selectToken["inputtime"],
                CommentCount = (int)selectToken["comments"],
                BodyText = (string)selectToken["bodytext"],
                HomeText = (string)selectToken["hometext"],
                Source = (string)selectToken["source"]
              };

            return newscotent;
        }

        public static IEnumerable<NewsComment> JsonToNewsComments(string jsontext)
        {
           
            var postlist = JObject.Parse(jsontext);
            var selectToken = postlist.SelectToken("result");
            var list =selectToken.Select(item=> new NewsComment
            {
                Against = (int)item["against"],
                Support = (int)item["support"],
                Pid = (int)item["pid"],
                Content = (string)item["content"],
                CreatTime = (string)item["created_time"],
                Tid = (int)item["tid"],
                UserName = (string)item["username"],
              }).ToList();
           
            return list;
        }


        public static IEnumerable<News> JsonToNewses(string jsontext)
        {

            var postlist = JObject.Parse(jsontext);
            var jsonList = postlist.SelectToken("result");

            if (jsonList.Any())
            {
                var list = jsonList?.Select(item => new News()
                {
                    Sid = (int)item["sid"],
                    TopicId = (int)item["topic"],
                    Title = (string)item["title"],
                    CommentsCount =(int)item["comments"],
                    ViewCount = (int)item["counter"],
                    CreatTime = (string)item["pubtime"],
                    Summary = (string)item["summary"],
                    ThumbPicture = (string)item["thumb"],
                    TopictLogoPicture = (string)item["topic_logo"]
                }).ToList();

                return list;
            }

            return null;


        }


        public static bool JsonToCommentAction(string jsontext)
        {
            var postlist = JObject.Parse(jsontext);
            var result =(string)postlist["status"];
            return result=="success";
        }
    }
}
