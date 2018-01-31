using System.Collections.Generic;
using TravoryContainers.Services.Flickr.API.Connector.EndPoints;
using TravoryContainers.Services.Flickr.API.Connector.OAuthParameterHandling.RequestParameters;
using TravoryContainers.Services.Flickr.API.Model;

namespace TravoryContainers.Services.Flickr.API.Connector.OAuthParameterHandling
{
    public interface IOAuthParameterHandler
    {
        IList<RequestParameter> OAuthParameters { get; }
        void AddUserParameters(UserData userData);
        void AddAdditionalParameter(string key, string value);
        void AddSignature(FlickrEndPointBase endpoint);
        string GetAuthenticationHeader();
        string GetQueryString();
        IList<RequestParameter> GetAdditionalParameters();
    }
}