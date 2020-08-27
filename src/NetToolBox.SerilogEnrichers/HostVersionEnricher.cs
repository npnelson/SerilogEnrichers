using Serilog.Core;
using Serilog.Events;
using System;
using System.Runtime.CompilerServices;

namespace NetToolBox.SerilogEnrichers
{
    public sealed class HostVersionEnricher : ILogEventEnricher
    {
        private LogEventProperty _cachedProperty = null!;

        public const string PropertyName = "HOST_VERSION";

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddPropertyIfAbsent(GetLogEventProperty(propertyFactory));
        }

        private LogEventProperty GetLogEventProperty(ILogEventPropertyFactory propertyFactory)
        {
            if (_cachedProperty == null)
            {
                _cachedProperty = CreateProperty(propertyFactory);
            }
            return _cachedProperty;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static LogEventProperty CreateProperty(ILogEventPropertyFactory propertyFactory)
        {
            var prop = Environment.GetEnvironmentVariable("HOST_VERSION");
            return propertyFactory.CreateProperty(PropertyName, prop);
        }
    }
}