﻿// © 2024, Worth Systems.

namespace EventsHandler.Services.DataLoading.Enums
{
    /// <summary>
    /// The types of DAO (Data Access Object) data providers.
    /// </summary>
    public enum LoaderTypes
    {
        /// <summary>
        /// The configuration JSON loader.
        /// </summary>
        AppSettings,

        /// <summary>
        /// The environment variables loader.
        /// </summary>
        Environment
    }
}
