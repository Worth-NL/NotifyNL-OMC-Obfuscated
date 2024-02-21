// © 2024, Worth Systems.

using EventsHandler.Services.DataLoading.Interfaces;
using EventsHandler.Services.DataLoading.Strategy.Interfaces;

namespace EventsHandler.Services.DataLoading.Strategy.Manager
{
    /// <inheritdoc cref="ILoadersContext"/>
    public sealed class LoadersContext : ILoadersContext
    {
        private ILoadingService _loadingService;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoadersContext"/> class.
        /// </summary>
        internal LoadersContext(ILoadingService loadingService)  // NOTE: Used to set up context directly from constructor
        {
            this._loadingService = loadingService;
        }

        /// <inheritdoc cref="ILoadersContext.SetLoader(ILoadingService)"/>
        void ILoadersContext.SetLoader(ILoadingService loadingService)
        {
            this._loadingService = loadingService;
        }

        T ILoadingService.GetData<T>(string key)
        {
            return this._loadingService.GetData<T>(key);
        }
    }
}
