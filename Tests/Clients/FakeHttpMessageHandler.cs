using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Tests.Clients
{
    public class FakeHttpMessageHandler : HttpMessageHandler
    {
        public virtual async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            return await Task.FromResult<HttpResponseMessage>(null);
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return await SendAsync(request);
        }
    }
}
