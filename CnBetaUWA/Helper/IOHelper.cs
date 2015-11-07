using System;
using System.Threading.Tasks;
using Windows.Storage;
using Q42.WinRT;
using UnicodeEncoding = Windows.Storage.Streams.UnicodeEncoding;

namespace CnBetaUWA.Helper
{
    public static class IOHelper
    {
        public static async Task<string> GetTextFromStorage(Uri uri)
        {
            var file = await StorageFile.GetFileFromApplicationUriAsync(uri);
            var text = await FileIO.ReadTextAsync(file);
            return text;

        }

        public static async Task<bool> WriteTextToLocalCacheStorageFile(string folder,string filename, string text)
        {
            
            var fold = await ApplicationData.Current.LocalCacheFolder.CreateFolderAsync(folder, CreationCollisionOption.OpenIfExists);//打开文件夹
            var exist = await fold.ContainsFileAsync(filename);
            if (exist)
            {
                return true;
            }
             var file = await fold.CreateFileAsync(filename, CreationCollisionOption.OpenIfExists);//创建个文件
            await FileIO.WriteTextAsync(file, text,UnicodeEncoding.Utf8);//在文件里面写内容
            return true;
        }
        public static async Task<string> ReadTextFromLocalCacheStorage(string folder,string filename)
        {
            var fold = await ApplicationData.Current.LocalCacheFolder.CreateFolderAsync(folder, CreationCollisionOption.OpenIfExists);//打开文件夹
            StorageFile file = await fold.GetFileAsync(filename);//打开对应的文件
            
            string result = await FileIO.ReadTextAsync(file);//读取文件里面的内容
            return result;
        }


    }
}
