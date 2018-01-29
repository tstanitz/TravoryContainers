using System;
using System.Collections.Generic;
using System.Linq;
using TravoryContainers.Services.Flickr.API.Connector.EndPoints;
using TravoryContainers.Services.Flickr.API.Connector.OAuthParameterHandling.RequestParameters;
using TravoryContainers.Services.Flickr.API.Model;
using static System.String;

namespace TravoryContainers.Services.Flickr.API.Connector.OAuthParameterHandling
{
    public class OAuthParameterHandler : IOAuthParameterHandler
    {
        private readonly IOAuthDataProvider _oAuthDataProvider;
        private readonly IFlickrSignatureCalculator _signatureCalculator;
        private string _consumerSecret;
        private string _tokenSecret;

        public OAuthParameterHandler(IOAuthDataProvider oAuthDataProvider, IFlickrSignatureCalculator signatureCalculator)
        {
            _oAuthDataProvider = oAuthDataProvider;
            _signatureCalculator = signatureCalculator;
        }

        public IList<RequestParameter> OAuthParameters { get; private set; }

        public void Initialize(UserData userData)
        {
            _consumerSecret = userData.ConsumerSecret;
            _tokenSecret = userData.TokenSecret;
            OAuthParameters = new List<RequestParameter>
            {
                new ConsumerKeyRequestParameter(userData.ConsumerKey),
                new SignatureMethodRequestParameter(),
                new VersionRequestParameter(),
                new TokenRequestParameter(userData.Token),
                new TimestampRequestParameter(_oAuthDataProvider.GetTimestamp()),
                new NonceRequestParameter(_oAuthDataProvider.GetNonce())
            };
        }

        public void AddAdditionalParameter(string key, string value)
        {
            OAuthParameters.Add(new AdditionalRequestParameter(key, value));
        }

        private string GetParametersString()
        {
            return Uri.EscapeDataString(Join("&", OAuthParameters.OrderBy(r => r.ParameterName).Select(op => op.EncodedValue)));
        }

        public void AddSignature(FlickrEndPointBase endpoint)
        {
            var baseString = $"{endpoint.GetEndPoint()}&{GetParametersString()}";
            var signature = _signatureCalculator.CalculateSignature(_consumerSecret, _tokenSecret, baseString);

            OAuthParameters.Add(new SignatureRequestParameter(signature));
        }

        public string GetAuthenticationHeader()
        {
            return Join(",", OAuthParameters.Where(p => !(p is AdditionalRequestParameter)).OrderBy(r => r.ParameterName).Select(op => op.EncodedQuotedValue));
        }

        public string GetQueryString()
        {
            return Join("&", OAuthParameters.Where(p => p is AdditionalRequestParameter).OrderBy(r => r.ParameterName).Select(op => op.GetValue));
        }

        public IList<RequestParameter> GetAdditionalParameters()
        {
            return OAuthParameters.Where(p => p is AdditionalRequestParameter).ToList();
        }
    }
}