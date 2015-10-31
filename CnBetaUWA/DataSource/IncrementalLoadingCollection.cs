using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Data;

namespace CnBetaUWA.DataSource
{

    public interface IIncrementalSource<T>
    {
        /// <summary>
        /// 数据源接口
        /// </summary>
        /// <param name="query">查询语句</param>
        /// <param name="pageIndex">页面索引</param>
        /// <param name="pageSize">分页大小</param>
        /// <returns></returns>
        Task<IEnumerable<T>> GetPagedItems(string query,string querytype);


        /// <summary>
        /// 获得最新Items
        /// </summary>
        /// <param name="query">查询语句</param>
        /// <returns></returns>
        Task<IEnumerable<T>> GetLastestItems(string query,string querytype);

        void InitSouce( IEnumerable<T> caches);

    }

    public class IncrementalLoadingCollection<T, I> : ObservableCollection<I>,
        ISupportIncrementalLoading
        where T : IIncrementalSource<I>, new()
    {
        private T _source;
       // private int _pageSize;
        private bool _hasMoreItems;
        private string _query;
        private string _querytype;
       // private IEnumerable<I> _caches;

        #region 注册通知机制
        public delegate void LoadMoreStarted(uint count);
        public delegate void LoadMoreCompleted(int count);

        public event LoadMoreStarted OnLoadMoreStarted;
        public event LoadMoreCompleted OnLoadMoreCompleted;
        #endregion



        public IncrementalLoadingCollection(string query, IEnumerable<I> caches)
        {
            _source = new T();
            if (caches!=null)
            {
                _source.InitSouce(caches);
            }
           
            _hasMoreItems = true;
            this._query = query;
          
        }


        public IncrementalLoadingCollection(string query,string querytype , IEnumerable<I> caches)
        {
            _source = new T();
            if (caches != null&&caches.Any())
            {
                _source.InitSouce(caches);
            }

            _hasMoreItems = true;
            this._query = query;
            this._querytype = querytype;

        }


        public bool HasMoreItems
        {
            get { return _hasMoreItems; }
        }

      
        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
          return AsyncInfo.Run((c) => LoadMoreItemsAsync(c, count));
        }

        async Task<LoadMoreItemsResult> LoadMoreItemsAsync(CancellationToken c, uint count)
        {
            try
            {
                OnLoadMoreStarted?.Invoke(count);

                uint resultCount = 0;

                var result = await _source.GetPagedItems(_query,_querytype);

                if (result != null &&  result.Any())
                {
                    resultCount = (uint)result.Count();
                
                        foreach (I item in result.ToList())
                            Add(item);

                    //if (resultCount < _pageSize)
                    //{
                    //    _hasMoreItems = false;
                    //}
                }
                else
                {
                    _hasMoreItems = false;
                }

                // 加载完成事件
                OnLoadMoreStarted?.Invoke(resultCount);
                Debug.WriteLine("Already Loading count{0},Everytime loading count:{1}", Items.Count, count);

                return new LoadMoreItemsResult { Count = resultCount };

            }
            finally
            {
              
            }
        }



        public async Task<int> AttachToEnd()
        {
            var newItems = await _source.GetLastestItems(_query,_querytype);
          
            if (newItems == null) return 0;

            var items = newItems as IList<I> ?? newItems.ToList();
            _source.InitSouce(this);
           foreach (I item in items.Reverse())
                Insert(0, item);
            return items.Count;
        }

    }
}