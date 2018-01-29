namespace TravoryContainers.Services.Flickr.API.Connector.OAuthParameterHandling.RequestParameters
{
    public class NonceRequestParameter : RequestParameter
    {
        public NonceRequestParameter(string value)
            : base("oauth_nonce", value)
        {

        }
    }
}