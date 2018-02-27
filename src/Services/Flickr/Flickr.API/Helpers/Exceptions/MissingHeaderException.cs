using System;

namespace TravoryContainers.Services.Flickr.API.Helpers.Exceptions
{
    [Serializable]
    public class MissingHeaderException : Exception
    {
        public MissingHeaderException(string message) : base(message)
        {
        }
    }
}
