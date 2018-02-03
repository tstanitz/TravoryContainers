using System;

namespace TravoryContainers.Services.Flickr.API.Model
{
    public class Album
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public long Primary { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}