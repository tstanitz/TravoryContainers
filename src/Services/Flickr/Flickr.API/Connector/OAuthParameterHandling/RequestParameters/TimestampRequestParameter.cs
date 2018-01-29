namespace TravoryContainers.Services.Flickr.API.Connector.OAuthParameterHandling.RequestParameters
{
    public class TimestampRequestParameter : RequestParameter
    {
        public TimestampRequestParameter(string value)
            : base("oauth_timestamp", value)
        {

        }
    }
}