using System;

namespace IntegrationTest.Flickr
{
    public abstract class EnvironmentVariableHandlerBase
    {
        protected string GetEnvironmentVariable(string variableName)
        {
            return Environment.GetEnvironmentVariable(variableName) ?? throw new ArgumentException($"Missing environment variable: {variableName}");
        }

        protected long GetEnvironmentVariableLong(string variableName)
        {
            return long.Parse(GetEnvironmentVariable(variableName));
        }
    }
}