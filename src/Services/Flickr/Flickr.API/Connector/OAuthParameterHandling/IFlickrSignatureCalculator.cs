namespace TravoryContainers.Services.Flickr.API.Connector.OAuthParameterHandling
{
    public interface IFlickrSignatureCalculator
    {
        string CalculateSignature(string consumerSecret, string tokenSecret, string baseString);
    }
}