// © 2023, Worth Systems.

using Microsoft.Extensions.Configuration;

namespace ApiClientWrapper.Configuration
{
    internal sealed class ApiClientConfiguration
    {
        private readonly IConfiguration _configuration;
        private readonly IConfigurationRoot _secrets;

        /// <summary>
        /// Gets the API service base URL.
        /// </summary>
        internal string ApiBaseUrl => this._configuration.GetValue<string>("ApiBaseUrl") ?? string.Empty;

        /// <summary>
        /// Gets the API key.
        /// <para>
        /// Resources: https://documentation.notification.canada.ca/en/start.html#base-url
        /// </para>
        /// <code>
        /// Key structure:   [  name  ] [          ISS: Service ID         ] [            Secret Key            ]
        ///                 "someString-00000000-0000-0000-0000-000000000000-00000000-0000-0000-0000-000000000000";
        /// </code>
        /// </summary>
        internal string ApiKey => this._secrets.GetValue<string>($"Authorization:ApiKey:{GetEnvironment()}") ?? string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiClientConfiguration"/> class.
        /// </summary>
        public ApiClientConfiguration(IConfiguration configuration)
        {
            // Mapping public configurations from "appsettings.json"
            this._configuration = configuration;

            // Mapping private configurations from "secrets.json"
            this._secrets = GetSecrets();
        }

        private static IConfigurationRoot GetSecrets()
        {
            return new ConfigurationBuilder().AddUserSecrets<ApiClientConfiguration>().Build();
        }

        /// <summary>
        /// Determines the runtime environment (Debug or Release).
        /// </summary>
        /// <returns>
        /// Key that indicates which environment is being used.
        /// </returns>
        private static string GetEnvironment()
        {
            #if DEBUG
            return "Test";  // Debug mode
            #else
            return "Prod";  // Release mode
            #endif
        }
    }
}