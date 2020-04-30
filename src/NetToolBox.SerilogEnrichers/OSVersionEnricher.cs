using Serilog.Core;
using Serilog.Events;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NetToolBox.SerilogEnrichers
{
    public sealed class OSVersionEnricher : ILogEventEnricher
    {

        LogEventProperty _cachedOSProperty = null!;
        LogEventProperty _cachedLinuxProperty = null!;

        public const string OSPropertyName = "OSVersion";
        public const string LinuxVersionPropertyName = "LinuxVersion";

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddPropertyIfAbsent(GetLogEventProperty(propertyFactory, OSPropertyName));
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                logEvent.AddPropertyIfAbsent(GetLogEventProperty(propertyFactory, LinuxVersionPropertyName));
            }
        }
        private LogEventProperty GetLogEventProperty(ILogEventPropertyFactory propertyFactory, string propertyName)
        {
            if (propertyName == OSPropertyName)
            {
                if (_cachedOSProperty == null)
                {
                    _cachedOSProperty = CreateOSProperty(propertyFactory);
                }
                return _cachedOSProperty;
            }
            else if (propertyName == LinuxVersionPropertyName)
            {
                if (_cachedLinuxProperty == null)
                {
                    _cachedLinuxProperty = CreateLinuxVersionProperty(propertyFactory);
                }
                return _cachedLinuxProperty;
            }
            else
            {
                throw new InvalidOperationException("Invalid PropertyName");
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static LogEventProperty CreateLinuxVersionProperty(ILogEventPropertyFactory propertyFactory)
        {
            return propertyFactory.CreateProperty(LinuxVersionPropertyName, NETToolBox.LinuxVersion.GetLinuxVersion.GetLinuxVersionInfo().VersionString);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static LogEventProperty CreateOSProperty(ILogEventPropertyFactory propertyFactory)
        {
            return propertyFactory.CreateProperty(OSPropertyName, RuntimeInformation.OSDescription);
        }
    }
}
