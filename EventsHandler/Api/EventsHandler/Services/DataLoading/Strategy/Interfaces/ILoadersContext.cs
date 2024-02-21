// © 2024, Worth Systems.

using EventsHandler.Services.DataLoading.Interfaces;

namespace EventsHandler.Services.DataLoading.Strategy.Interfaces
{
    /// <summary>
    /// The <see cref="ILoadingService"/>s strategies context behaving like a facade for a specific Data Access Object (DAO).
    /// </summary>
    public interface ILoadersContext : ILoadingService
    {
        /// <summary>
        /// Sets the specific loader.
        /// </summary>
        /// <param name="loadingService">The specific loader to be set.</param>
        internal void SetLoader(ILoadingService loadingService);
    }
}
