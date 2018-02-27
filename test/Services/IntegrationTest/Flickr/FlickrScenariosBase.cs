using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using TravoryContainers.Services.Flickr.API;

namespace IntegrationTest.Flickr
{
    public class FlickrScenariosBase
    {
        private const string ApiUrlBase = "api/v1/flickr";

        public TestServer CreateServer()
        {
            var webHostBuilder = WebHost.CreateDefaultBuilder().UseStartup<Startup>();

            return new TestServer(webHostBuilder);
        }

        public static class Post
        {
            public static string PhotoSetPhotos(long id) => $"{ApiUrlBase}/photoset/{id}/photos";
            public static string Photo(long id) => $"{ApiUrlBase}/photo/{id}";
        }

        public static class Get
        {
            public static string Albums = $"{ApiUrlBase}/albums";
        }
    }
}