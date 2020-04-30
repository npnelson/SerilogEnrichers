using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSeqWithCommonEnrichers<T>(this IServiceCollection serviceCollection, string seqUrl, string apiKey)
        {
            var logger = new LoggerConfiguration()
               .Enrich.WithClrVersion()
               .Enrich.WithOSVersion()
               .Enrich.WithAspNetEnvironment()
               .Enrich.WithApplicationVersion<T>()
               .WriteTo.Seq(seqUrl, apiKey: apiKey)
               .CreateLogger();
            serviceCollection.AddLogging(lb => lb.AddSerilog(logger));
            return serviceCollection;
        }
    }
}
