// © 2024, Worth Systems.

using Asp.Versioning;
using EventsHandler.Attributes.Authorization;
using EventsHandler.Attributes.Validation;
using EventsHandler.Behaviors.Communication.Enums;
using EventsHandler.Behaviors.Mapping.Models.POCOs.NotificatieApi;
using EventsHandler.Behaviors.Responding.Messages.Models.Informations;
using EventsHandler.Configuration;
using EventsHandler.Constants;
using EventsHandler.Services.UserCommunication.Interfaces;
using EventsHandler.Utilities.Swagger.Examples;
using Microsoft.AspNetCore.Mvc;
using Notify.Client;
using Notify.Exceptions;
using Notify.Models.Responses;
using Swashbuckle.AspNetCore.Filters;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace EventsHandler.Controllers
{
    /// <summary>
    /// Controller used to test other API services from which NotifyNL OMC is dependent.
    /// </summary>
    /// <seealso cref="ControllerBase"/>
    [ApiController]
    [Route(DefaultValues.ApiController.Route)]
    [Consumes(DefaultValues.Request.ContentType)]
    [Produces(DefaultValues.Request.ContentType)]
    [ApiVersion(DefaultValues.ApiController.Version)]
    public sealed class TestController : ControllerBase
    {
        // TODO: To be extracted into IValidationService
        private readonly Regex _invalidApiKeyPattern = new("Invalid token: service not found", RegexOptions.Compiled);
        private readonly Regex _templateIdInvalidFormatPattern = new("template_id is not a valid UUID", RegexOptions.Compiled);
        private readonly Regex _templateNotFoundPattern = new("Template not found", RegexOptions.Compiled);
        private readonly Regex _personalizationMissingPattern = new("Missing personalisation\\:[a-z.,\\ ]+", RegexOptions.Compiled);

        private readonly WebApiConfiguration _configuration;
        private readonly IRespondingService<NotificationEvent> _responder;  // TODO: To be used
        
        /// <summary>
        /// Initializes a new instance of the <see cref="TestController"/> class.
        /// </summary>
        /// <param name="configuration">The service handling Data Provider (DAO) loading strategies.</param>
        /// <param name="responder">The output standardization service (UX/UI).</param>
        public TestController(
            WebApiConfiguration configuration,
            IRespondingService<NotificationEvent> responder)  // TODO: Different generic to be used
        {
            this._configuration = configuration;
            this._responder = responder;
        }

        /// <summary>
        /// Sending Email messages to the NotifyNL API service. 
        /// Ensure that base URL and API key for NotifyNL API service are provided in the configuration.
        /// </summary>
        /// <param name="emailAddress">The email address (required) where the notification should be sent.</param>
        /// <param name="emailTemplateId">The email template ID (optional) to be used from NotifyNL API service.
        ///   <para>
        ///     If empty the ID of a very first looked up email template will be used.
        ///   </para>
        /// </param>
        /// <param name="personalization">The map (optional) of keys and values to be used as message personalization.
        ///   <para>
        ///     Example of personalization in template from NotifyNL Admin Portal: "This is ((placeholderText)) information".
        ///   </para>
        ///   <para>
        ///     Example of personalization values to be provided: { "placeholderText": "good" }
        ///   </para>
        ///   <para>
        ///     Resulting message would be: "This is good information" (or exception, if personalization is required but not provided).
        ///   </para>
        /// </param>
        [HttpPost]
        [Route("Notify/SendEmail")]
        // Security
        [ApiAuthorization]
        // User experience
        [StandardizeApiResponses]  // NOTE: Replace errors raised by ASP.NET Core with standardized API responses
        // Swagger UI
        [SwaggerRequestExample(typeof(Dictionary<string, object>), typeof(PersonalizationExample))]
        [ProducesResponseType(StatusCodes.Status202Accepted)]                                                         // REASON: The notification successfully sent to NotifyNL API service
        [ProducesResponseType(StatusCodes.Status401Unauthorized,        Type = typeof(string))]                       // REASON: JWT Token is invalid or expired
        [ProducesResponseType(StatusCodes.Status403Forbidden,           Type = typeof(ProcessingFailed.Simplified))]  // REASON: Base URL or API key to NotifyNL API service were incorrect
        [ProducesResponseType(StatusCodes.Status400BadRequest,          Type = typeof(ProcessingFailed.Simplified))]  // REASON: Issues on the NotifyNL API service side
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProcessingFailed.Simplified))]  // REASON: Unexpected internal error, not yet handled by Notify OMC logic
        public async Task<IActionResult> SendEmailAsync(
            [Required, FromQuery] string emailAddress,
            [Optional, FromQuery] string? emailTemplateId,
            [Optional, FromBody] Dictionary<string, object> personalization)
        {
            return await SendAsync(
                NotifyMethods.Email,
                emailAddress,
                emailTemplateId,
                personalization);
        }

        /// <summary>
        /// Sending SMS text messages to the NotifyNL API service. 
        /// Ensure that base URL and API key for NotifyNL API service are provided in the configuration.
        /// </summary>
        /// <param name="mobileNumber">The mobile phone number (required) where the notification should be sent.</param>
        /// <param name="smsTemplateId">The SMS template ID (optional) to be used from NotifyNL API service.
        ///   <para>
        ///     If empty the ID of a very first looked up SMS template will be used.
        ///   </para>
        /// </param>
        /// <param name="personalization">
        ///   <inheritdoc cref="SendEmailAsync" path="/param[@name='personalization']"/>
        /// </param>
        [HttpPost]
        [Route("Notify/SendSms")]
        // Security
        [ApiAuthorization]
        // User experience
        [StandardizeApiResponses]  // NOTE: Replace errors raised by ASP.NET Core with standardized API responses
        // Swagger UI
        [SwaggerRequestExample(typeof(Dictionary<string, object>), typeof(PersonalizationExample))]
        [ProducesResponseType(StatusCodes.Status202Accepted)]                                                         // REASON: The notification successfully sent to NotifyNL API service
        [ProducesResponseType(StatusCodes.Status401Unauthorized,        Type = typeof(string))]                       // REASON: JWT Token is invalid or expired
        [ProducesResponseType(StatusCodes.Status403Forbidden,           Type = typeof(ProcessingFailed.Simplified))]  // REASON: Base URL or API key to NotifyNL API service were incorrect
        [ProducesResponseType(StatusCodes.Status400BadRequest,          Type = typeof(ProcessingFailed.Simplified))]  // REASON: Issues on the NotifyNL API service side
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProcessingFailed.Simplified))]  // REASON: Unexpected internal error, not yet handled by Notify OMC logic
        public async Task<IActionResult> SendSmsAsync(
            [Required, FromQuery] string mobileNumber,
            [Optional, FromQuery] string? smsTemplateId,
            [Optional, FromBody] Dictionary<string, object> personalization)
        {
            return await SendAsync(
            NotifyMethods.Sms,
                mobileNumber,
                smsTemplateId,
                personalization);
        }

        #region Helper methods
        private static readonly Dictionary<NotifyMethods, string> s_templateTypes = new()
        {
            { NotifyMethods.Sms,   "sms"   },
            { NotifyMethods.Email, "email" }
        };

        /// <summary>
        /// Generic method sending notification through <see cref="NotificationClient"/> and handling its responses in a standardized way.
        /// </summary>
        /// <returns>
        ///   The standardized <see cref="ObjectResult"/> API response.
        /// </returns>
        private async Task<IActionResult> SendAsync(
            NotifyMethods notifyMethod,
            string contactDetails,
            string? templateId,
            Dictionary<string, object> personalization)
        {
            // TODO: This logic should be extracted into dedicated services (especially the logging errors part, as an extension of existing IRespondingService)
            try
            {
                // Initialize the .NET client of NotifyNL API service
                var notifyClient = new NotificationClient(    // TODO: Client to be resolved by IClientFactory (to be testable)
                    this._configuration.Notify.API.BaseUrl(),
                    this._configuration.User.API.Key.NotifyNL());

                // Determine template type
                string templateType = s_templateTypes[notifyMethod];

                // Determine first possible Email template ID if nothing was provided
                List<TemplateResponse>? allTemplates = (await notifyClient.GetAllTemplatesAsync(templateType)).templates; // NOTE: Assign to variables for debug purposes
                templateId ??= allTemplates.First().id;

                // TODO: To be extracted into a dedicated service
                // NOTE: Empty personalization
                if (personalization.Count == 1 || personalization.ContainsKey("key"))
                {
                    NotificationResponse _ = notifyMethod switch
                    {
                        NotifyMethods.Email => await notifyClient.SendEmailAsync(contactDetails, templateId),
                        NotifyMethods.Sms   => await notifyClient.SendSmsAsync(contactDetails, templateId),
                        _                   => NotImplementedNotifyMethod()
                    };
                }
                // NOTE: Personalization was provided by the user
                else
                {
                    NotificationResponse _ = notifyMethod switch
                    {
                        NotifyMethods.Email => await notifyClient.SendEmailAsync(contactDetails, templateId, personalization),
                        NotifyMethods.Sms   => await notifyClient.SendSmsAsync(contactDetails, templateId, personalization),
                        _                   => NotImplementedNotifyMethod()
                    };
                }

                return Ok(new ProcessingFailed.Simplified(HttpStatusCode.Accepted, "Email was successfully send to NotifyNL"));
            }
            catch (NotifyClientException exception)
            {
                string message;
                Match match;

                if (_templateIdInvalidFormatPattern.Match(exception.Message).Success)  // NOTE: The template ID is not in UUID (Universal Unique Identifier) format
                {
                    const string invalidUuid = "Template ID is not a valid UUID";

                    message = invalidUuid;
                }
                else if ((match = _invalidApiKeyPattern.Match(exception.Message)).Success)  // NOTE: The API key is invalid (access to NotifyNL API service denied)
                {
                    message = match.Value;
                    
                    // NOTE: This specific error message is inconsistently returned from Notify .NET client with 403 Forbidden status code (unlike others - with 400 BadRequest code)
                    return StatusCode((int)HttpStatusCode.Forbidden, new ProcessingFailed.Simplified(HttpStatusCode.Forbidden, message));
                }
                else if ((match = _templateNotFoundPattern.Match(exception.Message)).Success ||      // NOTE: The message template could not be find based on provided template ID
                         (match = _personalizationMissingPattern.Match(exception.Message)).Success)  // NOTE: Personalization was required by message template but wasn't provided
                {
                    message = match.Value;
                }
                else
                {
                    // Everything else, returned as NotifyClientException (to be handled during development process, for better UX experience)
                    message = exception.Message;
                }

                return BadRequest(new ProcessingFailed.Simplified(HttpStatusCode.BadRequest, message));
            }
            // NOTE: Authorization issues wrapped around 403 Forbidden status code
            catch (NotifyAuthException exception)
            {
                return StatusCode((int)HttpStatusCode.Forbidden, new ProcessingFailed.Simplified(HttpStatusCode.Forbidden, exception.Message));
            }
            // NOTE: Unexpected issues wrapped around 500 Internal Server Error status code
            catch (Exception exception)
            {
                return BadRequest(new ProcessingFailed.Simplified(HttpStatusCode.InternalServerError, exception.Message));
            }
        }

        private static NotificationResponse NotImplementedNotifyMethod()
            => throw new ArgumentException(@"This notification method is not supported");
        #endregion
    }
}
