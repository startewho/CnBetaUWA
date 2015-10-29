using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace CnBetaUWA.DataSource
{

    public interface IIncrementalPageSource<T>
    {
        /// <summary>
        /// 数据源接口
        /// </summary>
        /// <param name="query">查询语句</param>
        /// <param name="pageIndex">页面索引</param>
        /// <param name="pageSize">分页大小</param>
        /// <returns></returns>
        Task<IEnumerable<T>> GetPagedItems(string query, int pageIndex, int pageSize);

       

        /// <summary>
        /// 获得最新Items
        /// </summary>
        /// <param name="query">查询语句</param>
        /// <returns></returns>
        Task<IEnumerable<T>> GetLastestItems(string query);

    }

    public class IncrementalPageLoadingCollection<T, I> : ObservableCollection<I>,
        ISupportIncrementalLoading
        where T : IIncrementalPageSource<I>, new()
    {
        private T _source;
        private int _pageSize;
        private bool _hasMoreItems;
        private int _currentPage;
        private string _query;



        #region 注册通知机制
        public delegate void LoadMoreStarted(uint count);
        public delegate void LoadMoreCompleted(int count);

        public event LoadMoreStarted OnLoadMoreStarted;
        public event LoadMoreCompleted OnLoadMoreCompleted;
        #endregion



        public IncrementalPageLoadingCollection(string query,int pageindex,int pageSize )
        {
            _source = new T();
            this._pageSize = pageSize;
            _currentPage = pageindex;
            _hasMoreItems = true;
            this._query = query;
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

                var result = await _source.GetPagedItems(_query, _currentPage++, _pageSize);

                if (result != null &&  result.Any())
                {
                    resultCount = (uint)result.Count();
                
                        foreach (I item in result)
                            Add(item);

                    if (resultCount < _pageSize)
                    {
                        _hasMoreItems = false;
                    }
                }
                else
                {
                    _hasMoreItems = false;
                }

                // 加载完成事件
                OnLoadMoreCompleted?.Invoke((int)resultCount);

                Debug.WriteLine("Already Loading count{0},Everytime loading count:{1}", Items.Count, count);
                return new LoadMoreItemsResult { Count = resultCount };

            }
            finally
            {
              
            }
        }



        public async Task<int> AttachToEnd()
        {
            var newItems = await _source.GetLastestItems(_query);

            if (newItems == null) return 0;

            var items = newItems as IList<I> ?? newItems.ToList();
            foreach (I item in items.Reverse())
                Insert(0, item);
            return items.Count();
        }

    }
}