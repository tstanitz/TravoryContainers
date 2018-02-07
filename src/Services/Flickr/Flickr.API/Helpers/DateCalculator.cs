using System;

namespace TravoryContainers.Services.Flickr.API.Helpers
{
    public class DateCalculator : IDateCalculator
    {
        public DateTime? GetDate(string dateString)
        {
            if (!string.IsNullOrEmpty(dateString) && DateTime.TryParse(dateString, out var date))
            {
                return date.Date;
            }

            return null;
        }

        public DateTime? GetDateAndTime(string dateString)
        {
            if (!string.IsNullOrEmpty(dateString) && DateTime.TryParse(dateString, out var date))
            {
                return date;
            }

            return null;            
        }

        private DateTime RemoveMilliseconds(DateTime dateTime)
        {
            return new DateTime(dateTime.Ticks - (dateTime.Ticks % TimeSpan.TicksPerSecond), dateTime.Kind);
        }
    }
}