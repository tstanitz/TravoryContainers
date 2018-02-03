﻿using System;
using System.Collections.Generic;
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
        private (long id, long primary, string title, string description) _photoSetDataValue;
        private readonly UserData _userData;
        private readonly Mock<IFlickrConnector> _flickrConnectorMock;
        private readonly FlickrController _flickrController;
        private readonly Mock<IDateCalculator> _dateCalculatorMock;

        public FlickrControllerTest()
        {
            _userData = new UserData();
            _photoSetDataValue = (id: 27157680817475516, primary: 32182272603, title: "Title", description: "");
            _flickrConnectorMock = new Mock<IFlickrConnector>();
            _dateCalculatorMock = new Mock<IDateCalculator>();
            _flickrController = new FlickrController(_flickrConnectorMock.Object, _dateCalculatorMock.Object);
        }
        [Fact]
        public async Task GetAlbums_ReturnsListOfAlbums()
        {
            _flickrConnectorMock.Setup(f => f.GetPhotoSets(It.Is<UserData>(u => u == _userData))).ReturnsAsync(GetFlickrPhotoSetsResult(_photoSetDataValue)).Verifiable();

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
            _flickrConnectorMock.Setup(f => f.GetPhotoSets(It.Is<UserData>(u => u == _userData))).ReturnsAsync(GetFlickrPhotoSetsResult(_photoSetDataValue)).Verifiable();
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



        private static FlickrPhotoSetsResult GetFlickrPhotoSetsResult(params (long id, long primary, string title, string description)[] photoSetDataValues)
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
    }
}