// © 2023, Worth Systems.

namespace EventsHandler.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="IServiceCollection"/>.
    /// </summary>
    internal static class ServiceCollectionExtensions
    {
        /// <inheritdoc cref="ServiceProviderServiceExtensions.GetRequiredService{T}(IServiceProvider)"/>
        internal static TService GetRequiredService<TService>(this IServiceCollection services)
            where TService : class
        {
            return services.BuildServiceProvider().GetRequiredService<TService>();
        }
    }
}