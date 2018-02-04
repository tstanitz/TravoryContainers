using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTest.Flickr
{
    public class FlickrScenarios : FlickrScenariosBase
    {        
        [Fact]
        public async Task Post_albums_and_response_ok_status_code()
        {
            using (var server = CreateServer())
            {
                var content = new StringContent(UserDataProvider.BuildUserData(), Encoding.UTF8, "application/json");
                var response = await server.CreateClient().PostAsync(Post.Albums, content);

                response.EnsureSuccessStatusCode();
            }
        }       
    }
}
