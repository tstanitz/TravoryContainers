using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
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
        private readonly IOAuthParameterHandlerFactory _oAuthParameterHandlerFactory;
        private readonly HttpClient _httpClient;

        public FlickrConnector(IOAuthParameterHandlerFactory oAuthParameterHandlerFactory, IHttpClientFactory httpClientFactory)
        {
            _oAuthParameterHandlerFactory = oAuthParameterHandlerFactory;
            _httpClient = httpClientFactory.Client;
        }

        public async Task<FlickrPhotoSetsResult> GetPhotoSets(UserData userData)
        {
            var oAuthParameterHandler = _oAuthParameterHandlerFactory.CreateHandler(userData);

            oAuthParameterHandler.AddAdditionalParameter("method", FlickrMethod.GetPhotoSets);
            oAuthParameterHandler.AddAdditionalParameter("format", "json");

            var endPoint = new FlickrRestEndPoint();
            oAuthParameterHandler.AddSignature(endPoint);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("OAuth", oAuthParameterHandler.GetAuthenticationHeader());
            var response = await _httpClient.GetAsync($"{endPoint.Url}?{oAuthParameterHandler.GetQueryString()}");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            return ParseAndCheckResult<FlickrPhotoSetsResult>(responseString);
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