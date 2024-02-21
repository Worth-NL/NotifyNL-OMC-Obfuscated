// © 2023, Worth Systems.

using EventsHandler.Constants;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;

namespace EventsHandler.Services.Telemetry
{
    /// <inheritdoc cref="ITelemetryInitializer"/>
    internal sealed class AzureTelemetryService : ITelemetryInitializer
    {
        /// <inheritdoc cref="ITelemetryInitializer.Initialize(ITelemetry)"/>
        public void Initialize(ITelemetry telemetry)
        {
            if (string.IsNullOrEmpty(telemetry.Context.Cloud.RoleName))
            {
                telemetry.Context.Cloud.RoleName = DefaultValues.Logging.CloudRoleName;
            }
        }
    }
}