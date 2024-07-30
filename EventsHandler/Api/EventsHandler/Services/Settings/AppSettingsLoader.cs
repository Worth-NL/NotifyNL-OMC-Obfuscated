﻿// © 2024, Worth Systems.

using EventsHandler.Properties;
using EventsHandler.Services.Settings.Interfaces;

namespace EventsHandler.Services.Settings
{
    // ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
    /// <inheritdoc cref="ILoadingService"/>
    /// <remarks>
    ///   This data provider is using "appsettings.json" configuration file.
    /// </remarks>
    internal class AppSettingsLoader : ILoadingService
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppSettingsLoader"/> class.
        /// </summary>
        public AppSettingsLoader(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        #region Polymorphism
        /// <inheritdoc cref="ILoadingService.GetData{T}(string)"/>
        /// <exception cref="ArgumentException"/>
        public virtual TData GetData<TData>(string key)
            where TData : notnull
        {
            // The key is missing
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new KeyNotFoundException(string.Format(Resources.Configuration_ERROR_ValueNotFoundOrEmpty, key));
            }

            return this._configuration.GetValue<TData>(key)
                // The value is null
                ?? throw new KeyNotFoundException(string.Format(Resources.Configuration_ERROR_ValueNotFoundOrEmpty, key));
        }

        /// <inheritdoc cref="ILoadingService.GetPathWithNode(string, string)"/>
        public virtual string GetPathWithNode(string currentPath, string nodeName)
        {
            if (currentPath == "AppSettings")  // Skip "AppSettings" as part of the configuration path
            {
                return nodeName;
            }

            return $"{currentPath}{(string.IsNullOrWhiteSpace(nodeName)
                ? string.Empty
                : GetNodePath(nodeName))}";
        }

        /// <inheritdoc cref="ILoadingService.GetNodePath(string)"/>
        public virtual string GetNodePath(string nodeName)
        {
            const string separator = ":";

            return $"{separator}{nodeName}";
        }
        #endregion
    }
}