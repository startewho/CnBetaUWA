using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace CnBetaUWA.DataBase
{
    public   class DataBase
    {
        public static readonly string DbPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "CnbetaDataBase.db");
    }
}
