// © 2023, Worth Systems.

using EventsHandler.Behaviors.Communication.Strategy.Base;
using EventsHandler.Behaviors.Communication.Strategy.Interfaces;
using EventsHandler.Behaviors.Mapping.Enums.NotificatieApi;
using EventsHandler.Behaviors.Mapping.Models.POCOs.NotificatieApi;
using EventsHandler.Behaviors.Mapping.Models.POCOs.OpenZaak;
using EventsHandler.Services.DataQuerying.Interfaces;

namespace EventsHandler.Behaviors.Communication.Strategy.Manager
{
    /// <inheritdoc cref="IScenariosManager"/>
    public sealed class ScenariosManager : IScenariosManager
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IDataQueryService<NotificationEvent> _dataQuery;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScenariosManager"/> nested class.
        /// </summary>
        public ScenariosManager(IServiceProvider serviceScope, IDataQueryService<NotificationEvent> dataQuery)
        {
            this._serviceProvider = serviceScope;
            this._dataQuery = dataQuery;
        }

        /// <inheritdoc cref="IScenariosManager.DetermineScenarioAsync(NotificationEvent)"/>
        async Task<INotifyScenario> IScenariosManager.DetermineScenarioAsync(NotificationEvent notification)
        {
            // Scenarios for Cases
            if (notification.Action == Actions.Create &&
                notification.Channel == Channels.Cases &&
                notification.Resource == Resources.Status)
            {
                CaseStatuses caseStatuses = await this._dataQuery.From(notification).GetCaseStatusesAsync();

                // Scenario #1: "Case created"
                if (caseStatuses.WereNeverUpdated())
                {
                    return this._serviceProvider.GetRequiredService<CaseCreatedScenario>();
                }

                CaseStatusType lastCaseStatusType =
                    await this._dataQuery.From(notification).GetLastCaseStatusTypeAsync(caseStatuses);

                BaseStatusScenario strategy = !lastCaseStatusType.IsFinalStatus
                    // Scenario #2: "Case status updated"
                    ? this._serviceProvider.GetRequiredService<CaseStatusUpdatedScenario>()
                    // Scenario #3: "Case finished"
                    : this._serviceProvider.GetRequiredService<CaseFinishedScenario>();

                strategy.PassAlreadyQueriedResult(lastCaseStatusType);

                return strategy;
            }

            // There is no matching scenario to be applied
            return this._serviceProvider.GetRequiredService<NotImplementedScenario>();
        }
    }
}