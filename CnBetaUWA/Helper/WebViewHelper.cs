using System;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage.Streams;
using Windows.Web;
using Windows.Web.Http;
namespace CnBetaUWA.Helper
{
    public sealed class StreamUriWinRTResolver : IUriToStreamResolver
        {
            readonly HttpClient _httpClient = new  HttpClient();
            private readonly string _baseUrl;

            public StreamUriWinRTResolver(string baseUrl)
            {
                _baseUrl = baseUrl;
            }

            public IAsyncOperation<IInputStream> UriToStreamAsync(Uri uri)
            {
                try
                {
                    if (uri == null)
                    {
                        throw new Exception();
                    }
                    string path = uri.AbsolutePath;
                    return GetContent(path).AsAsyncOperation();

                }
                catch (Exception)
                {
                    return null;
                }
            }

            private async Task<IInputStream> GetContent(string uriPath)
            {
                try
                {
                    var url2 = _baseUrl + uriPath;
                    var bytes = await _httpClient.GetStringAsync(new Uri(url2));
                    IInputStream inputStream = await GetStreamForByteArray(bytes);
                    return inputStream;

                }
                catch (Exception)
                {
                    //catch not working.. 
                    return null;
                }
            }

            private static async Task<IInputStream> GetStreamForByteArray(string content2)
            {
                var ras = new InMemoryRandomAccessStream();
                var datawriter = new DataWriter(ras);
                datawriter.WriteString(content2);
                //datawriter.WriteBytes(content2);
                await datawriter.StoreAsync();
                await datawriter.FlushAsync();
                return ras.GetInputStreamAt(0);
            }
        }
    }
