﻿// © 2024, Worth Systems.

using EventsHandler.Behaviors.Communication.Strategy.Base;
using EventsHandler.Behaviors.Communication.Strategy.Interfaces;
using EventsHandler.Behaviors.Communication.Strategy.Models.DTOs;
using EventsHandler.Behaviors.Mapping.Enums.Objecten;
using EventsHandler.Behaviors.Mapping.Models.POCOs.NotificatieApi;
using EventsHandler.Behaviors.Mapping.Models.POCOs.Objecten;
using EventsHandler.Behaviors.Mapping.Models.POCOs.OpenKlant;
using EventsHandler.Behaviors.Mapping.Models.POCOs.OpenZaak;
using EventsHandler.Configuration;
using EventsHandler.Constants;
using EventsHandler.Exceptions;
using EventsHandler.Properties;
using EventsHandler.Services.DataQuerying.Interfaces;
using System.Globalization;

namespace EventsHandler.Behaviors.Communication.Strategy.Implementations
{
    /// <summary>
    /// <inheritdoc cref="INotifyScenario"/>
    /// The strategy for "Task assigned" scenario.
    /// </summary>
    /// <seealso cref="BaseScenario"/>
    internal sealed class TaskAssignedScenario : BaseScenario
    {
        /// <inheritdoc cref="Data"/>
        private Data? CachedTaskData { get; set; }

        /// <inheritdoc cref="Case"/>
        private Case? CachedCase { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskAssignedScenario"/> class.
        /// </summary>
        public TaskAssignedScenario(WebApiConfiguration configuration, IDataQueryService<NotificationEvent> dataQuery)
            : base(configuration, dataQuery)
        {
        }

        #region Polymorphic (GetAllNotifyDataAsync)
        /// <inheritdoc cref="BaseScenario.GetAllNotifyDataAsync(NotificationEvent)"/>
        internal override async Task<NotifyData[]> GetAllNotifyDataAsync(NotificationEvent notification)
        {
            this.QueryContext ??= this.DataQuery.From(notification);

            // Validation #1: The task needs to be of a specific type
            if (!this.QueryContext.IsValidType())
            {
                throw new AbortedNotifyingException(Resources.Processing_ABORT_DoNotSendNotification_TaskType);
            }

            this.CachedTaskData ??= (await this.QueryContext.GetTaskAsync()).Record.Data;

            // Validation #2: The task needs to have an open status
            if (this.CachedTaskData.Value.Status != TaskStatuses.Open)
            {
                throw new AbortedNotifyingException(Resources.Processing_ABORT_DoNotSendNotification_TaskClosed);
            }

            // Validation #3: The task needs to be assigned to a person
            if (this.CachedTaskData.Value.Identification.Type != IdTypes.Bsn)
            {
                throw new AbortedNotifyingException(Resources.Processing_ABORT_DoNotSendNotification_TaskNotPerson);
            }

            this.CachedCommonPartyData ??=
                await this.QueryContext.GetPartyDataAsync(this.CachedTaskData.Value.Identification.Value);

            return await base.GetAllNotifyDataAsync(notification);
        }
        #endregion
        
        #region Polymorphic (Email logic)
        /// <inheritdoc cref="BaseScenario.GetEmailTemplateId()"/>
        protected override string GetEmailTemplateId()
            => this.Configuration.User.TemplateIds.Email.TaskAssigned();

        /// <inheritdoc cref="BaseScenario.GetEmailPersonalizationAsync(CommonPartyData)"/>
        protected override async Task<Dictionary<string, object>> GetEmailPersonalizationAsync(CommonPartyData partyData)
        {
            this.CachedCase ??= await this.QueryContext!.GetCaseAsync(this.CachedTaskData!.Value.CaseUrl);

            DateTime expirationDate = this.CachedTaskData!.Value.ExpirationDate;

            string formattedExpirationDate;
            string isExpirationDateProvided;

            if (expirationDate == default)
            {
                formattedExpirationDate = DefaultValues.Models.DefaultEnumValueName;
                isExpirationDateProvided = "no";
            }
            else
            {
                // Convert time zone from UTC to CET (if necessary)
                if (expirationDate.Kind == DateTimeKind.Utc)
                {
                    var cetTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
                    expirationDate = TimeZoneInfo.ConvertTimeFromUtc(expirationDate, cetTimeZone);
                }

                // Formatting the date and time
                formattedExpirationDate = expirationDate.ToString("f", new CultureInfo("nl-NL"));
                isExpirationDateProvided = "yes";
            }

            return new Dictionary<string, object>
            {
                { "taak.verloopdatum", formattedExpirationDate },
                { "taak.heeft_verloopdatum", isExpirationDateProvided },
                { "taak.record.data.title", this.CachedTaskData!.Value.Title },
                { "zaak.omschrijving", this.CachedCase.Value.Name },
                { "zaak.identificatie", this.CachedCase.Value.Identification }
            };
        }
        #endregion

        #region Polymorphic (SMS logic)
        /// <inheritdoc cref="BaseScenario.GetSmsTemplateId()"/>
        protected override string GetSmsTemplateId()
            => this.Configuration.User.TemplateIds.Sms.TaskAssigned();

        /// <inheritdoc cref="BaseScenario.GetSmsPersonalizationAsync(CommonPartyData)"/>
        protected override async Task<Dictionary<string, object>> GetSmsPersonalizationAsync(CommonPartyData partyData)
        {
            return await GetEmailPersonalizationAsync(partyData);  // NOTE: Both implementations are identical
        }
        #endregion

        #region Polymorphic (DropCache)
        /// <inheritdoc cref="BaseScenario.DropCache()"/>
        /// <remarks>
        /// <list type="bullet">
        ///   <item><see cref="CachedTaskData"/></item>
        ///   <item><see cref="CachedCase"/></item>
        /// </list>
        /// </remarks>
        protected override void DropCache()
        {
            base.DropCache();

            this.CachedTaskData = null;
            this.CachedCase = null;
        }
        #endregion
    }
}