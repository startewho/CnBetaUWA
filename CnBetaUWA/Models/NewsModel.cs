using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVVMSidekick.ViewModels;
namespace CnBetaUWA
{
    public class NewsModel
    {
        public string Tilte { get; set; }

        public string Summary { get; set; }
        public string CreatTime { get; set; }

        public string ThumbPicture { get; set; }

        public int Sid { get; set; }
        public int ViewCount { get; set; }

        public int CommentsCount { get; set; }

        public int TopicId { get; set; }
        public string TopictLogoPicture { get; set; }
    }
}
