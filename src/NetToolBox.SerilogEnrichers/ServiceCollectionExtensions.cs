using Serilog;
using Serilog.Events;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSeqWithCommonEnrichers<T>(this IServiceCollection serviceCollection, string seqUrl, string apiKey, string environmentName, bool includeEFInformationLogging, bool includeAzureLogging)
        {
            var loggerConfiguration = new LoggerConfiguration()
               .MinimumLevel.Debug()
               .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
               .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning);

            if (!includeEFInformationLogging)
            {
                loggerConfiguration.MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning);
            }
            if (!includeAzureLogging)
            {
                loggerConfiguration.MinimumLevel.Override("Azure-Core", LogEventLevel.Warning) //the Azure-Identity,Azure-Core, and Azure-Messaging-ServiceBus sourcecontexts can be quite noisy and rarely useful
            .MinimumLevel.Override("Azure-Identity", LogEventLevel.Warning)
            .MinimumLevel.Override("Azure.Identity", LogEventLevel.Warning)
            .MinimumLevel.Override("Azure.Core", LogEventLevel.Warning)
            .MinimumLevel.Override("Azure.Messaging.ServiceBus", LogEventLevel.Warning)
            .MinimumLevel.Override("Azure-Messaging-ServiceBus", LogEventLevel.Warning);
            }
            loggerConfiguration.Enrich.WithClrVersion()
        .Enrich.WithOSVersion()
        .Enrich.WithProperty("Environment", environmentName)
        .Enrich.WithApplicationVersion<T>()
        .Enrich.WithAzureFunctionHostVersion()
        .Enrich.WithMachineName()
        .WriteTo.Seq(seqUrl, apiKey: apiKey)
        .WriteTo.Console();

            var logger = loggerConfiguration.CreateLogger();
            serviceCollection.AddLogging(lb => lb.AddSerilog(logger));
            return serviceCollection;
        }
    }
}