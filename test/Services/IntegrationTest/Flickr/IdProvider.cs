namespace IntegrationTest.Flickr
{
    public class IdProvider : EnvironmentVariableHandlerBase
    {
        public long GetPhotoId => GetEnvironmentVariableLong(ConfigurationKeys.PhotoId);
        public long GetPhotoSetId => GetEnvironmentVariableLong(ConfigurationKeys.PhotoSetId);
    }
}