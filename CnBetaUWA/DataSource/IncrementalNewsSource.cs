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

        private int _startSid;
        private int _endSid;
        private IEnumerable<News> _cacheNewes;
       private IEnumerable<News> _latestNewses;
        private const int pageSids = 40;
        private const int sidSpace = 2;
        private bool _firstLoad;
        public void InitSouce(IEnumerable<News> caches)

        {
            _startSid = caches.First().Sid;
            _endSid = caches.Last().Sid;
            _cacheNewes = caches;
        }

        public async Task<IEnumerable<News>> GetPagedItems(string query)
        {
           
            //没有缓存时,直接加载
            if (_cacheNewes==null)
            {
                var reuslt = await GetDownNewsFromNet(query, null, _endSid);
                _endSid = reuslt.Last().Sid;
                return reuslt;
            }

            //加载刷新+缓存数据
            else
            {
             
                _latestNewses = await GetUpItems(query);//更改_endSid

                var distance = _endSid - _cacheNewes.First().Sid;
                //有新数据
                if (distance>0)
                {
                    var result = await GetCaches(query, distance);
                    return result;
                }

                //没有得到新数据,
                else
                {
                   // _endSid = _cacheNewes.Last().Sid;
                    var newcache = _cacheNewes;
                    _cacheNewes = null;
                    return newcache;
                }

            }
        }


        private async Task<IEnumerable<News>> GetCaches(string query, int distance )
        {
           var list = new List<News>();
           var latestpage = distance / pageSids;
            
            //不多于一页
            if (latestpage==0)
            {
                var reuslt = await GetDownNewsFromNet(query, null, _endSid);
                if (_latestNewses!=null)
                {
                    list.AddRange(_latestNewses);
                    _latestNewses = null;
                   
                }
               
                list.AddRange(reuslt.Take(distance/ sidSpace-1));
                list.AddRange(_cacheNewes);
                _endSid = list.Last().Sid;
                _cacheNewes = null;
                return list;
            }

            //不小于1页
            if (latestpage >= 1)
            {
                if (_latestNewses != null)
                {
                    list.AddRange(_latestNewses);
                    _latestNewses = null;
                  
                }

                var downnewses = await GetDownNewsFromNet(query, null, _endSid);
                list.AddRange(downnewses);
                return list;
            }

            return null;
        }


        private async Task<IEnumerable<News>> GetDownNewsFromNet(string query,Type type, int endsid)
        {
            var jsontext = await CnBetaHelper.GetNews(query, null, endsid);

            if (jsontext == null) return null;

            JObject postlist = JObject.Parse(jsontext);
            var jsonList = postlist.SelectToken("result");

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
            
            _endSid = list.Last().Sid;
            return list;
        }


    /// <summary>
        /// 加载刷新数据,如果有新数据
        /// </summary>
        /// <param name="query"></param>
        /// <param name="startttindex"></param>
        /// <returns></returns>
    public async Task<IEnumerable<News>> GetUpItems(string query)
    {
        if (_firstLoad == false)
        {
            var jsontext = await CnBetaHelper.GetLastestNews(query, null, _startSid, 0);

            JObject postlist = JObject.Parse(jsontext);
            var jsonList = postlist.SelectToken("result");

            if (jsonList.Any())
            {
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

                _startSid = list.First().Sid;
                _endSid = list.Last().Sid;
                return list;
            }

            _firstLoad = true;
            return null;
        }

        return null;
    }

     
    }
    
}
