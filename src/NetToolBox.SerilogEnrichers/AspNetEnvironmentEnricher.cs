using Serilog.Core;
using Serilog.Events;
using System;
using System.Runtime.CompilerServices;

namespace NetToolBox.SerilogEnrichers
{
    public sealed class AspNetEnvironmentEnricher : ILogEventEnricher
    {
        LogEventProperty _cachedEnvironment = null!;
        LogEventProperty _cachedSiteName = null!;
        LogEventProperty _cachedSlotName = null!;
        public const string SiteName = "SiteName";
        public const string SlotName = "SlotName";
        public const string EnvironmentName = "Environment";

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddPropertyIfAbsent(GetLogEventProperty(propertyFactory, EnvironmentName));
            logEvent.AddPropertyIfAbsent(GetLogEventProperty(propertyFactory, SiteName));
            logEvent.AddPropertyIfAbsent(GetLogEventProperty(propertyFactory, SlotName));
        }
        private LogEventProperty GetLogEventProperty(ILogEventPropertyFactory propertyFactory, string propertyName)
        {
            if (propertyName == EnvironmentName)
            {
                if (_cachedEnvironment == null)
                {
                    _cachedEnvironment = CreateEnvironmentProperty(propertyFactory);
                }
                return _cachedEnvironment;
            }
            else if (propertyName == SiteName)
            {
                if (_cachedSiteName == null)
                {
                    _cachedSiteName = CreateSiteNameProperty(propertyFactory);
                }
                return _cachedSiteName;
            }
            else if (propertyName == SlotName)
            {
                if (_cachedSlotName == null)
                {
                    _cachedSlotName = CreateSlotNameProperty(propertyFactory);
                }
                return _cachedSlotName;
            }
            else
            {
                throw new InvalidOperationException("Invalid PropertyName");
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static LogEventProperty CreateSlotNameProperty(ILogEventPropertyFactory propertyFactory)
        {
            var prop = Environment.GetEnvironmentVariable("APPSETTING_WEBSITE_SLOT_NAME");
            if (prop == null)
            {
                prop = "Production";
            }
            return propertyFactory.CreateProperty(SlotName, prop);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static LogEventProperty CreateSiteNameProperty(ILogEventPropertyFactory propertyFactory)
        {
            var prop = Environment.GetEnvironmentVariable("APPSETTING_WEBSITE_SITE_NAME");
            if (prop == null)
            {
                prop = "Not Set";
            }
            return propertyFactory.CreateProperty(SiteName, prop);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static LogEventProperty CreateEnvironmentProperty(ILogEventPropertyFactory propertyFactory)
        {
            var prop = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (prop == null)
            {
                prop = "Production";
            }
            return propertyFactory.CreateProperty(EnvironmentName, prop);
        }
    }
}
