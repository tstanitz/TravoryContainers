using System;
using System.Security.Cryptography;
using System.Text;

namespace TravoryContainers.Services.Flickr.API.Connector.OAuthParameterHandling
{
    public class FlickrSignatureCalculator : IFlickrSignatureCalculator
    {
        public string CalculateSignature(string consumerSecret, string tokenSecret, string baseString)
        {
            var keyBytes = Encoding.UTF8.GetBytes($"{consumerSecret}&{tokenSecret}");

            HMACSHA1 sha1 = new HMACSHA1(keyBytes);
            byte[] signatureBytes = sha1.ComputeHash(Encoding.UTF8.GetBytes(baseString));
            return Convert.ToBase64String(signatureBytes);
        }
    }
}