namespace TravoryContainers.Services.Flickr.API.Model
{
    public class UserData
    {
        public string ConsumerKey { get; set; }
        public string ConsumerSecret { get; set; }
        public string Token { get; set; }
        public string TokenSecret { get; set; }

        public UserData()
        {
            
        }

        public UserData((string consumerKey, string consumnerSecret) consumer, (string token, string tokenSecret) token)
        {
            ConsumerKey = consumer.consumerKey;
            ConsumerSecret = consumer.consumnerSecret;
            Token = token.token;
            TokenSecret = token.tokenSecret;
        }
    }
}