using System.Threading.Tasks;
using TravoryContainers.Services.Flickr.API.Connector.FlickrResults;
using TravoryContainers.Services.Flickr.API.Model;

namespace TravoryContainers.Services.Flickr.API.Connector
{
    public interface IFlickrConnector
    {
        Task<FlickrPhotoSetsResult> GetPhotoSets(UserData userData);
        Task<FlickrPhotoSizesResult> GetPhotoSizes(UserData userData, long photoId);
        Task<FlickrPhotosResult> GetPhotosSetPhotos(UserData userData, long photoSetId);
    }
}