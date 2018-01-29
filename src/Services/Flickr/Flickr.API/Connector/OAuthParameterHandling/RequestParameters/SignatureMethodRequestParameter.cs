namespace TravoryContainers.Services.Flickr.API.Connector.OAuthParameterHandling.RequestParameters
{
    public class SignatureMethodRequestParameter : RequestParameter
    {
        public SignatureMethodRequestParameter()
            : base("oauth_signature_method", "HMAC-SHA1")
        {

        }
    }
}