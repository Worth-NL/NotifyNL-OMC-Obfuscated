﻿// © 2023, Worth Systems.

using EventsHandler.Attributes.Authorization;
using EventsHandler.Attributes.Validation;
using EventsHandler.Behaviors.Mapping.Enums;
using EventsHandler.Behaviors.Mapping.Models.POCOs.NotificatieApi;
using EventsHandler.Constants;
using EventsHandler.Controllers.Base;
using EventsHandler.Extensions;
using EventsHandler.Services.DataProcessing.Interfaces;
using EventsHandler.Services.Responding.Interfaces;
using EventsHandler.Services.Responding.Messages.Models.Errors;
using EventsHandler.Services.Serialization.Interfaces;
using EventsHandler.Services.Settings.Configuration;
using EventsHandler.Services.Validation.Interfaces;
using EventsHandler.Services.Versioning.Interfaces;
using EventsHandler.Utilities.Swagger.Examples;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;
using System.ComponentModel.DataAnnotations;
using Resources = EventsHandler.Properties.Resources;

namespace EventsHandler.Controllers
{
    /// <summary>
    /// Controller handling events workflow between "Notificatie API" events queue, services with citizens personal
    /// data from the municipalities in The Netherlands ("OpenZaak" and "OpenKlaant"), and "Notify NL" API service.
    /// </summary>
    /// <seealso cref="OmcController"/>
    public sealed class EventsController : OmcController
    {
        private readonly WebApiConfiguration _configuration;
        private readonly ISerializationService _serializer;
        private readonly IValidationService<NotificationEvent> _validator;
        private readonly IProcessingService<NotificationEvent> _processor;
        private readonly IRespondingService<NotificationEvent> _responder;
        private readonly IVersionsRegister _register;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventsController"/> class.
        /// </summary>
        /// <param name="configuration">The configuration of the application.</param>
        /// <param name="serializer">The input de(serializing) service.</param>
        /// <param name="validator">The input validating service.</param>
        /// <param name="processor">The input processing service (business logic).</param>
        /// <param name="responder">The output standardization service (UX/UI).</param>
        /// <param name="register">The register of versioned services.</param>
        public EventsController(
            WebApiConfiguration configuration,
            ISerializationService serializer,
            IValidationService<NotificationEvent> validator,
            IProcessingService<NotificationEvent> processor,
            IRespondingService<NotificationEvent> responder,
            IVersionsRegister register)
        {
            this._configuration = configuration;
            this._serializer = serializer;
            this._validator = validator;
            this._processor = processor;
            this._responder = responder;
            this._register = register;
        }

        /// <summary>
        /// Callback URL listening to notification events from subscribed channels.
        /// </summary>
        /// <param name="json">The notification from "OpenNotificaties" Web API service (as a plain JSON object).</param>
        [HttpPost]
        [Route("Listen")]
        // Security
        [ApiAuthorization]
        // User experience
        [StandardizeApiResponses]  // NOTE: Replace errors raised by ASP.NET Core with standardized API responses
        // Swagger UI
        [SwaggerRequestExample(typeof(NotificationEvent), typeof(NotificationEventExample))]  // NOTE: Documentation of expected JSON schema with sample and valid payload values
        [ProducesResponseType(StatusCodes.Status202Accepted)]                                                       // REASON: The notification was valid, and it was successfully sent to "Notify NL" Web API service
        [ProducesResponseType(StatusCodes.Status206PartialContent)]                                                 // REASON: The notification was not sent (e.g., "test" ping received or scenario is not yet implemented. No need to retry sending it)
        [ProducesResponseType(StatusCodes.Status400BadRequest,          Type = typeof(ProcessingFailed.Detailed))]  // REASON: The notification was not sent (e.g., it was invalid due to missing data or improper structure. Retry sending is required)
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(ProcessingFailed.Detailed))]  // REASON: Input deserialization error (e.g. model binding of required properties)
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProcessingFailed.Detailed))]  // REASON: Internal server error (if-else / try-catch-finally handle)
        [ProducesResponseType(StatusCodes.Status501NotImplemented,      Type = typeof(string))]                     // REASON: Operation is not implemented (a new case is not yet supported)
        public async Task<IActionResult> ListenAsync([Required, FromBody] object json)
        {
            /* Validation #1: The validation of JSON payload structure and model-binding of [Required] properties are
             *                happening on the level of [FromBody] annotation. The attribute [StandardizeApiResponses]
             *                is meant to intercept native framework errors, raised immediately by ASP.NET Core validation
             *                mechanism, and to re-pack them ("beautify") into user-friendly standardized API responses */
            try
            {
                // Deserialize received JSON payload
                NotificationEvent notification = this._serializer.Deserialize<NotificationEvent>(json);

                // Validation #2: Structural and data inconsistencies of optional properties
                return this._validator.Validate(ref notification) is HealthCheck.OK_Valid
                                                                  or HealthCheck.OK_Inconsistent
                    // Try to process the received notification
                    ? await Task.Run<IActionResult>(async () =>
                    {
                        (ProcessingResult Status, string) result = await this._processor.ProcessAsync(notification);

                        return LogApiResponse(result.Status.ConvertToLogLevel(),
                            this._responder.Get_Processing_Status_ActionResult(
                                GetResult(result, json), notification.Details));
                    })

                    // The notification cannot be processed
                    : LogApiResponse(LogLevel.Error,
                        this._responder.Get_Processing_Status_ActionResult(
                            GetAbortedResult(notification.Details.Message, json), notification.Details));
            }
            catch (Exception exception)
            {
                // Serious problems occurred during the attempt to process the notification
                return LogApiResponse(exception,
                    this._responder.Get_Exception_ActionResult(exception));
            }
        }

        /// <summary>
        /// Gets the current version and setup of the OMC (Output Management Component).
        /// </summary>
        [HttpGet]
        [Route("Version")]
        // Security
        [ApiAuthorization]
        // User experience
        [StandardizeApiResponses]  // NOTE: Replace errors raised by ASP.NET Core with standardized API responses
        // Swagger UI
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Version()
        {
            LogApiResponse(LogLevel.Trace, Resources.Events_ApiVersionRequested);

            return Ok($"{Resources.Application_Name}: v{DefaultValues.ApiController.Version} " +
                      $"({Environment.GetEnvironmentVariable(DefaultValues.EnvironmentVariables.AspNetCoreEnvironment)}) | " +
                      $"Workflow: v{this._configuration.AppSettings.Features.OmcWorkflowVersion()} " +
                      $"{this._register.GetVersions()}");
        }

        #region Helper methods
        private static (ProcessingResult, string) GetResult((ProcessingResult Status, string Description) result, object json)
        {
            return (result.Status, EnrichDescription(result.Description, json));
        }

        private static (ProcessingResult, string) GetAbortedResult(string message, object json)
        {
            return (ProcessingResult.NotPossible, EnrichDescription(message, json));
        }

        private static string EnrichDescription(string originalText, object json)
        {
            return $"{originalText} | Notification: {json}";
        }
        #endregion
    }
}