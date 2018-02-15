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
using Xunit;

namespace UnitTest.Flickr
{
    public class FlickrControllerTest
    {
        private (string id, long primary, string title, string description) _photoSetDataValue;
        private readonly UserData _userData;
        private readonly Mock<IFlickrConnector> _flickrConnectorMock;
        private readonly FlickrController _flickrController;
        private readonly Mock<IDateCalculator> _dateCalculatorMock;

        public FlickrControllerTest()
        {
            _userData = new UserData();
            _photoSetDataValue = (id: "27157680817475516", primary: 32182272603, title: "Title", description: "");
            _flickrConnectorMock = new Mock<IFlickrConnector>();
            _dateCalculatorMock = new Mock<IDateCalculator>();
            _flickrController = new FlickrController(_flickrConnectorMock.Object, _dateCalculatorMock.Object);
        }

        [Fact]
        public async Task GetAlbums_ReturnsListOfAlbums()
        {
            _flickrConnectorMock.Setup(f => f.GetPhotoSets(It.Is<UserData>(u => u == _userData))).ReturnsAsync(GetPhotoSetsResult(_photoSetDataValue)).Verifiable();

            var actionResult = await _flickrController.GetAlbums(_userData) as OkObjectResult;

            _flickrConnectorMock.Verify();
            Assert.NotNull(actionResult);
            Assert.Equal((int)System.Net.HttpStatusCode.OK, actionResult.StatusCode);
            Assert.Single((List<Album>)actionResult.Value);
            Assert.Equal(_photoSetDataValue.id, ((List<Album>)actionResult.Value)[0].Id);
            Assert.Equal(_photoSetDataValue.primary, ((List<Album>)actionResult.Value)[0].Primary);
            Assert.Equal(_photoSetDataValue.title, ((List<Album>)actionResult.Value)[0].Title);
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

            var actionResult = await _flickrController.GetAlbums(_userData) as OkObjectResult;

            _dateCalculatorMock.Verify();
            Assert.NotNull(actionResult);
            Assert.Equal((int)System.Net.HttpStatusCode.OK, actionResult.StatusCode);
            Assert.Single((List<Album>)actionResult.Value);
            Assert.Equal(new DateTime(2017, 5, 30), ((List<Album>)actionResult.Value)[0].FromDate);
            Assert.Equal(new DateTime(2017, 5, 31), ((List<Album>)actionResult.Value)[0].ToDate);
        }

        [Fact]
        public async Task GetPhotoSetPhotos_ReturnsPhotoIds()
        {
            long photoSetId = 27157680817475516;
            long photoId = 32182272603;
            _flickrConnectorMock.Setup(f => f.GetPhotosSetPhotos(It.Is<UserData>(u => u == _userData), It.Is<long>(i => i == photoSetId))).ReturnsAsync(GetPhotosResult(photoId)).Verifiable();

            var actionResult = await _flickrController.GetPhotoSetPhotos(_userData, photoSetId) as OkObjectResult;

            _flickrConnectorMock.Verify();
            Assert.NotNull(actionResult);
            Assert.Equal((int)System.Net.HttpStatusCode.OK, actionResult.StatusCode);
            Assert.Single((List<PhotoReference>)actionResult.Value);
            Assert.Equal(photoId, ((List<PhotoReference>)actionResult.Value)[0].Id);
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

            var actionResult = await _flickrController.GetPhoto(_userData, photoId) as OkObjectResult;

            _dateCalculatorMock.Verify();
            Assert.NotNull(actionResult);
            Assert.Equal((int)System.Net.HttpStatusCode.OK, actionResult.StatusCode);
            Assert.Equal(photoId, ((Photo)actionResult.Value).Id);
            Assert.Equal(square, ((Photo)actionResult.Value).Square);
            Assert.Equal(large, ((Photo)actionResult.Value).Large);
            Assert.Equal(original, ((Photo)actionResult.Value).Original);
            Assert.Equal(date, ((Photo)actionResult.Value).Taken);
        }

        private FlickrPhotoSetsResult GetPhotoSetsResult(params (string id, long primary, string title, string description)[] photoSetDataValues)
        {
            return new FlickrPhotoSetsResult()
            {
                PhotoSets = new FlickrPhotoSetsData
                {
                    PhotoSet = photoSetDataValues.Select(p => new FlickrPhotoSetData
                    {
                        Id = p.id,
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