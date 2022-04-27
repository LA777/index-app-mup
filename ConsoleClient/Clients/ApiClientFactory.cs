using Microsoft.Extensions.DependencyInjection;
using System;

namespace ConsoleClient.Clients
{
    public class ApiClientFactory : IApiClientFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public ApiClientFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IApiClient Create()
        {
            return _serviceProvider.GetRequiredService<ApiClient>();
        }
    }
}
