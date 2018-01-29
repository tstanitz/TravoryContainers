using System;
using System.Runtime.Serialization;

namespace TravoryContainers.Services.Flickr.API.Connector.Exceptions
{
    [Serializable]
    public class FlickrConnectorException : Exception
    {
        public string Code { get; }

        public FlickrConnectorException(string code, string message) : base(message)
        {
            Code = code;
        }
        public FlickrConnectorException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}