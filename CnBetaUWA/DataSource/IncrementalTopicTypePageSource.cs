using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CnBetaUWA.Helper;
using CnBetaUWA.Models;
using ImageLib.Helpers;

namespace CnBetaUWA.DataSource
{
    public  class IncrementalTopicTypePageSource : IIncrementalPageSource<TopicType>
    {
      

        private async Task<IEnumerable<TopicType>> GetData()
        {
            var jsontext = await IOHelper.GetTextFromStorage(new Uri("ms-appx:///AppData/CnbetaAllTopics.json"));

           return ModelHelper.JsonToTopicTypes(jsontext);
        }

        private List<TopicType> _sourceCollection; 

        public Task<IEnumerable<TopicType>> GetLastestItems(string query)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<TopicType>> GetPagedItems(string query, int pageIndex, int pageSize)
        {
            if (_sourceCollection == null)
            {
                _sourceCollection = (List<TopicType>) await GetData();
            }

            var colltions = _sourceCollection.Skip(pageIndex*pageSize);

            return colltions.Take(pageSize);

        }

       
    }
}
