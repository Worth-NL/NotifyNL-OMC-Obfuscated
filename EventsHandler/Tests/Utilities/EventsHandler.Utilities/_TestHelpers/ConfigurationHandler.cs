// © 2023, Worth Systems.

using EventsHandler.Services.DataLoading;
using Microsoft.Extensions.Configuration;

namespace EventsHandler.Utilities._TestHelpers
{
    /// <summary>
    /// The configuration handler used for test project.
    /// </summary>
    internal static class ConfigurationHandler
    {
        /// <summary>
        /// Gets the test <see cref="IConfiguration"/>.
        /// </summary>
        internal static IConfiguration GetConfiguration()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.Test.json")
                .AddEnvironmentVariables()
                .Build();

            return configuration;
        }

        /// <summary>
        /// Gets the test <see cref="ConfigurationLoader"/>.
        /// </summary>
        internal static ConfigurationLoader GetConfigurationLoader()
        {
            return new ConfigurationLoader(GetConfiguration());
        }
    }
}