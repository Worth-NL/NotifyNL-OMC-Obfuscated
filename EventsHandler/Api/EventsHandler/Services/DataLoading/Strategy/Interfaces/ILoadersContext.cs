// © 2024, Worth Systems.

using EventsHandler.Services.DataLoading.Enums;
using EventsHandler.Services.DataLoading.Interfaces;

namespace EventsHandler.Services.DataLoading.Strategy.Interfaces
{
    /// <summary>
    /// The strategy context for <see cref="ILoadingService"/> strategies - acting like a facade for a specific Data Access Object (DAO) providers.
    /// </summary>
    public interface ILoadersContext
    {
        /// <summary>
        /// Sets the specific DAO (Data Access Object) data provider / aka. "Loader".
        /// </summary>
        /// <param name="loaderType">The specific loader to be set.</param>
        internal void SetLoader(LoaderTypes loaderType);
    }
}
