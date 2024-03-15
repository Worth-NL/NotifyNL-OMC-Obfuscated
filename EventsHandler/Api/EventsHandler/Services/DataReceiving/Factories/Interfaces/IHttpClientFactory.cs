﻿// © 2023, Worth Systems.

namespace EventsHandler.Services.DataReceiving.Factories.Interfaces
{
    /// <summary>
    /// The service to return a specific instance of <typeparamref name="THttpClient"/>.
    /// </summary>
    public interface IHttpClientFactory<out THttpClient, in TParameters>
        where THttpClient : class
        where TParameters : notnull
    {
        // TODO: Caching IHttpClient on this level

        /// <summary>
        /// Gets the <typeparamref name="THttpClient"/> with customized parameters (e.g. Headers).
        /// </summary>
        /// <param name="parameters">
        ///   The parameters to configure a requested <typeparamref name="THttpClient"/>.
        /// </param>
        /// <returns>
        ///   The customized <typeparamref name="THttpClient"/>.
        /// </returns>
        internal THttpClient GetHttpClient(TParameters parameters);
    }
}