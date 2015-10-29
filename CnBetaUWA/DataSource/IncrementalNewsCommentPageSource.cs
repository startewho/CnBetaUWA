using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CnBetaUWA.Helper;
using CnBetaUWA.Models;

namespace CnBetaUWA.DataSource
{
    public   class IncrementalNewsCommentPageSource:IIncrementalPageSource<NewsComment>
    {
       

        public  async Task<IEnumerable<NewsComment>> GetPagedItems(string query, int pageIndex, int pageSize)
        {
            var result = await CnBetaHelper.GetNewsComment(Convert.ToInt32(query),pageIndex, 2*pageSize);
            var list = ModelHelper.JsonToNewsComments(result);
            return list;
        }

        Task<IEnumerable<NewsComment>> IIncrementalPageSource<NewsComment>.GetLastestItems(string query)
        {
            throw new NotImplementedException();
        }
    }
}
