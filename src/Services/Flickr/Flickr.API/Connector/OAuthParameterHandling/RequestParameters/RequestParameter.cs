using System;

namespace TravoryContainers.Services.Flickr.API.Connector.OAuthParameterHandling.RequestParameters
{
    public abstract class RequestParameter
    {
        public string ParameterName { get; }
        public string ParameterValue { get; }

        protected RequestParameter(string name, string value)
        {
            ParameterName = name;
            ParameterValue = value;
        }

        public string GetValue => $"{ParameterName}={ParameterValue}";
        public string EncodedValue => $"{ParameterName}={Uri.EscapeDataString(ParameterValue)}";
        public string EncodedQuotedValue => $"{ParameterName}=\"{Uri.EscapeDataString(ParameterValue)}\"";
    }
}