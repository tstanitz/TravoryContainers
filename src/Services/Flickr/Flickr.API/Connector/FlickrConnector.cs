using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TravoryContainers.Services.Flickr.API.Connector.EndPoints;
using TravoryContainers.Services.Flickr.API.Connector.Exceptions;
using TravoryContainers.Services.Flickr.API.Connector.FlickrResults;
using TravoryContainers.Services.Flickr.API.Connector.OAuthParameterHandling;
using TravoryContainers.Services.Flickr.API.Helpers;
using TravoryContainers.Services.Flickr.API.Model;

namespace TravoryContainers.Services.Flickr.API.Connector
{
    public class FlickrConnector : IFlickrConnector
    {
        private readonly IOAuthParameterHandler _oAuthParameterHandler;
        private readonly HttpClient _httpClient;

        public FlickrConnector(IOAuthParameterHandler oAuthParameterHandler, IHttpClientFactory httpClientFactory)
        {
            _oAuthParameterHandler = oAuthParameterHandler;
            _httpClient = httpClientFactory.Client;
        }

        public async Task<FlickrPhotoSetsResult> GetPhotoSets([FromBody]UserData userData)
        {
            _oAuthParameterHandler.AddUserParameters(userData);

            _oAuthParameterHandler.AddAdditionalParameter("method", FlickrMethod.GetPhotoSets);
            _oAuthParameterHandler.AddAdditionalParameter("format", "json");

            var endPoint = new FlickrRestEndPoint();
            _oAuthParameterHandler.AddSignature(endPoint);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("OAuth", _oAuthParameterHandler.GetAuthenticationHeader());
            var response = await _httpClient.GetAsync($"{endPoint.Url}?{_oAuthParameterHandler.GetQueryString()}");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            return ParseAndCheckResult<FlickrPhotoSetsResult>(responseString);
        }

        public async Task<FlickrPhotoSizesResult> GetPhotoSizes(UserData userData, long photoId)
        {
            _oAuthParameterHandler.AddUserParameters(userData);

            _oAuthParameterHandler.AddAdditionalParameter("photo_id", $"{photoId}");
            _oAuthParameterHandler.AddAdditionalParameter("method", FlickrMethod.GetPhotoSizes);
            _oAuthParameterHandler.AddAdditionalParameter("format", "json");

            var endPoint = new FlickrRestEndPoint();
            _oAuthParameterHandler.AddSignature(endPoint);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("OAuth", _oAuthParameterHandler.GetAuthenticationHeader());
            var response = await _httpClient.GetAsync($"{endPoint.Url}?{_oAuthParameterHandler.GetQueryString()}");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            return ParseAndCheckResult<FlickrPhotoSizesResult>(responseString);
        }

        private T ParseAndCheckResult<T>(string jsonResult) where T : FlickrResult
        {
            var result = jsonResult.Substring(14, jsonResult.Length - 15);
            var flickrResult = JsonConvert.DeserializeObject<T>(result);
            if (flickrResult.Stat != "ok")
            {
                throw new FlickrConnectorException(flickrResult.Code, flickrResult.Message);
            }
            return flickrResult;
        }
    }
}