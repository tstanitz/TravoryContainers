using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
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
        public async Task Post_albums_and_response_ok_status_code()
        {
            using (var server = CreateServer())
            {
                var content = new StringContent(_userDataProvider.BuildUserData(), Encoding.UTF8, "application/json");
                var response = await server.CreateClient().PostAsync(Post.Albums, content);

                response.EnsureSuccessStatusCode();
            }
        }

        [Fact]
        public async Task Post_photosources_and_response_ok_status_code()
        {
            using (var server = CreateServer())
            {
                var content = new StringContent(_userDataProvider.BuildUserData(), Encoding.UTF8, "application/json");
                var response = await server.CreateClient().PostAsync(Post.PhotoSources(_idProvider.GetPhotoId), content);

                response.EnsureSuccessStatusCode();
            }
        }
    }
}
