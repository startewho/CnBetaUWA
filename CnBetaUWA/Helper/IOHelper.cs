using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Search;
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

        public static async Task<double> GetFolderSize(StorageFolder folder)
        {
            
            // Query all files in the folder. Make sure to add the CommonFileQuery
            // So that it goes through all sub-folders as well
            var folders = folder.CreateFileQuery(CommonFileQuery.OrderByName);

            // Await the query, then for each file create a new Task which gets the size
            var fileSizeTasks = (await folders.GetFilesAsync()).Select(async file => (await file.GetBasicPropertiesAsync()).Size);

            // Wait for all of these tasks to complete. WhenAll thankfully returns each result
            // as a whole list
            var sizes = await Task.WhenAll(fileSizeTasks);

            // Sum all of them up. You have to convert it to a long because Sum does not accept ulong.
            var folderSize = sizes.Sum(l => (long)l);

            return folderSize;
        }

        public static async Task<bool> DeleteFolder(StorageFolder folder)
        {
            var folders = await folder.GetItemsAsync();
            for (int i = 0; i < folders.Count; i++)
            {
                 DeletItem(folders[i]);
            }

        
            
            return true;
        }

        public static async void DeletItem(IStorageItem item)
        {
            await item.DeleteAsync();
        }

    }
}
