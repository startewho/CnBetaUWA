using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CnBetaUWA.Models
{
   public class NewsComment
    {
       public int Against { get; set; }

       public int Support { get; set; }

       public int Tid { get; set; }

       public string UserName { get; set; }

       public string Content { get; set; }
       public string CreatTime { get; set; }
    }
}
