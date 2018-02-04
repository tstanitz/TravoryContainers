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
            public static string Albums = $"{ApiUrlBase}/albums";
            public static string PhotoSources(long id) => $"{ApiUrlBase}/photosources/{id}";
            public static string PhotoSetPhotos(long id) => $"{ApiUrlBase}/photoset/{id}/photos";
            public static string PhotoInfo(long id) => $"{ApiUrlBase}/photo/{id}/info";
        }
    }
}