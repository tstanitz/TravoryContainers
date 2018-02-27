using System;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using IntegrationTest.Helpers;
using Microsoft.AspNetCore.TestHost;
using Xunit;

namespace IntegrationTest.Flickr
{
    public class FlickrScenarios : FlickrScenariosBase
    {
        private readonly UserDataProvider _userDataProvider;
        private readonly IdProvider _idProvider;

        public FlickrScenarios()
        {
            _userDataProvider = new UserDataProvider();
            _idProvider = new IdProvider();
        }
        [Fact]
        public async Task Get_albums_and_response_ok_status_code()
        {
            var request = GetRequest(Get.Albums);

            var response = await request.GetAsync();

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Post_photosetphotos_and_response_ok_status_code()
        {
            var request = GetRequest(Post.PhotoSetPhotoIds(_idProvider.GetPhotoSetId));

            var response = await request.GetAsync();

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Post_photo_and_response_ok_status_code()
        {
            var request = GetRequest(Post.Photo(_idProvider.GetPhotoId));

            var response = await request.GetAsync();

            response.EnsureSuccessStatusCode();
        }

        private RequestBuilder GetRequest(string url)
        {
            var server = CreateServer();

            var request = server.CreateRequest(url);
            var userData = _userDataProvider.GetUserData();
            request.AddHeader("Consumer", EncodeHeader(userData.ConsumerKey, userData.ConsumerSecret));
            request.AddHeader("Token", EncodeHeader(userData.Token, userData.TokenSecret));

            return request;
        }

        private string EncodeHeader(string key, string value)
        {
            Encoding encoding = Encoding.GetEncoding("iso-8859-1");
            string credential = String.Format(CultureInfo.InvariantCulture, "{0}:{1}", key, value);

            return Convert.ToBase64String(encoding.GetBytes(credential));
        }
    }
}
