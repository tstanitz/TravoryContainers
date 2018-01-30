using TravoryContainers.Services.Flickr.API.Connector.OAuthParameterHandling;
using Xunit;

namespace UnitTest.Flickr
{
    public class FlickrSignatureCalculatorTest
    {
        [Fact]
        public void CalculateSignature_CalculatesSignatureBasedOnBaseString()
        {
            var customerSecret = "43a9b4583e54e5de";
            var tokenSecret = "7a245781d0370gf8";
            var baseString = "GET&flickr.photosets.getList&aName=value%3Aa&bName=value%3Ab&cName=value%3Ac";
            var signatureCalculator = new FlickrSignatureCalculator();

            var result = signatureCalculator.CalculateSignature(customerSecret, tokenSecret, baseString);

            Assert.Equal("kUOBe0GsjTChEon/40dGKxUDlmg=", result);
        }
    }
}
