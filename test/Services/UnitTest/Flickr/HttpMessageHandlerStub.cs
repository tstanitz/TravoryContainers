using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace UnitTest.Flickr
{
    public class HttpMessageHandlerStub : HttpMessageHandler
    {
        public string Content { get; set; }
        public HttpStatusCode HttpStatusCode => HttpStatusCode.OK;

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new HttpResponseMessage
            {
                Content = new StringContent(Content),
                StatusCode = HttpStatusCode
            });
        }
    }
}