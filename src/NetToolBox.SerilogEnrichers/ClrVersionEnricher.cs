using Serilog.Core;
using Serilog.Events;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NetToolBox.SerilogEnrichers
{
    public sealed class ClrVersionEnricher : ILogEventEnricher
    {
        LogEventProperty _cachedProperty = null!;

        public const string PropertyName = "ClrVersion";

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
            return propertyFactory.CreateProperty(PropertyName, RuntimeInformation.FrameworkDescription);
        }
    }
}
