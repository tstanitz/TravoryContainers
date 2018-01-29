namespace TravoryContainers.Services.Flickr.API.Connector.FlickrResults
{
    public abstract class FlickrResult
    {
        public string Stat { get; set; }
        public string Message { get; set; }
        public string Code { get; set; }
    }
}