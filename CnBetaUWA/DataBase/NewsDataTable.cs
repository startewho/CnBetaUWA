using SQLite.Net;
using SQLite.Net.Platform.WinRT;
using CnBetaUWA.Models;
namespace CnBetaUWA.DataBase
{
    public  class NewsDataTable
    {
        private static SQLiteConnection CreateTableConnection()
        {
            // 创建 News 模型对应的表,如果已存在,则忽略该操作。
            var connection = new SQLiteConnection(new SQLitePlatformWinRT(), DataBase.DbPath);
            connection.CreateTable<News>();
            return connection;
        }

        public static News Query(int sid)
        {
            return (from t in CreateTableConnection().Table<News>()
                where t.Sid == sid
                select t).FirstOrDefault();
        }

        public static void Delete(News news)
        {
            using ( var connect=CreateTableConnection())
            {
                connect.Delete(news);
            }
        }

        public static void Add(News news)
        {
            using (var connect = CreateTableConnection())
            {
                connect.InsertOrReplace(news);
            }
        }
    }
}
