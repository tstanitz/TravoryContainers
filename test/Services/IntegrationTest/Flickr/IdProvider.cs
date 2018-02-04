namespace IntegrationTest.Flickr
{
    public class IdProvider : EnvironmentVariableHandlerBase
    {
        public long GetPhotoId => GetEnvironmentVariableLong(ConfigurationKeys.PhotoId);
    }
}