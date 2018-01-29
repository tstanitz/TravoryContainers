using System.Net.Http;
using TravoryContainers.Services.Flickr.API.Connector;

namespace TravoryContainers.Services.Flickr.API.Helpers
{
    public class HttpClientFactory : IHttpClientFactory
    {
        public HttpClient Client => new HttpClient();
    }
}