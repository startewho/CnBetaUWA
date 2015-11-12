using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
           
            if (caches.Any())
            {
                _startSid = caches.First().Sid;
                _endSid = caches.Last().Sid;
                _cacheNewes = caches.ToList();
                _firstLoad = true;
            }
        }

        public async Task<IEnumerable<News>> GetPagedItems(string query,string querytype)
        {
            //没有缓存时,直接加载
            if (_cacheNewes == null)
            {
                var reuslt = await GetDownNewsFromNet(query, querytype, _endSid);
                _endSid = reuslt.Last().Sid;
                return reuslt;
            }

            //加载刷新+缓存数据
            _latestNewses = await GetLastestItems(query, querytype); //更改_endSid

            var distance = _endSid - _cacheNewes.First().Sid;

            //有新数据
            if (distance > 0)
            {
                var result = await GetCaches(query, querytype, distance);
                return result;
            }

            //没有得到新数据,
            // _endSid = _cacheNewes.Last().Sid;
            var newcache = _cacheNewes;
            _cacheNewes = null;
            return newcache;
        }


        private async Task<IEnumerable<News>> GetCaches(string query,string querytype, int distance)
        {
            var list = new List<News>();
            var latestpage = distance/pageSids;

            if (_latestNewses != null)
            {
                list.AddRange(_latestNewses);
                _latestNewses = null;
            }

            //不多于一页
            if (latestpage == 0)
            {
                var diffcount = distance/sidSpace - 1;
                if (diffcount>0)
                {
                    var reuslt = await GetDownNewsFromNet(query, querytype, _endSid);
                    list.AddRange(reuslt.Take(diffcount));
                }
                list.AddRange(_cacheNewes);
                _endSid = list.Last().Sid;
                _cacheNewes = null;
               return list;
            }

            //不小于1页
            var downnewses = await GetDownNewsFromNet(query, querytype, _endSid);
            list.AddRange(downnewses);
            return list;
        }


        private async Task<IEnumerable<News>> GetDownNewsFromNet(string query ,string querytype, int endsid)
        {
            var jsontext = await CnBetaHelper.GetNews(query, querytype, endsid);

            if (jsontext == null) return null;

            var list = ModelHelper.JsonToNewses(jsontext);
            if (list != null)
            {
                _endSid = list.Last().Sid;
            }

            return list;
        }


        /// <summary>
        /// 加载刷新数据,如果有新数据
        /// </summary>
        /// <param name="query"></param>
        /// <param name="startttindex"></param>
        /// <returns></returns>
        public async Task<IEnumerable<News>> GetLastestItems(string query,string querytype)
        {
            if (_firstLoad)
            {
                var jsontext = await CnBetaHelper.GetLastestNews(query,querytype, _startSid, 0);

                var list = ModelHelper.JsonToNewses(jsontext);
                if (list != null)
                {
                    var enumerable = list as IList<News> ?? list.ToList();
                    _startSid = enumerable.First().Sid;
                    _endSid = enumerable.Last().Sid;
                }

                _firstLoad = false;
                
                return list;
            }

            return null;
        }
    }
    
}
