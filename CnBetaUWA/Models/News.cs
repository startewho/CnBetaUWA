using MVVMSidekick.ViewModels;
using Newtonsoft.Json;

namespace CnBetaUWA.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class News:ViewModelBase<News>
    {
        [JsonProperty]

        public string Title { get; set; }
        [JsonProperty]

        public string Summary { get; set; }

        [JsonProperty]
        public string CreatTime { get; set; }

        [JsonProperty]
        public string ThumbPicture { get; set; }

        [JsonProperty]
        public int Sid { get; set; }
        [JsonProperty]
        public int ViewCount { get; set; }

        [JsonProperty]
        public int CommentsCount { get; set; }

        [JsonProperty]
        public int TopicId { get; set; }
        [JsonProperty]
        public string TopictLogoPicture { get; set; }


        [JsonProperty]
        public int Index { get; set; }


     

     
       
    }
}
