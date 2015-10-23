using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CnBetaUWA.Models;
using Newtonsoft.Json.Linq;

namespace CnBetaUWA.Helper
{
    public static class ModelHelper
    {
        public static NewsContent JsonToNewsContent(string jsontext)
        {
            JObject postlist = JObject.Parse(jsontext);
            var selectToken = postlist.SelectToken("result");
            var newscotent = new NewsContent
            {
                Sid =(int)selectToken["sid"],
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
           
            JObject postlist = JObject.Parse(jsontext);
            var selectToken = postlist.SelectToken("result");
            var list =selectToken.Select(item=> new NewsComment
            {
                Against = (int)item["against"],
                Support = (int)item["support"],
                Content = (string)item["content"],
                CreatTime = (string)item["created_time"],
                Tid = (int)item["tid"],
                UserName = (string)item["username"],
              });

            return list;
        }
    }
}
