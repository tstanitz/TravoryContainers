    namespace TravoryContainers.Services.Flickr.API.Connector.OAuthParameterHandling.RequestParameters
{
    public class TokenRequestParameter : RequestParameter
    {
        public TokenRequestParameter(string value)
            : base("oauth_token", value)
        {

        }
    }
}