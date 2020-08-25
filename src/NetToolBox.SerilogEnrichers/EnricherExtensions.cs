using NetToolBox.SerilogEnrichers;
using Serilog.Configuration;

namespace Serilog
{
    public static class EnricherExtensions
    {
        /// <summary>
        /// Adds Application Version to the log
        /// </summary>
        /// <typeparam name="T">Any type from the assembly that you want the version stamp from</typeparam>
        /// <param name="enrich"></param>
        /// <returns></returns>
        public static LoggerConfiguration WithApplicationVersion<T>(this LoggerEnrichmentConfiguration enrich)
        {
            return enrich.With<ApplicationVersionEnricher<T>>();
        }

        /// <summary>
        /// Adds the Runtime Version to the log
        /// </summary>
        /// <param name="enrich"></param>
        /// <returns></returns>
        public static LoggerConfiguration WithClrVersion(this LoggerEnrichmentConfiguration enrich)
        {
            return enrich.With<ClrVersionEnricher>();
        }

        /// <summary>
        /// Adds the OSVersion to the log
        /// </summary>
        /// <param name="enrich"></param>
        /// <returns></returns>
        public static LoggerConfiguration WithOSVersion(this LoggerEnrichmentConfiguration enrich)
        {
            return enrich.With<OSVersionEnricher>();
        }
    }
}