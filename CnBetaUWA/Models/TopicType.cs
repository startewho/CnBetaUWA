using Newtonsoft.Json;
namespace CnBetaUWA.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public   class TopicType
    {
        [JsonProperty]
        private string NamePre { get; set; }

        [JsonProperty]
        public string Name { get; set; }

        [JsonProperty]
        public int Id { get; set; }

        [JsonProperty]
        public string LogoUrl { get; set; }
        [JsonProperty]
        public bool IsSelected { get; set; }
    }
}
