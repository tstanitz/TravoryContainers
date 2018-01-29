using TravoryContainers.Services.Flickr.API.Model;

namespace TravoryContainers.Services.Flickr.API.Connector.OAuthParameterHandling
{
    public class OAuthParameterHandlerFactory : IOAuthParameterHandlerFactory
    {
        public IOAuthParameterHandler CreateHandler(UserData userData)
        {
            var handler = new OAuthParameterHandler(new OAuthDataProvider(), new FlickrSignatureCalculator());
            handler.Initialize(userData);
            return handler;
        }
    }
}