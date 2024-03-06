// © 2024, Worth Systems.

using Asp.Versioning;
using EventsHandler.Attributes.Authorization;
using EventsHandler.Configuration;
using EventsHandler.Constants;
using Microsoft.AspNetCore.Mvc;
using Notify.Client;
using Notify.Exceptions;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace EventsHandler.Controllers
{
    [ApiController]
    [Route(DefaultValues.ApiController.Route)]
    [Consumes(DefaultValues.Request.ContentType)]
    [Produces(DefaultValues.Request.ContentType)]
    [ApiVersion(DefaultValues.ApiController.Version)]
    public sealed class TestController : ControllerBase
    {
        private readonly WebApiConfiguration _configuration;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="TestController"/> class.
        /// </summary>
        /// <param name="configuration">The service handling Data Provider (DAO) loading strategies.</param>
        public TestController(WebApiConfiguration configuration)
        {
            this._configuration = configuration;
        }

        /// <summary>
        /// Tests the NotifyNL API service - whether it's running, _configuration is proper, and communication possible.
        /// </summary>
        /// <param name="emailAddress">The email address where the notification should be sent through NotifyNL API service.</param>
        /// <param name="templateId">The email address where the notification should be sent through NotifyNL API service.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("Notify/SendEmail")]
        // Security
        [ApiAuthorization]
        public async Task<IActionResult> SendEmailAsync([Required] string emailAddress, string? templateId = null)
        {
            // Initialize the .NET client of NotifyNL API service
            var notifyClient = new NotificationClient(
                this._configuration.Notify.API.BaseUrl(),
                this._configuration.User.API.Key.NotifyNL());

            // Determine first possible Email template ID if nothing was provided
            templateId ??= (await notifyClient.GetAllTemplatesAsync("email")).templates.First().id;

            // Sending Email through NotifyNL API service
            try
            {
                _ = await notifyClient.SendEmailAsync(emailAddress, templateId);

                return Ok("Test SUCCESSFUL: Sending Email to NotifyNL email service");
            }
            catch (NotifyClientException exception)
            {
                string message;
                Match match;

                if ((match = _personalisationPattern.Match(exception.Message)).Success)
                {
                    message = match.Groups[PersonalisationGroup].Value;
                }
                else
                {
                    message = exception.Message;
                }

                return BadRequest($"Test FAILURE:\n\n{message}");
            }
            catch (Exception exception)
            {
                return BadRequest($"Test FAILURE:\n\n{exception.Message}");
            }
        }

        private const string PersonalisationGroup = nameof(PersonalisationGroup);

        private readonly Regex _personalisationPattern =
            new($"(?<{PersonalisationGroup}>Missing personalisation\\:.+?)\\\\", RegexOptions.Compiled);
    }
}
