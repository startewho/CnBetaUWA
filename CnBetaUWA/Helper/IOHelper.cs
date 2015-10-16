using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

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
    }
}
