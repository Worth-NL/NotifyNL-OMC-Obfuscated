﻿// © 2023, Worth Systems.

using EventsHandler.Behaviors.Mapping.Enums;
using EventsHandler.Behaviors.Mapping.Models.POCOs.NotificatieApi;
using EventsHandler.Behaviors.Responding.Messages.Models.Details;
using EventsHandler.Behaviors.Responding.Messages.Models.Details.Base;
using EventsHandler.Behaviors.Responding.Messages.Models.Informations;
using EventsHandler.Behaviors.Responding.Messages.Models.Successes;
using EventsHandler.Behaviors.Responding.Results.Builder.Interface;
using EventsHandler.Behaviors.Responding.Results.Enums;
using EventsHandler.Behaviors.Responding.Results.Extensions;
using EventsHandler.Constants;
using EventsHandler.Properties;
using EventsHandler.Services.UserCommunication.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EventsHandler.Services.UserCommunication
{
    /// <inheritdoc cref="IRespondingService{TModel}"/>
    internal sealed class NotificationResponder : IRespondingService<NotificationEvent>
    {
        private readonly IDetailsBuilder _detailsBuilder;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationResponder"/> class.
        /// </summary>
        public NotificationResponder(IDetailsBuilder builder)
        {
            this._detailsBuilder = builder;
        }

        #region IRespondingService
        /// <inheritdoc cref="IRespondingService.GetStandardized_Exception_ActionResult(Exception)"/>
        ObjectResult IRespondingService.GetStandardized_Exception_ActionResult(Exception exception)
        {
            if (exception is HttpRequestException)
            {
                return this._detailsBuilder.Get<ErrorDetails>(Reasons.HttpRequestError, exception.Message).AsResult_400();
            }

            return ((IRespondingService<NotificationEvent>)this).GetStandardized_Exception_ActionResult(exception.Message);
        }

        /// <inheritdoc cref="IRespondingService.GetStandardized_Exception_ActionResult(string)"/>
        ObjectResult IRespondingService.GetStandardized_Exception_ActionResult(string errorMessage)
        {
            try
            {
                // JSON serialization issues
                if (errorMessage.StartsWith(DefaultValues.Validation.Deserialization_MissingProperty))
                {
                    return this._detailsBuilder.Get<ErrorDetails>(Reasons.MissingProperties_Notification, GetMissingPropertiesNames(errorMessage)).AsResult_422();
                }

                return errorMessage.StartsWith(DefaultValues.Validation.Deserialization_InvalidValue)
                    // JSON serialization issues
                    ? this._detailsBuilder.Get<ErrorDetails>(Reasons.InvalidProperties_Notification, errorMessage).AsResult_422()
                    // Invalid JSON structure
                    : this._detailsBuilder.Get<ErrorDetails>(Reasons.InvalidJson, errorMessage).AsResult_422();
            }
            catch
            {
                return this._detailsBuilder.Get<UnknownDetails>(Reasons.ValidationIssue).AsResult_500();
            }
        }
        #endregion

        #region IRespondingService<TModel>
        /// <inheritdoc cref="IRespondingService{TResult, TDetails}.GetStandardized_Processing_ActionResult(TResult, TDetails)"/>
        ObjectResult IRespondingService<(ProcessingResult, string), BaseEnhancedDetails>.GetStandardized_Processing_ActionResult((ProcessingResult, string) result, BaseEnhancedDetails details)
        {
            return ((IRespondingService<NotificationEvent>)this).GetStandardized_Processing_ActionResult(result, details);
        }

        /// <inheritdoc cref="IRespondingService{TResult, TDetails}.GetStandardized_Processing_Failed_ActionResult(TDetails)"/>
        ObjectResult IRespondingService<(ProcessingResult, string), BaseEnhancedDetails>.GetStandardized_Processing_Failed_ActionResult(BaseEnhancedDetails details)
        {
            return ((IRespondingService<NotificationEvent>)this).GetStandardized_Processing_Failed_ActionResult(details);
        }
        #endregion

        #region Implementation
        /// <inheritdoc cref="IRespondingService{TModel}.GetStandardized_Processing_ActionResult(ValueTuple{ProcessingResult, string}, BaseEnhancedDetails)"/>
        ObjectResult IRespondingService<NotificationEvent>.GetStandardized_Processing_ActionResult((ProcessingResult Status, string Description) result, BaseEnhancedDetails details)
        {
            return result.Status switch
            {
                ProcessingResult.Success
                    => new ProcessingSucceeded(result.Description, details).AsResult_202(),

                ProcessingResult.Skipped
                    => new ProcessingSkipped(result.Description, details).AsResult_206(),

                ProcessingResult.Failure
                    => string.Equals(details.Message, Resources.Operation_RESULT_Deserialization_Failure) ||
                       string.Equals(details.Message, Resources.Deserialization_INFO_UnexpectedData_Notification_Message)
                        ? new ProcessingFailed.Detailed(HttpStatusCode.UnprocessableEntity, result.Description, details).AsResult_422()
                        : new ProcessingFailed.Simplified(HttpStatusCode.BadRequest, result.Description).AsResult_400(),

                _ => ObjectResultExtensions.AsResult_501()
            };
        }

        /// <inheritdoc cref="IRespondingService{TModel}.GetStandardized_Processing_Failed_ActionResult(BaseEnhancedDetails)"/>
        ObjectResult IRespondingService<NotificationEvent>.GetStandardized_Processing_Failed_ActionResult(BaseEnhancedDetails details)
        {
            return ((IRespondingService<NotificationEvent>)this).GetStandardized_Processing_ActionResult(
                (ProcessingResult.Failure, Resources.Processing_ERROR_Scenario_NotificationNotSent),
                details);
        }
        #endregion

        #region Helper methods
        /// <summary>
        /// Gets the names of missing JSON properties.
        /// </summary>
        /// <param name="errorMessage">The error message produced by validation framework during model binding.</param>
        /// <returns>
        ///   A single property or comma separated string of properties.
        /// </returns>
        private static string GetMissingPropertiesNames(string errorMessage)
        {
            const int spaceAndOffset = 2;

            int startIndexWhereMissingPropertiesAreListed = errorMessage.LastIndexOf(':') + spaceAndOffset;

            return errorMessage[startIndexWhereMissingPropertiesAreListed..];
        }
        #endregion
    }
}