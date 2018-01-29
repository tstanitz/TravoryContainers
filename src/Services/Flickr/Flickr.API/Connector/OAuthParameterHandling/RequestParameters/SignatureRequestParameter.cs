namespace TravoryContainers.Services.Flickr.API.Connector.OAuthParameterHandling.RequestParameters
{
    public class SignatureRequestParameter : RequestParameter
    {
        public SignatureRequestParameter(string value)
            : base("oauth_signature", value)
        {
        }
    }
}