using System;
using System.Globalization;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using TravoryContainers.Services.Flickr.API.Helpers.Exceptions;

namespace TravoryContainers.Services.Flickr.API.Helpers
{
    public static class HttpHeaderReaderExtension
    {
        public static (string token, string tokenSecret) GetToken(this HttpContext context)
        {            
            var parts = Decode(context, "Token");
            return (token: parts[0], tokenSecret: parts[1]); 
        }

        public static (string consumerKey, string consumerSecret) GetConsumer(this HttpContext context)
        {
            var parts = Decode(context, "Consumer");
            return (consumerKey: parts[0], consumerSecret: parts[1]);
        }

        private static string[] Decode(HttpContext context, string header)
        {
            var headerData = context.Request.Headers[header];
            if (headerData == StringValues.Empty)
            {
                throw new MissingHeaderException(header);
            }

            Encoding encoding = Encoding.GetEncoding("iso-8859-1");
            var decodedBytes = Convert.FromBase64String(headerData.ToString());

            return encoding.GetString(decodedBytes).Split(':');
        }
    }


}