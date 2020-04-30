using Serilog.Core;
using Serilog.Events;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace NetToolBox.SerilogEnrichers
{
    public sealed class ApplicationVersionEnricher<T> : ILogEventEnricher
    {
        LogEventProperty _cachedProperty = null!;

        public const string PropertyName = "ApplicationVersion";

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
            return propertyFactory.CreateProperty(PropertyName, typeof(T).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion);
        }
    }
}
