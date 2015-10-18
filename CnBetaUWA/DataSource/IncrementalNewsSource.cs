using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.AllJoyn;
using Newtonsoft.Json.Linq;
using CnBetaUWA.Helper;
using CnBetaUWA.Models;

namespace CnBetaUWA.DataSource
{
    public class IncrementalNewsSource : IIncrementalSource<News>
    {
        private int StartSid { get; set; }
        private int EndSid { get; set; }

        private bool _firestLoad;

        public async Task<IEnumerable<News>> GetPagedItems(string query, int pageIndex, int pageSize)
        {
            var jsontext = await CnBetaHelper.GetNews(query, null, 0, EndSid);
            if (jsontext != null)
            {
                JObject postlist = JObject.Parse(jsontext);
                var jsonList = postlist.SelectToken("result");
                var list = jsonList?.Select(item => new News()
                {
                    Sid = (int) item["sid"],
                    TopicId = (int) item["topic"],
                    Title = (string) item["title"],
                    CreatTime = (string) item["pubtime"],
                    Summary = (string) item["summary"],
                    ThumbPicture = (string) item["thumb"],
                    TopictLogoPicture = (string) item["topic_logo"]
                    //Title = (string)item["title"],
                    //Des = (string)item["des"],
                    //Creattime = ((string)item["creation_time"]).ToDateTime(),
                    //Icon = AppStrings.HostUri + (string)item["icon"],
                    //Id = Convert.ToInt32((string)item["id"]),
                    //PostUrl = string.Format(AppStrings.PostUri, Convert.ToInt32((string)item["id"]), AppSettings.Instance.IsEnableImageMode ? "show" : "hide")
                }).ToList();
                if (!_firestLoad)
                {
                   StartSid = list.First().Sid;
                   _firestLoad = true;
                }

                EndSid = list.Last().Sid;
                return list;
            }
            return null;
        }

        public async Task<IEnumerable<News>> GetLastestItems(string query)
        {
            var jsontext = await CnBetaHelper.GetLastestNews(query, null, StartSid, 0);

            JObject postlist = JObject.Parse(jsontext);
            var jsonList = postlist.SelectToken("result");
           
            if (jsonList.Any())
            {
                var list = jsonList?.Select(item => new News()
                {
                    Sid = (int)item["sid"],
                    TopicId = (int)item["topic"],
                    Title = (string)item["title"],
                    CreatTime = (string)item["pubtime"],
                    Summary = (string)item["summary"],
                    ThumbPicture = (string)item["thumb"],
                    TopictLogoPicture = (string)item["topic_logo"]

                }).ToList();

                StartSid = list.First().Sid;

                return list;
            }
            return null;


        }


    }
    
}
