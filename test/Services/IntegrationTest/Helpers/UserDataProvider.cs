using TravoryContainers.Services.Flickr.API.Model;

namespace IntegrationTest.Helpers
{
    public class UserDataProvider : EnvironmentVariableHandlerBase
    {      
        public UserData GetUserData()
        {
            return new UserData
            {
                ConsumerKey = GetEnvironmentVariable(ConfigurationKeys.ConsumerKey),
                ConsumerSecret = GetEnvironmentVariable(ConfigurationKeys.ConsumerSecret),
                Token = GetEnvironmentVariable(ConfigurationKeys.Token),
                TokenSecret = GetEnvironmentVariable(ConfigurationKeys.TokenSecret)
            };
        }
    }
}