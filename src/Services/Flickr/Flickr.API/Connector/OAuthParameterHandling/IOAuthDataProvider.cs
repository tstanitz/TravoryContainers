namespace TravoryContainers.Services.Flickr.API.Connector.OAuthParameterHandling
{
    public interface IOAuthDataProvider
    {
        string GetTimestamp();

        string GetNonce();
    }
}