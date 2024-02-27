// © 2024, Worth Systems.

using EventsHandler.Extensions;
using EventsHandler.Properties;
using EventsHandler.Services.DataLoading.Enums;
using EventsHandler.Services.DataLoading.Interfaces;
using EventsHandler.Services.DataLoading.Strategy.Interfaces;

namespace EventsHandler.Services.DataLoading.Strategy.Manager
{
    /// <inheritdoc cref="ILoadersContext"/>
    public sealed class LoadersContext : ILoadersContext, ILoadingService
    {
        private readonly IServiceCollection _services;

        private ILoadingService? _loadingService;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoadersContext"/> class.
        /// </summary>
        public LoadersContext(IServiceCollection services)
        {
            this._services = services;
        }

        #region ILoadersContext
        /// <inheritdoc cref="ILoadersContext.SetLoader(LoaderTypes)"/>
        void ILoadersContext.SetLoader(LoaderTypes loaderType)
        {
            this._loadingService = loaderType switch
            {
                // Reading configurations from "appsettings.json" (Dev, Test, Prod, and the fallback general file)
                LoaderTypes.Configuration => this._services.GetRequiredService<ConfigurationLoader>(),

                // Reading configurations from the preset environment variables (e.g. in Windows, Linus, macOS)
                LoaderTypes.Environment => this._services.GetRequiredService<EnvironmentLoader>(),

                _ => throw new NotImplementedException(Resources.Processing_ERROR_Loader_NotImplemented)
            };
        }
        #endregion

        #region ILoadingService
        /// <inheritdoc cref="ILoadingService.GetData{TData}(string)"/>
        TData ILoadingService.GetData<TData>(string key)
        {
            if (this._loadingService == null)
            {
                throw new NotImplementedException(Resources.Processing_ERROR_Loader_NotImplemented);
            }

            return this._loadingService.GetData<TData>(key);
        }
        #endregion
    }
}
