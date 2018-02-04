using System.Linq;
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

        [Fact]
        public async Task GetPhotoSizes_SuccessfullResponse()
        {
            _httpMessageHandlerStub.Content = "jsonFlickrApi({\"sizes\":{\"canblog\":1,\"canprint\":1,\"candownload\":1,\"size\":" +
                "[" +
                    "{\"label\":\"Square\",\"width\":75,\"height\":75,\"source\":\"https:\\/\\/farm5.staticflickr.com\\/7654\\/5435435463_square.jpg\",\"url\":\"https:\\/\\/www.flickr.com\\/photos\\/3452562@M87\\/435436346\\/sizes\\/sq\\/\",\"media\":\"photo\"}," +
                    "{\"label\":\"Large Square\",\"width\":150,\"height\":150,\"source\":\"https:\\/\\/farm5.staticflickr.com\\/7654\\/5435435463_large_square_s.jpg\",\"url\":\"https:\\/\\/www.flickr.com\\/photos\\/3452562@M87\\/435436346\\/sizes\\/q\\/\",\"media\":\"photo\"}," +
                    "{\"label\":\"Thumbnail\",\"width\":100,\"height\":67,\"source\":\"https:\\/\\/farm5.staticflickr.com\\/7654\\/5435435463_thumb_s.jpg\",\"url\":\"https:\\/\\/www.flickr.com\\/photos\\/3452562@M87\\/435436346\\/sizes\\/t\\/\",\"media\":\"photo\"}," +
                    "{\"label\":\"Small\",\"width\":\"240\",\"height\":\"160\",\"source\":\"https:\\/\\/farm5.staticflickr.com\\/7654\\/5435435463_small.jpg\",\"url\":\"https:\\/\\/www.flickr.com\\/photos\\/3452562@M87\\/435436346\\/sizes\\/s\\/\",\"media\":\"photo\"}," +
                    "{\"label\":\"Small 320\",\"width\":\"320\",\"height\":\"213\",\"source\":\"https:\\/\\/farm5.staticflickr.com\\/7654\\/5435435463_small320.jpg\",\"url\":\"https:\\/\\/www.flickr.com\\/photos\\/3452562@M87\\/435436346\\/sizes\\/s3\\/\",\"media\":\"photo\"}," +
                    "{\"label\":\"Medium\",\"width\":\"500\",\"height\":\"333\",\"source\":\"https:\\/\\/farm5.staticflickr.com\\/7654\\/5435435463_medium.jpg\",\"url\":\"https:\\/\\/www.flickr.com\\/photos\\/3452562@M87\\/435436346\\/sizes\\/m\\/\",\"media\":\"photo\"}," +
                    "{\"label\":\"Medium 640\",\"width\":\"640\",\"height\":\"427\",\"source\":\"https:\\/\\/farm5.staticflickr.com\\/7654\\/5435435463_medium640.jpg\",\"url\":\"https:\\/\\/www.flickr.com\\/photos\\/3452562@M87\\/435436346\\/sizes\\/m6\\/\",\"media\":\"photo\"}," +
                    "{\"label\":\"Medium 800\",\"width\":\"800\",\"height\":\"534\",\"source\":\"https:\\/\\/farm5.staticflickr.com\\/7654\\/5435435463_medium800.jpg\",\"url\":\"https:\\/\\/www.flickr.com\\/photos\\/3452562@M87\\/435436346\\/sizes\\/m8\\/\",\"media\":\"photo\"}," +
                    "{\"label\":\"Large\",\"width\":\"1024\",\"height\":\"768\",\"source\":\"https:\\/\\/farm5.staticflickr.com\\/7654\\/5435435463_large.jpg\",\"url\":\"https:\\/\\/www.flickr.com\\/photos\\/3452562@M87\\/435436346\\/sizes\\/l\\/\",\"media\":\"photo\"}," +
                    "{\"label\":\"Large 1600\",\"width\":\"1600\",\"height\":1200,\"source\":\"https:\\/\\/farm5.staticflickr.com\\/7654\\/5435435463_large1600.jpg\",\"url\":\"https:\\/\\/www.flickr.com\\/photos\\/3452562@M87\\/435436346\\/sizes\\/h\\/\",\"media\":\"photo\"}," +
                    "{\"label\":\"Large 2048\",\"width\":\"2048\",\"height\":1536,\"source\":\"https:\\/\\/farm5.staticflickr.com\\/7654\\/5435435463_latge2800.jpg\",\"url\":\"https:\\/\\/www.flickr.com\\/photos\\/3452562@M87\\/435436346\\/sizes\\/k\\/\",\"media\":\"photo\"}," +
                    "{\"label\":\"Original\",\"width\":\"3264\",\"height\":\"2448\",\"source\":\"https:\\/\\/farm5.staticflickr.com\\/7654\\/5435435463_original.jpg\",\"url\":\"https:\\/\\/www.flickr.com\\/photos\\/3452562@M87\\/435436346\\/sizes\\/o\\/\",\"media\":\"photo\"}" +
                "]},\"stat\":\"ok\"})";

            var flickrPhotoSizesResult = await _connector.GetPhotoSizes(_userData, 32182272603);

            Assert.Equal("https://farm5.staticflickr.com/7654/5435435463_square.jpg", flickrPhotoSizesResult.Sizes.Size.FirstOrDefault(s => s.Label == "Square")?.Source);
            Assert.Equal("https://farm5.staticflickr.com/7654/5435435463_large.jpg", flickrPhotoSizesResult.Sizes.Size.FirstOrDefault(s => s.Label == "Large")?.Source);
            Assert.Equal("https://farm5.staticflickr.com/7654/5435435463_original.jpg", flickrPhotoSizesResult.Sizes.Size.FirstOrDefault(s => s.Label == "Original")?.Source);
        }

        [Fact]
        public async Task GetPhotoSetPhotos_SuccessfullResponse()
        {
            _httpMessageHandlerStub.Content = "jsonFlickrApi({\"photoset\":{\"id\":\"72157680817475516\",\"primary\":\"34546724775\",\"owner\":\"148184080@N02\",\"ownername\":\"tstanitz\",\"photo\":[" +
                                             "{\"id\":\"34546724775\",\"secret\":\"e0a5a8e527\",\"server\":\"4173\",\"farm\":5,\"title\":\"Test Title\",\"isprimary\":\"1\",\"ispublic\":0,\"isfriend\":0,\"isfamily\":0}," +
                                             "{\"id\":\"34171365780\",\"secret\":\"37d23dd9c3\",\"server\":\"4184\",\"farm\":5,\"title\":\"Test Title\",\"isprimary\":\"0\",\"ispublic\":0,\"isfriend\":0,\"isfamily\":0}],\"page\":1,\"per_page\":500,\"perpage\":500,\"pages\":1,\"total\":\"5\",\"title\":\"Test photoset\"},\"stat\":\"ok\"})";

            var flickrPhotosResult = await _connector.GetPhotosSetPhotos(_userData, 27157680817475516);

            Assert.Equal(2, flickrPhotosResult.PhotoSet.Photo.Count);
            Assert.Equal("34546724775", flickrPhotosResult.PhotoSet.Photo[0].Id);
            Assert.Equal("1", flickrPhotosResult.PhotoSet.Photo[0].IsPrimary);
            Assert.Equal("34171365780", flickrPhotosResult.PhotoSet.Photo[1].Id);
            Assert.Equal("0", flickrPhotosResult.PhotoSet.Photo[1].IsPrimary);
        }
    }
}