using TravoryContainers.Services.Flickr.API.Model;

namespace TravoryContainers.Services.Flickr.API.Connector.OAuthParameterHandling
{
    public interface IOAuthParameterHandlerFactory
    {
        IOAuthParameterHandler CreateHandler(UserData userData);
    }
}