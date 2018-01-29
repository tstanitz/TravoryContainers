namespace TravoryContainers.Services.Flickr.API.Connector.FlickrResults
{
    public class FlickrPhotoSetData
    {
        public string Id { get; set; }
        public string Primary { get; set; }
        public FlickrContentData Title { get; set; }
        public FlickrContentData Description { get; set; }
    }
}