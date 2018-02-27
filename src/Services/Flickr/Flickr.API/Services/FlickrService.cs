using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravoryContainers.Services.Flickr.API.Connector;
using TravoryContainers.Services.Flickr.API.Connector.FlickrResults;
using TravoryContainers.Services.Flickr.API.Helpers;
using TravoryContainers.Services.Flickr.API.Model;

namespace TravoryContainers.Services.Flickr.API.Services
{
    public class FlickrService : IFlickrService
    {
        private readonly IFlickrConnector _flickrConnector;
        private readonly IDateCalculator _dateCalculator;

        public FlickrService(IFlickrConnector flickrConnector, IDateCalculator dateCalculator)
        {
            _flickrConnector = flickrConnector;
            _dateCalculator = dateCalculator;
        }

        public async Task<List<Album>> GetPhotoSets(UserData userData)
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

            return albums;
        }

        public async Task<List<PhotoReference>> GetPhotoSetPhotoIds(UserData userData, long id)
        {
            var flickrResult = await _flickrConnector.GetPhotosSetPhotos(userData, id);
            var photoIds = new List<PhotoReference>();

            foreach (var photoReference in flickrResult.PhotoSet.Photo)
            {
                photoIds.Add(new PhotoReference
                {
                    Id = long.Parse(photoReference.Id)
                });
            }

            return photoIds;
        }

        public async Task<Photo> GetPhoto(UserData userData, long id)
        {
            var photoSizes = await _flickrConnector.GetPhotoSizes(userData, id);
            var photoInfo = await _flickrConnector.GetPhotoInfo(userData, id);

            var flickrPhotoSizeDataList = photoSizes?.Sizes?.Size;
            return new Photo
            {
                Id = id,
                Square = flickrPhotoSizeDataList?.FirstOrDefault(s => s.Label == "Square")?.Source,
                Large = flickrPhotoSizeDataList?.FirstOrDefault(s => s.Label == "Large")?.Source,
                Original = flickrPhotoSizeDataList?.FirstOrDefault(s => s.Label == "Original")?.Source,
                Taken = _dateCalculator.GetDateAndTime(photoInfo?.Photo?.Dates?.Taken)
            };
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

    public interface IFlickrService
    {
        Task<List<Album>> GetPhotoSets(UserData userData);
        Task<List<PhotoReference>> GetPhotoSetPhotoIds(UserData userData, long id);
        Task<Photo> GetPhoto(UserData userData, long id);
    }
}
