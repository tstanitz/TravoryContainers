using System;

namespace TravoryContainers.Services.Flickr.API.Connector.EndPoints
{
    public abstract class FlickrEndPointBase
    {
        public string Url { get; protected set; }
        public string MethodType { get; protected set; }

        public string GetEndPoint()
        {
            return $"{MethodType}&{Uri.EscapeDataString(Url)}";
        }
    }
}