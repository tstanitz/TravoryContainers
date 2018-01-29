using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TravoryContainers.Services.Flickr.API.Connector;
using TravoryContainers.Services.Flickr.API.Model;

namespace TravoryContainers.Services.Flickr.API.Controllers
{
    [Route("api/v1/[controller]")]
    public class FlickrController : Controller
    {
        private readonly IFlickrConnector _flickrConnector;

        public FlickrController(IFlickrConnector flickrConnector)
        {
            _flickrConnector = flickrConnector;
        }

        [HttpPost]
        public async Task<IActionResult> GetAlbums(UserData userData)
        {
            var flickrResult = await _flickrConnector.GetPhotoSets(userData);
            var albums = new List<Album>();
            if (flickrResult?.PhotoSets?.PhotoSet != null)
            {
                foreach (var photoSet in flickrResult.PhotoSets.PhotoSet)
                {
                    albums.Add(new Album
                    {
                        Id = long.Parse(photoSet.Id),
                        Title = photoSet.Title._Content
                    });
                }
            }
            return Ok(albums);
        }
    }
}
