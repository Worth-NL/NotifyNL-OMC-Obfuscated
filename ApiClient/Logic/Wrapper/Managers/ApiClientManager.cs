// © 2023, Worth Systems.

using ApiClientWrapper.Configuration;
using ApiClientWrapper.Managers.Interfaces;
using Notify.Client;  // External library from Notify UK (gov.uk)

namespace ApiClientWrapper.Managers
{
    internal sealed class ApiClientManager : IApiClientManager
    {
        private readonly ApiClientConfiguration _configuration;

        /// <inheritdoc cref="IApiClientManager.Client"/>
        NotificationClient IApiClientManager.Client => GetClient();

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiClientManager"/> class.
        /// </summary>
        public ApiClientManager(ApiClientConfiguration configuration)
        {
            this._configuration = configuration;
        }

        private NotificationClient GetClient()
        {
            return new NotificationClient(this._configuration.ApiBaseUrl, this._configuration.ApiKey);
        }
    }
}