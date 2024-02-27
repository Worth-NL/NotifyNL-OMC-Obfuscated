// © 2024, Worth Systems.

using EventsHandler.Properties;
using EventsHandler.Services.DataLoading.Interfaces;

namespace EventsHandler.Services.DataLoading
{
    /// <inheritdoc cref="ILoadingService"/>
    internal sealed class EnvironmentLoader : ILoadingService
    {
        /// <inheritdoc cref="ILoadingService.GetData{T}(string)"/>
        /// <exception cref="NotImplementedException">The operating system (OS) is not supported.</exception>
        TData ILoadingService.GetData<TData>(string key)
        {
            // The key is missing
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new KeyNotFoundException(Resources.Configuration_ERROR_EnvironmentVariableGetNull);
            }

            // Support for modern Windows OS (e.g., Windows XP, Vista, 7, 8+, 10, 11)
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                object value = Environment.GetEnvironmentVariable(key)
                    ?? throw new KeyNotFoundException(Resources.Configuration_ERROR_EnvironmentVariableGetNull);

                return (TData)Convert.ChangeType(value, typeof(TData));
            }

            throw new NotImplementedException(Resources.Configuration_ERROR_EnvironmentNotSupported);
        }
    }
}
