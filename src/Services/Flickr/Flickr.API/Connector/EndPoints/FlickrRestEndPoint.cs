namespace TravoryContainers.Services.Flickr.API.Connector.EndPoints
{
    public class FlickrRestEndPoint : FlickrEndPointBase
    {
        public FlickrRestEndPoint()
        {
            Url = "https://api.flickr.com/services/rest/";
            MethodType = "GET";
        }
    }
}