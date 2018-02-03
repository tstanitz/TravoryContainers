using System;

namespace TravoryContainers.Services.Flickr.API.Helpers
{
    public interface IDateCalculator
    {
        DateTime? GetDate(string dateString);
    }
}