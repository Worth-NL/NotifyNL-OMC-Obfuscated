﻿// © 2023, Worth Systems.

using EventsHandler.Exceptions;
using EventsHandler.Services.DataProcessing.Strategy.Models.DTOs;
using EventsHandler.Services.DataProcessing.Strategy.Responses;
using System.Text.Json;
using NotificatieApi;
using NotificatieApi;

namespace EventsHandler.Services.DataProcessing.Strategy.Base.Interfaces
{
    /// <summary>
    /// The "Notify NL" business-cases for notification scenarios.
    /// </summary>
    internal interface INotifyScenario
    {
        /// <summary>
        /// Prepares all data consumed by "Notify NL" API Client.
        /// </summary>
        /// <param name="notification"><inheritdoc cref="ZhvModels.Mapping.Models.POCOs.NotificatieApi.NotificationEvent" path="/summary"/></param>
        /// <returns>
        ///   The data required by "Notify NL".
        /// </returns>
        /// <exception cref="KeyNotFoundException">
        ///   The looked up key to configuration value is missing or invalid.
        /// </exception>
        /// <exception cref="HttpRequestException">
        ///   Something could not be queried from external Web API services.
        /// </exception>
        /// <exception cref="JsonException">
        ///   The HTTP response wasn't deserialized properly.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///   Some internal business logic failed (implementation mistake).
        /// </exception>
        /// <exception cref="AbortedNotifyingException">
        ///   The notification should not be sent.
        /// </exception>
        internal Task<GettingDataResponse> TryGetDataAsync(NotificationEvent notification);

        /// <summary>
        /// Processes the prepared data in a specific way (determined by the scenario itself).
        /// </summary>
        /// <param name="notification"><inheritdoc cref="ZhvModels.Mapping.Models.POCOs.NotificatieApi.NotificationEvent" path="/summary"/></param>
        /// <param name="notifyData"><inheritdoc cref="NotifyData" path="/summary"/></param>
        /// <returns>
        ///   The status of the processing operation.
        /// </returns>
        internal Task<ProcessingDataResponse> ProcessDataAsync(NotificationEvent notification, IReadOnlyCollection<NotifyData> notifyData);
    }
}