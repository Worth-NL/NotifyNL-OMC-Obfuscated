// © 2024, Worth Systems.

using EventsHandler.Behaviors.Mapping.Models.POCOs.NotificatieApi;

namespace EventsHandler.Behaviors.Communication.Strategy.Interfaces
{
    /// <summary>
    /// The <see cref="INotifyScenario"/>s strategies manager determining a specific workflow.
    /// </summary>
    internal interface IScenariosManager
    {
        /// <summary>
        /// Determines which workflow scenario should be used based on the delivered <see cref="NotificationEvent"/>.
        /// </summary>
        /// <param name="notification">The notification from "Notificatie API" Web service.</param>
        /// <returns>
        ///   The appropriate <see cref="INotifyScenario"/> strategy.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        ///   The following service (scenario strategy) could not be resolved.
        /// </exception>
        /// <exception cref="NotImplementedException">
        ///   The processing strategy could not be determined.
        /// </exception>
        internal Task<INotifyScenario> DetermineScenarioAsync(NotificationEvent notification);
    }
}
