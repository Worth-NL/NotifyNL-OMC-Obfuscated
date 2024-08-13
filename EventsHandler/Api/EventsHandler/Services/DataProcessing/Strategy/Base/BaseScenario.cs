﻿// © 2023, Worth Systems.

using EventsHandler.Exceptions;
using EventsHandler.Mapping.Enums.OpenKlant;
using EventsHandler.Mapping.Models.POCOs.NotificatieApi;
using EventsHandler.Mapping.Models.POCOs.OpenKlant;
using EventsHandler.Services.DataProcessing.Enums;
using EventsHandler.Services.DataProcessing.Strategy.Interfaces;
using EventsHandler.Services.DataProcessing.Strategy.Models.DTOs;
using EventsHandler.Services.DataQuerying.Interfaces;
using EventsHandler.Services.Settings.Configuration;
using System.Text.Json;
using Resources = EventsHandler.Properties.Resources;

namespace EventsHandler.Services.DataProcessing.Strategy.Base
{
    /// <summary>
    /// The intermediate layer between concrete scenario strategies and their <see cref="INotifyScenario"/> interface.
    /// <para>
    ///   This <see langword="abstract class"/> applies to all strategies, and it's defining the
    ///   mandatory strategies methods (which should not be visible outside on the other hand).
    /// </para>
    /// </summary>
    /// <seealso cref="INotifyScenario"/>
    internal abstract class BaseScenario : INotifyScenario
    {
        /// <inheritdoc cref="WebApiConfiguration"/>
        protected WebApiConfiguration Configuration { get; }

        /// <inheritdoc cref="IDataQueryService{TModel}"/>
        protected IDataQueryService<NotificationEvent> DataQuery { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseScenario"/> class.
        /// </summary>
        protected BaseScenario(WebApiConfiguration configuration, IDataQueryService<NotificationEvent> dataQuery)
        {
            this.Configuration = configuration;
            this.DataQuery = dataQuery;
        }

        #region Parent (GetAllNotifyDataAsync)
        /// <inheritdoc cref="INotifyScenario.GetAllNotifyDataAsync(NotificationEvent)"/>
        async Task<NotifyData[]> INotifyScenario.GetAllNotifyDataAsync(NotificationEvent notification)
        {
            CommonPartyData commonPartyData = await PrepareDataAsync(notification);

            // Determine which types of notifications should be published
            return commonPartyData.DistributionChannel switch
            {
                DistributionChannels.Email => new[] { GetEmailNotifyData(commonPartyData) },

                DistributionChannels.Sms   => new[] { GetSmsNotifyData(commonPartyData) },

                // NOTE: Older version of "OpenKlant" was supporting option for many types of notifications
                DistributionChannels.Both  => new[] { GetEmailNotifyData(commonPartyData),
                                                      GetSmsNotifyData(commonPartyData) },

                DistributionChannels.None  => Array.Empty<NotifyData>(),

                // NOTE: Notification method cannot be unknown or undefined. Fill the data properly in "OpenKlant"
                DistributionChannels.Unknown
                  => throw new InvalidOperationException(Resources.Processing_ERROR_Notification_DeliveryMethodUnknown),
                _ => throw new InvalidOperationException(Resources.Processing_ERROR_Notification_DeliveryMethodUnknown)
            };
        }
        #endregion

        #region Parent (Validation)
        /// <summary>
        /// Validates whether the case identifier is whitelisted in <see cref="WebApiConfiguration"/> settings.
        /// </summary>
        /// <param name="isCaseIdWhitelistedValidation">The scenario-specific validation delegate to be invoked.</param>
        /// <param name="caseId">The case identifier to be checked.</param>
        /// <param name="scenarioName">The name of the scenario used for logging.</param>
        /// <exception cref="AbortedNotifyingException"/>
        protected static void ValidateCaseId(Predicate<string> isCaseIdWhitelistedValidation, string caseId, string scenarioName)
        {
            if (isCaseIdWhitelistedValidation.Invoke(caseId))
            {
                return;
            }

            throw new AbortedNotifyingException(string.Format(Resources.Processing_ABORT_DoNotSendNotification_CaseIdWhitelisted, caseId, scenarioName));
        }
        
        /// <summary>
        /// Validates whether the inform field is set to false.
        /// </summary>
        /// <param name="isNotificationExpected">The value of the inform field.</param>
        /// <exception cref="AbortedNotifyingException"/>
        protected static void ValidateNotifyPermit(bool isNotificationExpected)
        {
            if (!isNotificationExpected)
            {
                throw new AbortedNotifyingException(Resources.Processing_ABORT_DoNotSendNotification_Informeren);
            }
        }
        #endregion

        #region Virtual (Email logic)
        /// <summary>
        /// Gets the e-mail notify data to be used with "Notify NL" API Client.
        /// </summary>
        /// <param name="partyData">The data associated to a specific party (e.g., citizen, organization).</param>
        /// <returns>
        ///   The e-mail data for "Notify NL" Web API service.
        /// </returns>
        protected virtual NotifyData GetEmailNotifyData(CommonPartyData partyData)
        {
            return new NotifyData
            (
                notificationMethod: NotifyMethods.Email,
                contactDetails: partyData.EmailAddress,
                templateId: GetEmailTemplateId(),
                personalization: GetEmailPersonalization(partyData)
            );
        }
        #endregion

        #region Virtual (SMS logic)
        /// <summary>
        /// Gets the SMS notify data to be used with "Notify NL" API Client.
        /// </summary>
        /// <param name="partyData">The data associated to a specific party (e.g., citizen, organization).</param>
        /// <returns>
        ///   The SMS data for "Notify NL" Web API service.
        /// </returns>
        protected virtual NotifyData GetSmsNotifyData(CommonPartyData partyData)
        {
            return new NotifyData
            (
                notificationMethod: NotifyMethods.Sms,
                contactDetails: partyData.TelephoneNumber,
                templateId: GetSmsTemplateId(),
                personalization: GetSmsPersonalization(partyData)
            );
        }
        #endregion

        #region Abstract (PrepareData)        
        /// <summary>
        /// Prepares all the data required by this specific scenario.
        /// </summary>
        /// <param name="notification">The notification from "OpenNotificaties" Web API service.</param>
        /// <returns>
        ///   The data containing basic information required to send the notification.
        /// </returns>
        /// <exception cref="KeyNotFoundException"/>
        /// <exception cref="HttpRequestException"/>
        /// <exception cref="JsonException"/>
        /// <exception cref="AbortedNotifyingException"/>
        protected abstract Task<CommonPartyData> PrepareDataAsync(NotificationEvent notification);
        #endregion

        #region Abstract (Email logic)
        /// <summary>
        /// Gets the e-mail template ID for this strategy.
        /// </summary>
        /// <returns>
        ///   The template ID from "Notify NL" Web API service in format "00000000-0000-0000-0000-00000000" (UUID).
        /// </returns>
        protected abstract Guid GetEmailTemplateId();

        /// <summary>
        /// Gets the e-mail "personalization" for this strategy.
        /// </summary>
        /// <param name="partyData">The data associated to a specific party (e.g., citizen, organization).</param>
        /// <returns>
        ///   The dictionary of &lt;placeholder, value&gt; used for personalization of "Notify NL" Web API service notification.
        /// </returns>
        protected abstract Dictionary<string, object> GetEmailPersonalization(CommonPartyData partyData);
        #endregion

        #region Abstract (SMS logic)
        /// <summary>
        /// Gets the SMS template ID for this strategy.
        /// </summary>
        /// <returns>
        ///   The template ID from "Notify NL" Web API service in format "00000000-0000-0000-0000-00000000".
        /// </returns>
        protected abstract Guid GetSmsTemplateId();

        /// <summary>
        /// Gets the SMS "personalization" for this strategy.
        /// </summary>
        /// <param name="partyData">The data associated to a specific party (e.g., citizen, organization).</param>
        /// <returns>
        ///   The dictionary of &lt;placeholder, value&gt; used for personalization of "Notify NL" Web API service notification.
        /// </returns>
        protected abstract Dictionary<string, object> GetSmsPersonalization(CommonPartyData partyData);
        #endregion

        #region Abstract (GetWhitelistName)        
        /// <summary>
        /// Gets the name of this specific scenario.
        /// </summary>
        /// <returns></returns>
        protected abstract string GetWhitelistName();
        #endregion
    }
}