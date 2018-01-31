using System;
using Newtonsoft.Json;
using TravoryContainers.Services.Flickr.API.Model;

namespace IntegrationTest.Flickr
{
    public static class UserDataProvider
    {
        public static string BuildUserData()
        {
            return JsonConvert.SerializeObject(new UserData
            {
                ConsumerKey = GetEnvironmentVariable(ConfigurationKeys.ConsumerKey),
                ConsumerSecret = GetEnvironmentVariable(ConfigurationKeys.ConsumerSecret),
                Token = GetEnvironmentVariable(ConfigurationKeys.Token),
                TokenSecret = GetEnvironmentVariable(ConfigurationKeys.TokenSecret)
            });
        }
        private static string GetEnvironmentVariable(string variableName)
        {
            return Environment.GetEnvironmentVariable(variableName) ?? throw new ArgumentException($"Missing environment variable: {variableName}");
        }
    }
}