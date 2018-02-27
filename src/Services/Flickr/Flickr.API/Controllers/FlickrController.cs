using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravoryContainers.Services.Flickr.API.Connector;
using TravoryContainers.Services.Flickr.API.Connector.FlickrResults;
using TravoryContainers.Services.Flickr.API.Helpers;
using TravoryContainers.Services.Flickr.API.Model;
using TravoryContainers.Services.Flickr.API.Services;

namespace TravoryContainers.Services.Flickr.API.Controllers
{
    [Route("api/v1/flickr")]
    public class FlickrController : Controller
    {
        private readonly IFlickrService _flickrService;        

        public FlickrController(IFlickrService flickrService)
        {
            _flickrService = flickrService;
        }
        
        [HttpGet]
        [Route("albums")]
        public async Task<IActionResult> GetAlbums()
        {
            var albums = await _flickrService.GetPhotoSets(GetUserData(HttpContext));

            return Ok(albums);
        }

        [HttpGet]
        [Route("photoset/{id}/photoids")]
        public async Task<IActionResult> GetPhotoSetPhotoIds(long id)
        {
            var photoIds = await _flickrService.GetPhotoSetPhotoIds(GetUserData(HttpContext), id);            

            return Ok(photoIds);
        }

        [HttpGet]
        [Route("photo/{id}")]
        public async Task<IActionResult> GetPhoto(long id)
        {
            var photo = await _flickrService.GetPhoto(GetUserData(HttpContext), id);
            return Ok(photo);
        }

        private UserData GetUserData(HttpContext context)
        {
            return new UserData(
                context.GetConsumer(),
                context.GetToken());
        }

         
    }
}
