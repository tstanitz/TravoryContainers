namespace TravoryContainers.Services.Flickr.API.Connector.OAuthParameterHandling.RequestParameters
{
    public class ConsumerKeyRequestParameter : RequestParameter
    {
        public ConsumerKeyRequestParameter(string value)
            : base("oauth_consumer_key", value)
        {

        }
    }
}