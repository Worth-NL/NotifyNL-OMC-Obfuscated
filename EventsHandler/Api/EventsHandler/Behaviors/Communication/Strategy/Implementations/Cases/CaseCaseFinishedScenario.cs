﻿// © 2023, Worth Systems.

using EventsHandler.Behaviors.Communication.Strategy.Base;
using EventsHandler.Behaviors.Communication.Strategy.Implementations.Cases.Base;
using EventsHandler.Behaviors.Communication.Strategy.Interfaces;
using EventsHandler.Behaviors.Mapping.Models.POCOs.NotificatieApi;
using EventsHandler.Behaviors.Mapping.Models.POCOs.OpenKlant;
using EventsHandler.Behaviors.Mapping.Models.POCOs.OpenZaak;
using EventsHandler.Configuration;
using EventsHandler.Services.DataQuerying.Interfaces;

namespace EventsHandler.Behaviors.Communication.Strategy.Implementations.Cases
{
    /// <summary>
    /// <inheritdoc cref="INotifyScenario"/>
    /// The strategy for "Case finished" scenario.
    /// </summary>
    /// <seealso cref="BaseScenario"/>
    /// <seealso cref="BaseCaseScenario"/>
    internal sealed class CaseCaseFinishedScenario : BaseCaseScenario
    {
        /// <inheritdoc cref="CaseStatuses"/>
        private CaseStatuses? CachedCaseStatuses { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CaseCaseFinishedScenario"/> class.
        /// </summary>
        public CaseCaseFinishedScenario(WebApiConfiguration configuration, IDataQueryService<NotificationEvent> dataQuery)
            : base(configuration, dataQuery)
        {
        }

        #region Polymorphic (Email logic)
        /// <inheritdoc cref="BaseScenario.GetEmailTemplateId()"/>
        protected override string GetEmailTemplateId()
            => this.Configuration.User.TemplateIds.Email.ZaakClose();

        /// <inheritdoc cref="BaseScenario.GetEmailPersonalizationAsync(NotificationEvent, CommonPartyData)"/>
        protected override async Task<Dictionary<string, object>> GetEmailPersonalizationAsync(
            NotificationEvent notification, CommonPartyData partyData)
        {
            this.CachedCase ??= await this.DataQuery.From(notification).GetCaseAsync();
            this.CachedCaseStatuses ??= await this.DataQuery.From(notification).GetCaseStatusesAsync();
            this.CachedLastCaseStatusType ??= await this.DataQuery.From(notification).GetLastCaseStatusTypeAsync(this.CachedCaseStatuses);

            return new Dictionary<string, object>
            {
                { "zaak.omschrijving", this.CachedCase.Value.Name },
                { "zaak.identificatie", this.CachedCase.Value.Identification },
                { "klant.voorvoegselAchternaam", partyData.SurnamePrefix },
                { "klant.voornaam", partyData.Name },
                { "klant.achternaam", partyData.Surname },
                { "status.omschrijving", this.CachedLastCaseStatusType.Value.Description }
            };
        }
        #endregion

        #region Polymorphic (SMS logic)
        /// <inheritdoc cref="BaseScenario.GetSmsTemplateId"/>
        protected override string GetSmsTemplateId()
            => this.Configuration.User.TemplateIds.Sms.ZaakClose();

        /// <inheritdoc cref="BaseScenario.GetSmsPersonalizationAsync(NotificationEvent, CommonPartyData)"/>
        protected override async Task<Dictionary<string, object>> GetSmsPersonalizationAsync(
            NotificationEvent notification, CommonPartyData partyData)
        {
            this.CachedCase ??= await this.DataQuery.From(notification).GetCaseAsync();
            this.CachedCaseStatuses ??= await this.DataQuery.From(notification).GetCaseStatusesAsync();
            this.CachedLastCaseStatusType ??= await this.DataQuery.From(notification).GetLastCaseStatusTypeAsync(this.CachedCaseStatuses);

            return new Dictionary<string, object>
            {
                { "zaak.omschrijving", this.CachedCase.Value.Name },
                { "zaak.identificatie", this.CachedCase.Value.Identification },
                { "klant.voornaam", partyData.Name },
                { "klant.voorvoegselAchternaam", partyData.SurnamePrefix },
                { "klant.achternaam", partyData.Surname },
                { "status.omschrijving", this.CachedLastCaseStatusType.Value.Description }
            };
        }
        #endregion

        #region Polymorphic (DropCache)
        /// <inheritdoc cref="BaseCaseScenario.DropCache()"/>
        /// <remarks>
        /// <list type="bullet">
        ///   <item><see cref="CachedCaseStatuses"/></item>
        /// </list>
        /// </remarks>
        protected override void DropCache()
        {
            base.DropCache();

            this.CachedCaseStatuses = null;
            this.CachedLastCaseStatusType = null;
        }
        #endregion
    }
}