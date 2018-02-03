using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TravoryContainers.Services.Flickr.API.Connector;
using TravoryContainers.Services.Flickr.API.Connector.FlickrResults;
using TravoryContainers.Services.Flickr.API.Controllers;
using TravoryContainers.Services.Flickr.API.Model;
using Xunit;

namespace UnitTest.Flickr
{
    public class FlickrControllerTest
    {
        [Fact]
        public async Task GetAlbums_ReturnsListOfAlbums()
        {
            var flickrConnectorMock = new Mock<IFlickrConnector>();
            var controller = new FlickrController(flickrConnectorMock.Object);
            var userData = new UserData();
            string id = "27157680817475516";
            string title = "Title";
            flickrConnectorMock.Setup(f => f.GetPhotoSets(It.Is<UserData>(u => u == userData))).ReturnsAsync(new FlickrPhotoSetsResult()
            {
                PhotoSets = new FlickrPhotoSetsData()
                {
                    PhotoSet = new List<FlickrPhotoSetData>()
                    {
                        new FlickrPhotoSetData()
                        {
                            Id = id,
                            Title = new FlickrContentData()
                            {
                                _Content = title
                            }
                        }
                    }
                }
            }).Verifiable();
            
            var actionResult = await controller.GetAlbums(userData) as OkObjectResult;

            flickrConnectorMock.Verify();
            Assert.NotNull(actionResult);
            Assert.Equal((int)System.Net.HttpStatusCode.OK, actionResult.StatusCode);
            Assert.Single((List<Album>)actionResult.Value);
            Assert.Equal(long.Parse(id), ((List<Album>)actionResult.Value)[0].Id);
            Assert.Equal(title, ((List<Album>)actionResult.Value)[0].Title);
        }
    }
}