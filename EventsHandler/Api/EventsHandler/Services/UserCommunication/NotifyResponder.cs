// © 2024, Worth Systems.

using EventsHandler.Behaviors.Responding.Messages.Models.Details.Base;
using EventsHandler.Behaviors.Responding.Messages.Models.Informations;
using EventsHandler.Services.UserCommunication.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Notify.Exceptions;
using Notify.Models.Responses;
using System.Net;
using System.Text.RegularExpressions;

namespace EventsHandler.Services.UserCommunication
{
    /// <inheritdoc cref="IRespondingService{TResult, TDetails}"/>
    internal sealed partial class NotifyResponder : IRespondingService<NotificationResponse, BaseSimpleDetails>  // NOTE: "partial" is introduced by the new RegEx generation approach
    {
        #region RegEx patterns
        [GeneratedRegex("Invalid token: service not found", RegexOptions.Compiled)]
        private static partial Regex InvalidApiKeyPattern();

        // Email
        [GeneratedRegex("Address field is required", RegexOptions.Compiled)]
        private static partial Regex MissingEmailAddressPattern();

        [GeneratedRegex("Not a valid email address", RegexOptions.Compiled)]
        private static partial Regex InvalidEmailSymbolsPattern();

        // Phone number
        [GeneratedRegex("Number field is required", RegexOptions.Compiled)]
        private static partial Regex MissingPhoneNumberPattern();

        [GeneratedRegex("Must not contain letters or symbols", RegexOptions.Compiled)]
        private static partial Regex InvalidPhoneSymbolsPattern();

        [GeneratedRegex("Not enough digits", RegexOptions.Compiled)]
        private static partial Regex InvalidPhoneTooShortPattern();

        [GeneratedRegex("Too many digits", RegexOptions.Compiled)]
        private static partial Regex InvalidPhoneTooLongPattern();

        [GeneratedRegex("Please enter mobile number according to the expected format", RegexOptions.Compiled)]
        private static partial Regex InvalidPhoneFormatPattern();

        // Template or its ID
        [GeneratedRegex("not a valid UUID", RegexOptions.Compiled)]
        private static partial Regex InvalidTemplateIdFormatPattern();
        
        [GeneratedRegex("Template not found", RegexOptions.Compiled)]
        private static partial Regex NotFoundTemplatePattern();

        // Personalization
        [GeneratedRegex("Missing personalisation\\:[a-z.,\\ ]+", RegexOptions.Compiled)]
        private static partial Regex MissingPersonalizationPattern();
        #endregion

        #region IRespondingService
        /// <inheritdoc cref="IRespondingService.GetStandardized_Exception_ActionResult(Exception)"/>
        ObjectResult IRespondingService.GetStandardized_Exception_ActionResult(Exception exception)
        {
            switch (exception)
            {
                case NotifyClientException:
                {
                    string message;
                    Match match;

                    // HttpStatus Code: 403 Forbidden
                    if ((match = InvalidApiKeyPattern().Match(exception.Message)).Success)  // NOTE: The API key is invalid (access to NotifyNL API service denied)
                    {
                        message = match.Value;

                        // NOTE: This specific error message is inconsistently returned from Notify .NET client with 403 Forbidden status code (unlike others - with 400 BadRequest code)
                        return new ObjectResult(new ProcessingFailed.Simplified(HttpStatusCode.Forbidden, message))
                        {
                            StatusCode = (int)HttpStatusCode.Forbidden
                        };
                    }
                
                    // HttpStatus Code: 400 BadRequest
                    if ((match = MissingEmailAddressPattern().Match(exception.Message)).Success ||  // NOTE: The email address is empty (whitespaces only)
                        (match = InvalidEmailSymbolsPattern().Match(exception.Message)).Success)    // NOTE: The email address is invalid
                    {
                        const string email = "Email: ";

                        message = $"{email}{match.Value}";
                    }
                    else if ((match = MissingPhoneNumberPattern().Match(exception.Message)).Success   ||  // NOTE: The phone number is empty (whitespaces only)
                             (match = InvalidPhoneSymbolsPattern().Match(exception.Message)).Success  ||  // NOTE: The phone number contains letters or illegal symbols
                             (match = InvalidPhoneTooShortPattern().Match(exception.Message)).Success ||  // NOTE: The phone number contains not enough digits
                             (match = InvalidPhoneTooLongPattern().Match(exception.Message)).Success  ||  // NOTE: The phone number contains too many digits
                             (match = InvalidPhoneFormatPattern().Match(exception.Message)).Success)      // NOTE: The phone number format is invalid: e.g., the country code is unsupported
                    {
                        const string phone = "Phone: ";

                        message = $"{phone}{match.Value}";
                    }
                    else if (InvalidTemplateIdFormatPattern().Match(exception.Message).Success)  // NOTE: The template ID is not in UUID (Universal Unique Identifier) format
                    {
                        const string template = "Template: Is ";  // "is" is not capitalized in the original error message

                        message = $"{template}{match.Value}";
                    }
                    else if ((match = NotFoundTemplatePattern().Match(exception.Message)).Success ||      // NOTE: The message template could not be find based on provided template ID
                             (match = MissingPersonalizationPattern().Match(exception.Message)).Success)  // NOTE: Personalization was required by message template but wasn't provided
                    {
                        message = match.Value;
                    }
                    else
                    {
                        // Everything else that is throw as NotifyClientException (NOTE: for better UX experience it should be also handled and formatted appropriately)
                        message = exception.Message;
                    }

                    return new BadRequestObjectResult(new ProcessingFailed.Simplified(HttpStatusCode.BadRequest, message));
                }

                case NotifyAuthException:
                    // NOTE: Authorization issues wrapped around 403 Forbidden status code
                    return new ObjectResult(new ProcessingFailed.Simplified(HttpStatusCode.Forbidden, exception.Message))
                    {
                        StatusCode = (int)HttpStatusCode.Forbidden
                    };

                default:
                    // NOTE: Unexpected issues wrapped around 500 Internal Server Error status code
                    return new BadRequestObjectResult(new ProcessingFailed.Simplified(HttpStatusCode.InternalServerError, exception.Message));
            }
        }

        /// <inheritdoc cref="IRespondingService.GetStandardized_Exception_ActionResult(string)"/>
        ObjectResult IRespondingService.GetStandardized_Exception_ActionResult(string errorMessage)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region IRespondingService<TResult, TDetails>
        /// <inheritdoc cref="IRespondingService{TResult, TDetails}.GetStandardized_Processing_ActionResult(TResult, TDetails)"/>
        ObjectResult IRespondingService<NotificationResponse, BaseSimpleDetails>.GetStandardized_Processing_ActionResult(NotificationResponse result, BaseSimpleDetails details)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="IRespondingService{TResult, TDetails}.GetStandardized_Processing_Failed_ActionResult(TDetails)"/>
        ObjectResult IRespondingService<NotificationResponse, BaseSimpleDetails>.GetStandardized_Processing_Failed_ActionResult(BaseSimpleDetails details)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}