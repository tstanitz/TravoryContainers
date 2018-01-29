using System.Net.Http;

namespace TravoryContainers.Services.Flickr.API.Helpers
{
    public interface IHttpClientFactory
    {
        HttpClient Client { get; }
    }
}