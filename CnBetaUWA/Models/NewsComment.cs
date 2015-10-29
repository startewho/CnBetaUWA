using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CnBetaUWA.Models
{
   public class NewsComment
    {
       private string _userName;
       private bool _isShow;
       private NewsComment _pidComment;

       public int Against { get; set; }

       public int Support { get; set; }

       public int Tid { get; set; }

       public int Pid { get; set; }

     

       public string Content { get; set; }
       public string CreatTime { get; set; }

       public NewsComment PidComment
       {
           get { return _pidComment; }
           set { _pidComment = value; }
       }

       public string UserName
        {
            get
            {
                return _userName==string.Empty ? "匿名用户" : _userName;
            }

            set
            {
                _userName = value;
            }
        }

       public bool IsShow
       {
           get { return Pid>0; }
           set { _isShow = value; }
       }
    }
}
