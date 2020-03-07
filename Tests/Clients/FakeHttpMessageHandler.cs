using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Tests.Clients
{
    public class FakeHttpMessageHandler : HttpMessageHandler
    {
        public virtual async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            throw new NotImplementedException("We can setup this method in the unit tests since its virtual");
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return await SendAsync(request);
        }
    }
}
