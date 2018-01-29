using System;

namespace TravoryContainers.Services.Flickr.API.Connector.OAuthParameterHandling
{
    public class OAuthDataProvider : IOAuthDataProvider
    {
        public string GetTimestamp() => $"{(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds}";

        public string GetNonce() => Guid.NewGuid().ToString("N");
    }
}