using System.Net.Http;
using System.Threading.Tasks;
using Moq;
using TravoryContainers.Services.Flickr.API.Connector;
using TravoryContainers.Services.Flickr.API.Connector.OAuthParameterHandling;
using TravoryContainers.Services.Flickr.API.Helpers;
using TravoryContainers.Services.Flickr.API.Model;
using Xunit;

namespace UnitTest.Flickr
{
    public class FlickrConnectorTest
    {
        private readonly IFlickrConnector _connector;
        private readonly UserData _userData = new UserData();
        private readonly HttpMessageHandlerStub _httpMessageHandlerStub;

        public FlickrConnectorTest()
        {
            var oAuthParameterHandlerMock = new Mock<IOAuthParameterHandler>();
            _httpMessageHandlerStub = new HttpMessageHandlerStub();
            var htpClientFactoryMock = new Mock<IHttpClientFactory>();
            htpClientFactoryMock.Setup(h => h.Client).Returns(new HttpClient(_httpMessageHandlerStub));
            _connector = new FlickrConnector(oAuthParameterHandlerMock.Object, htpClientFactoryMock.Object);
        }

        [Fact]
        public async Task GetPhotoSets_SuccessfullResponse()
        {
            _httpMessageHandlerStub.Content = "jsonFlickrApi({\"photosets\":{\"cancreate\":1,\"page\":1,\"pages\":1,\"perpage\":5,\"total\":2,\"photoset\":[" +
                "{\"id\":\"27157680817475516\",\"primary\":\"32182272603\",\"secret\":\"4f31b3981d\",\"server\":\"681\",\"farm\":1,\"photos\":2,\"videos\":\"0\",\"title\":{\"_content\":\"Test photoset 1\"},\"description\":{\"_content\":\"2017. 05. 30. - 2017. 05. 31.\"},\"needs_interstitial\":0,\"visibility_can_see_set\":1,\"count_views\":\"0\",\"count_comments\":\"0\",\"can_comment\":1,\"date_create\":\"1488385423\",\"date_update\":\"1488824376\"}," +
                "{\"id\":\"27157680817475517\",\"primary\":\"32182272604\",\"secret\":\"4f31b3981d\",\"server\":\"681\",\"farm\":1,\"photos\":1,\"videos\":\"0\",\"title\":{\"_content\":\"Test photoset 2\"},\"description\":{\"_content\":\"2017. 05. 30.\"},\"needs_interstitial\":0,\"visibility_can_see_set\":1,\"count_views\":\"0\",\"count_comments\":\"0\",\"can_comment\":1,\"date_create\":\"1487704807\",\"date_update\":\"0\"}]},\"stat\":\"ok\"})";

            var photoSetsResult = await _connector.GetPhotoSets(_userData);

            Assert.Equal(2, photoSetsResult.PhotoSets.PhotoSet.Count);
            Assert.Equal("27157680817475516", photoSetsResult.PhotoSets.PhotoSet[0].Id);
            Assert.Equal("Test photoset 1", photoSetsResult.PhotoSets.PhotoSet[0].Title._Content);
            Assert.Equal("2017. 05. 30. - 2017. 05. 31.", photoSetsResult.PhotoSets.PhotoSet[0].Description._Content);
            Assert.Equal("32182272603", photoSetsResult.PhotoSets.PhotoSet[0].Primary);

            Assert.Equal("27157680817475517", photoSetsResult.PhotoSets.PhotoSet[1].Id);
            Assert.Equal("Test photoset 2", photoSetsResult.PhotoSets.PhotoSet[1].Title._Content);
            Assert.Equal("2017. 05. 30.", photoSetsResult.PhotoSets.PhotoSet[1].Description._Content);
            Assert.Equal("32182272604", photoSetsResult.PhotoSets.PhotoSet[1].Primary);
        }
    }
}