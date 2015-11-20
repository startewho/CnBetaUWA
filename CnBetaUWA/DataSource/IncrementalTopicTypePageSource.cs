using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CnBetaUWA.Helper;
using CnBetaUWA.Models;
using Q42.WinRT.Storage;

namespace CnBetaUWA.DataSource
{
    public  class IncrementalTopicTypePageSource : IIncrementalPageSource<TopicType>
    {
      

        private async Task<IEnumerable<TopicType>> GetData(string query)
        {
            var jsontext = await IOHelper.GetTextFromStorage(new Uri("ms-appx:///AppData/CnbetaAllTopics.json"));
            var jsontopics = SettingsHelper.Get<string>(CnBetaHelper.SettingSelectedTotics);
            var settingtopics = SerializerHelper.JsonDeserialize<List<TopicType>>(jsontopics);
            var alltopics= ModelHelper.JsonToTopicTypes(jsontext).ToList();
            foreach (var settingtopicType in settingtopics)
            {
                foreach (var topictype in alltopics)
                {
                    if (settingtopicType.Id==topictype.Id)
                    {
                        topictype.IsSelected = settingtopicType.IsSelected;
                    }
                }
                
            }
            var querytopics = alltopics.Where(item => item.Name.Contains(query)).ToList();
            return querytopics;
        }

        private static List<TopicType> _sourceCollection; 

        public Task<IEnumerable<TopicType>> GetLastestItems(string query)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<TopicType>> GetPagedItems(string query, int pageIndex, int pageSize)
        {
            
                _sourceCollection = (List<TopicType>) await GetData(query);
            
            var colltions = _sourceCollection.Skip(pageIndex*pageSize);

            return colltions.Take(pageSize);

        }

       
    }
}
