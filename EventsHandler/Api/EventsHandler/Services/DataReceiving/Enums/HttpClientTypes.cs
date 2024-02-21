// © 2023, Worth Systems.

namespace EventsHandler.Services.DataReceiving.Enums
{
    /// <summary>
    /// A specific types of <see cref="HttpClient"/>s used by internal business logic.
    /// </summary>
    internal enum HttpClientTypes
    {
        /// <summary>
        /// The <see cref="HttpClient"/> used to obtain data from external API services.
        /// </summary>
        Data = 0,

        /// <summary>
        /// The <see cref="HttpClient"/> used for feedback and telemetry purposes.
        /// </summary>
        Telemetry = 1
    }
}