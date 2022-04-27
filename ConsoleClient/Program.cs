using ConsoleClient.Clients;
using ConsoleClient.Options;
using ConsoleClient.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ConsoleClient
{
    class Program
    {
        private static IConfiguration Configuration { get; } = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false, true)
            .Build();

        static async Task Main(string[] args)
        {
            var host = CreateBuilder().Build();
            using var serviceScope = host.Services.CreateScope();
            var services = serviceScope.ServiceProvider;
            var logger = services.GetRequiredService<ILogger<Program>>();

            try
            {
                logger.LogDebug("Start application.");

                var workflowService = services.GetRequiredService<IWorkflowService>();
                await workflowService.DoTestTaskAsync();

                logger.LogDebug("Exit application.");
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "An error occurred.");
            }
        }

        private static IHostBuilder CreateBuilder()
        {
            var builder = new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddOptions();
                    services.Configure<ApiClientOptions>(Configuration.GetSection("ApiClient"));
                    services.Configure<UserDataOptions>(Configuration.GetSection("UserData"));
                    services.AddHttpClient<ApiClient>(s => s.BaseAddress = new Uri(Configuration["ApiClient:baseUrl"]));
                    services.AddSingleton<IApiClientFactory, ApiClientFactory>();
                    services.AddTransient<IStudentService, StudentService>();
                    services.AddTransient<IWorkflowService, WorkflowService>();
                })
                .ConfigureLogging(logging =>
                {
                    logging.AddFilter("Microsoft", LogLevel.Warning);
                    logging.AddFilter("System", LogLevel.Warning);
                    logging.AddFilter("ConsoleClient.Program", LogLevel.Debug);
                    logging.AddConsole(o => o.TimestampFormat = "[yyyy.MM.dd HH:mm:ss] ");
                    logging.AddDebug();
                    logging.SetMinimumLevel(LogLevel.Debug);
                })
                .UseConsoleLifetime();

            return builder;
        }
    }
}
