using Microsoft.Extensions.DependencyInjection;
using System;

namespace ConsoleClient.Clients
{
    public class ApiClientFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public ApiClientFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public ApiClient Create()
        {
            return _serviceProvider.GetRequiredService<ApiClient>();
        }
    }
}
