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

        private bool _firestGetPagedItems;
        private bool _firesGetLastestItems;

        public async Task<IEnumerable<News>> GetPagedItems(string query, int startindex, int endindex)
        {
            if (!_firestGetPagedItems)
            {
                StartSid = startindex;
                EndSid = endindex;
               _firestGetPagedItems = true;
            }

            var jsontext = await CnBetaHelper.GetNews(query, null, StartSid, EndSid);
            if (jsontext == null) return null;

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
                   
            }).ToList();
            
            StartSid = list.First().Sid;
            EndSid = list.Last().Sid;
            return list;
        }

        public async Task<IEnumerable<News>> GetLastestItems(string query,int startttindex)
        {
            if (!_firesGetLastestItems)
            {
                StartSid = startttindex;
                _firesGetLastestItems = true;
            }
            //StartSid = startttindex;
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
