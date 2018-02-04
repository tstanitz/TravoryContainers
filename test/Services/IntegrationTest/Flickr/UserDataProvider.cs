using Newtonsoft.Json;
using TravoryContainers.Services.Flickr.API.Model;

namespace IntegrationTest.Flickr
{
    public class UserDataProvider : EnvironmentVariableHandlerBase
    {
        public string BuildUserData()
        {
            return JsonConvert.SerializeObject(new UserData
            {
                ConsumerKey = GetEnvironmentVariable(ConfigurationKeys.ConsumerKey),
                ConsumerSecret = GetEnvironmentVariable(ConfigurationKeys.ConsumerSecret),
                Token = GetEnvironmentVariable(ConfigurationKeys.Token),
                TokenSecret = GetEnvironmentVariable(ConfigurationKeys.TokenSecret)
            });
        }        
    }
}