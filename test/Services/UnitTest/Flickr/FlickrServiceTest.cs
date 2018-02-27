using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TravoryContainers.Services.Flickr.API.Connector;
using TravoryContainers.Services.Flickr.API.Connector.FlickrResults;
using TravoryContainers.Services.Flickr.API.Controllers;
using TravoryContainers.Services.Flickr.API.Helpers;
using TravoryContainers.Services.Flickr.API.Model;
using TravoryContainers.Services.Flickr.API.Services;
using Xunit;

namespace UnitTest.Flickr
{
    public class FlickrServiceTest
    {
        private (long id, long primary, string title, string description) _photoSetDataValue;
        private readonly UserData _userData;
        private readonly Mock<IFlickrConnector> _flickrConnectorMock;
        private readonly FlickrService _flickrService;
        private readonly Mock<IDateCalculator> _dateCalculatorMock;

        public FlickrServiceTest()
        {
            _userData = new UserData();
            _photoSetDataValue = (id: 27157680817475516, primary: 32182272603, title: "Title", description: "");
            _flickrConnectorMock = new Mock<IFlickrConnector>();
            _dateCalculatorMock = new Mock<IDateCalculator>();
            _flickrService = new FlickrService(_flickrConnectorMock.Object, _dateCalculatorMock.Object);
        }

        [Fact]
        public async Task GetAlbums_ReturnsListOfAlbums()
        {
            _flickrConnectorMock.Setup(f => f.GetPhotoSets(It.Is<UserData>(u => u == _userData))).ReturnsAsync(GetPhotoSetsResult(_photoSetDataValue)).Verifiable();

            var photoSets = await _flickrService.GetPhotoSets(_userData);

            _flickrConnectorMock.Verify();
            Assert.NotNull(photoSets);
            Assert.Single(photoSets);
            Assert.Equal(_photoSetDataValue.id, photoSets[0].Id);
            Assert.Equal(_photoSetDataValue.primary, photoSets[0].Primary);
            Assert.Equal(_photoSetDataValue.title, photoSets[0].Title);
        }

        [Fact]
        public async Task GetAlbums_CalculatedFromAndToDates()
        {
            _photoSetDataValue.description = "2017. 05. 30. - 2017. 05. 31.";
            _flickrConnectorMock.Setup(f => f.GetPhotoSets(It.Is<UserData>(u => u == _userData))).ReturnsAsync(GetPhotoSetsResult(_photoSetDataValue)).Verifiable();
            _dateCalculatorMock
                .Setup(d => d.GetDate(It.Is<string>(s => s == "2017. 05. 30."))).Returns(new DateTime(2017, 5, 30)).Verifiable();
            _dateCalculatorMock
                .Setup(d => d.GetDate(It.Is<string>(s => s == "2017. 05. 31."))).Returns(new DateTime(2017, 5, 31)).Verifiable();

            var photoSets = await _flickrService.GetPhotoSets(_userData);

            _dateCalculatorMock.Verify();
            Assert.NotNull(photoSets);
            Assert.Single(photoSets);
            Assert.Equal(new DateTime(2017, 5, 30), photoSets[0].FromDate);
            Assert.Equal(new DateTime(2017, 5, 31), photoSets[0].ToDate);
        }

        [Fact]
        public async Task GetPhotoSetPhotos_ReturnsPhotoIds()
        {
            long photoSetId = 27157680817475516;
            long photoId = 32182272603;
            _flickrConnectorMock.Setup(f => f.GetPhotosSetPhotos(It.Is<UserData>(u => u == _userData), It.Is<long>(i => i == photoSetId))).ReturnsAsync(GetPhotosResult(photoId)).Verifiable();

            var photoIds = await _flickrService.GetPhotoSetPhotoIds(_userData, photoSetId);

            _flickrConnectorMock.Verify();
            Assert.NotNull(photoIds);
            Assert.Single(photoIds);
            Assert.Equal(photoId, photoIds[0].Id);
        }

        [Fact]
        public async Task GetPhoto_ReturnsPhotoSources()
        {
            var square = "https://farm5.staticflickr.com/7654/5435435463_square.jpg";
            var large = "https://farm5.staticflickr.com/7654/5435435463_large.jpg";
            var original = "https://farm5.staticflickr.com/7654/5435435463_orig.jpg";
            long photoId = 32182272603;
            string dateTaken = "2017-07-16 08:11:15";
            var date = DateTime.Parse("2017-07-16 08:11:15");
            _flickrConnectorMock
                .Setup(f => f.GetPhotoSizes(It.Is<UserData>(u => u == _userData), It.Is<long>(l => l == photoId)))
                .ReturnsAsync(GetPhotoSizesResult((label: "Square", square), ("Large", large), ("Original", original)));

            _flickrConnectorMock.Setup(f => f.GetPhotoInfo(It.Is<UserData>(u => u == _userData), It.Is<long>(i => i == photoId))).ReturnsAsync(GetPhotoInfoResult(photoId, dateTaken)).Verifiable();
            _dateCalculatorMock
                .Setup(d => d.GetDateAndTime(It.Is<string>(s => s == dateTaken))).Returns(date).Verifiable();

            var photo = await _flickrService.GetPhoto(_userData, photoId);

            _dateCalculatorMock.Verify();
            Assert.NotNull(photo);
            Assert.Equal(photoId, photo.Id);
            Assert.Equal(square, photo.Square);
            Assert.Equal(large, photo.Large);
            Assert.Equal(original, photo.Original);
            Assert.Equal(date, photo  .Taken);
        }

        private FlickrPhotoSetsResult GetPhotoSetsResult(params (long id, long primary, string title, string description)[] photoSetDataValues)
        {
            return new FlickrPhotoSetsResult()
            {
                PhotoSets = new FlickrPhotoSetsData
                {
                    PhotoSet = photoSetDataValues.Select(p => new FlickrPhotoSetData
                    {
                        Id = p.id.ToString(),
                        Primary = p.primary.ToString(),
                        Title = new FlickrContentData
                        {
                            _Content = p.title
                        },
                        Description = new FlickrContentData
                        {
                            _Content = p.description
                        }
                    }).ToList()
                }
            };
        }

        private FlickrPhotoSizesResult GetPhotoSizesResult(params (string label, string source)[] sizes)
        {
            return new FlickrPhotoSizesResult
            {
                Sizes = new FlickrPhotoSizesData
                {
                    Size = sizes.Select(s => new FlickrPhotoSizeData
                    {
                        Label = s.label,
                        Source = s.source
                    }).ToList()
                }
            };
        }

        private static FlickrPhotosResult GetPhotosResult(params long[] photoIds)
        {
            return new FlickrPhotosResult()
            {
                PhotoSet = new FlickrPhotoSetPhotosData
                {
                    Photo = photoIds.Select(p => new FlickrPhotoInfoData
                    {
                        Id = p.ToString()
                    }).ToList()
                }
            };
        }
        private static FlickrPhotoInfoResult GetPhotoInfoResult(long photoId, string taken)
        {
            return new FlickrPhotoInfoResult
            {
                Photo = new FlickrPhotoData
                {
                    Id = photoId.ToString(),
                    Dates = new FlickrDates()
                    {
                        Taken = taken
                    }
                }
            };
        }
    }
}