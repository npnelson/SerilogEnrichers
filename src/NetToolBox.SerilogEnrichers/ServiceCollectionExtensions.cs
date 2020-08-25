using Serilog;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSeqWithCommonEnrichers<T>(this IServiceCollection serviceCollection, string seqUrl, string apiKey, string environmentName)
        {
            var logger = new LoggerConfiguration()
               .Enrich.WithClrVersion()
               .Enrich.WithOSVersion()
               .Enrich.WithProperty("Environment", environmentName)
               .Enrich.WithApplicationVersion<T>()
               .Enrich.WithMachineName()
               .WriteTo.Seq(seqUrl, apiKey: apiKey)
               .WriteTo.Console()
               .CreateLogger();
            serviceCollection.AddLogging(lb => lb.AddSerilog(logger));
            return serviceCollection;
        }
    }
}