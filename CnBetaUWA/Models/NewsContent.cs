using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CnBetaUWA.Models
{
    public class NewsContent
    {
        public int Sid { get; set; }
        public string InputTime { get; set; }
        public Int32 CommentCount { get; set; }
        public string BodyText { get; set; }
        public string HomeText { get; set; }
        public string Source { get; set; }
    }

}
