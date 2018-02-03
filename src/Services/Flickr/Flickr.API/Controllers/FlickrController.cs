using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TravoryContainers.Services.Flickr.API.Connector;
using TravoryContainers.Services.Flickr.API.Connector.FlickrResults;
using TravoryContainers.Services.Flickr.API.Helpers;
using TravoryContainers.Services.Flickr.API.Model;

namespace TravoryContainers.Services.Flickr.API.Controllers
{
    [Route("api/v1/flickr")]
    public class FlickrController : Controller
    {
        private readonly IFlickrConnector _flickrConnector;
        private readonly IDateCalculator _dateCalculator;

        public FlickrController(IFlickrConnector flickrConnector, IDateCalculator dateCalculator)
        {
            _flickrConnector = flickrConnector;
            _dateCalculator = dateCalculator;
        }

        [HttpPost]
        [Route("albums")]
        public async Task<IActionResult> GetAlbums([FromBody]UserData userData)
        {
            var flickrResult = await _flickrConnector.GetPhotoSets(userData);

            var albums = new List<Album>();
            if (flickrResult?.PhotoSets?.PhotoSet != null)
            {
                foreach (var photoSet in flickrResult.PhotoSets.PhotoSet)
                {
                    var dates = GetFromAndToDates(photoSet);
                    albums.Add(new Album
                    {
                        Id = long.Parse(photoSet.Id),
                        Primary = long.Parse(photoSet.Primary),
                        Title = photoSet.Title._Content,
                        FromDate = dates.from,
                        ToDate = dates.to
                    });
                }
            }
            return Ok(albums);
        }

        [HttpPost]
        [Route("photo/{id}")]
        public async Task<IActionResult> GetPhoto([FromBody]UserData userData, long id)
        {
            var flickrResult = await _flickrConnector.GetPhotoSizes(userData, id);
            var flickrPhotoSizeDataList = flickrResult?.Sizes?.Size;
            return Ok(new Photo
            {
                Id = id,
                Square = flickrPhotoSizeDataList?.FirstOrDefault(s => s.Label == "Square")?.Source,
                Large = flickrPhotoSizeDataList?.FirstOrDefault(s => s.Label == "Large")?.Source,
                Original = flickrPhotoSizeDataList?.FirstOrDefault(s => s.Label == "Original")?.Source
            });
        }

        private (DateTime? from, DateTime? to) GetFromAndToDates(FlickrPhotoSetData photoSet)
        {
            var intervalComment = photoSet?.Description?._Content;
            if (!string.IsNullOrEmpty(intervalComment))
            {
                var dates = intervalComment.Split('-').Select(p => p.Trim()).ToArray();

                var fromString = dates.Length > 0 ? dates[0] : null;
                var toString = dates.Length > 1 ? dates[1] : null;

                return (from: _dateCalculator.GetDate(fromString), to: _dateCalculator.GetDate(toString));
            }
            return (from: null, to: null);
        }


    }
}
