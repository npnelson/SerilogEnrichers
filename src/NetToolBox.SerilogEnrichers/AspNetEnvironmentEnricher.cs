using Serilog.Core;
using Serilog.Events;
using System;
using System.Runtime.CompilerServices;

namespace NetToolBox.SerilogEnrichers
{
    public sealed class AspNetEnvironmentEnricher : ILogEventEnricher
    {
        LogEventProperty _cachedProperty = null!;

        public const string PropertyName = "Environment";

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
            var prop = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (prop == null)
            {
                prop = "Production";
            }
            return propertyFactory.CreateProperty(PropertyName, prop);
        }
    }
}
