using System.Collections.Generic;
using System.Linq;
using Moq;
using TravoryContainers.Services.Flickr.API.Connector.EndPoints;
using TravoryContainers.Services.Flickr.API.Connector.OAuthParameterHandling;
using TravoryContainers.Services.Flickr.API.Connector.OAuthParameterHandling.RequestParameters;
using TravoryContainers.Services.Flickr.API.Model;
using Xunit;

namespace UnitTest.Flickr
{
    public class OAuthParameterHandlerTest
    {
        private readonly Mock<IFlickrSignatureCalculator> _signatureCalculatorMock;
        private readonly UserData _userData;
        private readonly string timestamp = "1487531026";
        private readonly string nonce = "d853dfc999a94ca6b91625222b13165b";
        private readonly Mock<IOAuthDataProvider> _oAuthDataProviderMock;

        public OAuthParameterHandlerTest()
        {
            _oAuthDataProviderMock = new Mock<IOAuthDataProvider>();
            _oAuthDataProviderMock.Setup(o => o.GetTimestamp()).Returns(timestamp);
            _oAuthDataProviderMock.Setup(o => o.GetNonce()).Returns(nonce);

            _signatureCalculatorMock = new Mock<IFlickrSignatureCalculator>();            
            _userData = new UserData
            {
                ConsumerKey = "84f0b045885f5d7541d96fbf0457ahgf",
                ConsumerSecret = "43a9b4583e54e5de",
                Token = "77846176941945127-4202641874561rg",
                TokenSecret = "7a245781d0370gf8"
            };
        }

        [Fact]
        public void Initialize_AddDefaultParameters()
        {
            var oAuthParameterHandler = new OAuthParameterHandler(_oAuthDataProviderMock.Object, _signatureCalculatorMock.Object);
            oAuthParameterHandler.AddUserParameters(_userData);

            var parameters = oAuthParameterHandler.OAuthParameters;

            Assert.NotNull(parameters);
            AssertRequestParameter<ConsumerKeyRequestParameter>(parameters, "oauth_consumer_key", _userData.ConsumerKey);
            AssertRequestParameter<SignatureMethodRequestParameter>(parameters, "oauth_signature_method", "HMAC-SHA1");
            AssertRequestParameter<VersionRequestParameter>(parameters, "oauth_version", "1.0");
            AssertRequestParameter<TokenRequestParameter>(parameters, "oauth_token", _userData.Token);
            AssertRequestParameter<TimestampRequestParameter>(parameters, "oauth_timestamp", timestamp);
            AssertRequestParameter<NonceRequestParameter>(parameters, "oauth_nonce", nonce);
        }

        [Fact]
        public void AddAdditionalParameter_AddAdditionalRequestParameter()
        {
            string key = "user_id";
            string value = "112549080@D84";
            var oAuthParameterHandler = new OAuthParameterHandler(_oAuthDataProviderMock.Object, _signatureCalculatorMock.Object);
            oAuthParameterHandler.AddUserParameters(_userData);

            oAuthParameterHandler.AddAdditionalParameter(key, value);

            var parameters = oAuthParameterHandler.OAuthParameters;

            Assert.NotNull(parameters);
            AssertRequestParameter<AdditionalRequestParameter>(parameters, key, value);
        }

        [Fact]
        public void AddSignature_AddSignatureRequestParameter()
        {
            var endpoint = new FlickrRestEndPoint();
            var signature = "+PKp4GzLPLq3fvIObMcwe76nFIk=";
            _signatureCalculatorMock.Setup(s => s.CalculateSignature(It.Is<string>(c => c == _userData.ConsumerSecret), It.Is<string>(t => t == _userData.TokenSecret), It.IsAny<string>())).Returns(signature).Verifiable();
            var oAuthParameterHandler = new OAuthParameterHandler(_oAuthDataProviderMock.Object, _signatureCalculatorMock.Object);
            oAuthParameterHandler.AddUserParameters(_userData);

            oAuthParameterHandler.AddSignature(endpoint);

            var parameters = oAuthParameterHandler.OAuthParameters;

            Assert.NotNull(parameters);
            _signatureCalculatorMock.Verify();
            AssertRequestParameter<SignatureRequestParameter>(parameters, "oauth_signature", signature);
        }

        [Fact]
        public void GetAuthenticationHeader_ReturnsBasicEncodedParameters()
        {
            var oAuthParameterHandler = new OAuthParameterHandler(_oAuthDataProviderMock.Object, _signatureCalculatorMock.Object);
            oAuthParameterHandler.AddUserParameters(_userData);
            oAuthParameterHandler.AddAdditionalParameter("cName", "value:c");

            var result = oAuthParameterHandler.GetAuthenticationHeader();

            Assert.Equal("oauth_consumer_key=\"84f0b045885f5d7541d96fbf0457ahgf\",oauth_nonce=\"d853dfc999a94ca6b91625222b13165b\",oauth_signature_method=\"HMAC-SHA1\",oauth_timestamp=\"1487531026\",oauth_token=\"77846176941945127-4202641874561rg\",oauth_version=\"1.0\"", result);
        }

        [Fact]
        public void GetQueryString_ReturnsEncodedAdditionalParameters()
        {
            var oAuthParameterHandler = new OAuthParameterHandler(_oAuthDataProviderMock.Object, _signatureCalculatorMock.Object);
            oAuthParameterHandler.AddUserParameters(_userData);
            oAuthParameterHandler.AddAdditionalParameter("aName", "value:a");
            oAuthParameterHandler.AddAdditionalParameter("cName", "value:c");

            var result = oAuthParameterHandler.GetQueryString();

            Assert.Equal("aName=value:a&cName=value:c", result);
        }

        [Fact]
        public void GetAdditionalParameters_ReturnsAdditionalParameters()
        {
            var oAuthParameterHandler = new OAuthParameterHandler(_oAuthDataProviderMock.Object, _signatureCalculatorMock.Object);
            oAuthParameterHandler.AddUserParameters(_userData);
            oAuthParameterHandler.AddAdditionalParameter("aName", "value:a");
            oAuthParameterHandler.AddAdditionalParameter("cName", "value:c");

            var result = oAuthParameterHandler.GetAdditionalParameters();

            Assert.Equal(2, result.Count);
            Assert.Equal("aName", result[0].ParameterName);
            Assert.Equal("value:a", result[0].ParameterValue);
            Assert.Equal("cName", result[1].ParameterName);
            Assert.Equal("value:c", result[1].ParameterValue);
        }

        private void AssertRequestParameter<T>(IList<RequestParameter> parameters, string parameterName, string parameterValue) where T : RequestParameter
        {
            var parameter = parameters.Single(p => p is T);
            Assert.NotNull(parameter);
            Assert.Equal(parameterName, parameter.ParameterName);
            Assert.Equal(parameterValue, parameter.ParameterValue);
        }
    }
}