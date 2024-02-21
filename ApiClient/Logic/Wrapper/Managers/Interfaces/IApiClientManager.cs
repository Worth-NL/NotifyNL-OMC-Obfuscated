// © 2023, Worth Systems.

using Notify.Client;  // External library from Notify UK (gov.uk)

namespace ApiClientWrapper.Managers.Interfaces
{
    /// <summary>
    /// Manages Notify NL API client.
    /// </summary>
    internal interface IApiClientManager
    {
        /// <summary>
        /// Gets the pre-configured API <see cref="HttpClient"/> using specific <see cref="Uri"/> and API access token.
        /// </summary>
        internal NotificationClient Client { get; }
    }
}