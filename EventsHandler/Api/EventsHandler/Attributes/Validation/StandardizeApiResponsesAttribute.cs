// © 2023, Worth Systems.

using EventsHandler.Behaviors.Mapping.Models.POCOs.NotificatieApi;
using EventsHandler.Behaviors.Responding.Messages.Models.Base;
using EventsHandler.Constants;
using EventsHandler.Services.UserCommunication.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EventsHandler.Attributes.Validation
{
    /// <summary>
    /// The UX wrapper to handle and display ASP.NET Core MVC framework errors into a standardized human-friendly API responses.
    /// </summary>
    /// <seealso cref="ActionFilterAttribute"/>
    [AttributeUsage(AttributeTargets.Method)]
    internal sealed class StandardizeApiResponsesAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Intercepts the <see cref="IActionResult"/> error messages from the validation of
        /// <see cref="NotificationEvent"/> to display <see cref="BaseApiStandardResponseBody"/>.
        /// </summary>
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            // Intercepting model binding validation issues
            if (ContainsValidationProblems(context, out ValidationProblemDetails? details))
            {
                // TODO: Switch to IRespondingService with different generic to catch generic JSON issues (without text "Notification could not be...")
                IRespondingService<NotificationEvent> outputService =
                    context.HttpContext.RequestServices.GetRequiredService<IRespondingService<NotificationEvent>>();

                // Replacing native error messages by user-friendly API responses
                if (ContainsErrorMessage(details!.Errors, out string errorMessage))
                {
                    context.Result = outputService.GetStandardized_Exception_ActionResult(errorMessage);
                }
            }

            base.OnResultExecuting(context);
        }

        #region Helper methods        
        /// <summary>
        /// Performs a series of checks on <see cref="ResultExecutingContext"/> to determine
        /// whether a specific problem (<see cref="ValidationProblemDetails"/>) has occurred.
        /// </summary>
        /// <param name="context">The context to be checked.</param>
        /// <param name="validationProblemDetails">The retrieved details.</param>
        /// <returns>
        ///   <see langword="true"/> if specific problem with model binding was encountered; otherwise, <see langword="false"/>.
        /// </returns>
        private static bool ContainsValidationProblems(ResultExecutingContext context, out ValidationProblemDetails? validationProblemDetails)
        {
            if (!context.ModelState.IsValid &&
                context.Result is BadRequestObjectResult result &&
                result.Value is ValidationProblemDetails details &&
                details.Title == DefaultValues.Validation.ErrorsOccurred)
            {
                validationProblemDetails = details;

                return true;
            }

            validationProblemDetails = default;

            return false;
        }

        /// <summary>
        /// Tries to get valid and meaningful error message from the collection of encountered errors.
        /// </summary>
        /// <param name="errorDetails">The error details to be revieved.</param>
        /// <param name="errorMessage">The error message to be returned.</param>
        /// <returns>
        ///   <see langword="true"/> if an error message was found; otherwise, <see langword="false"/>.
        /// </returns>
        private static bool ContainsErrorMessage(IDictionary<string, string[]> errorDetails, out string errorMessage)
        {
            const int MessageIndex = 0;     // NOTE: Under index 1 the source is stored (where the error encountered: property, object)
            const string MessageKey = "$";  // NOTE: The data binding validation mechanism is storing error messages under this predefined key + .[source]

            // Known keys where error messages are present for sure
            if (errorDetails.TryGetValue(MessageKey,                out string[]? errorMessages) ||
                errorDetails.TryGetValue($"{MessageKey}.kanaal",    out           errorMessages) ||
                errorDetails.TryGetValue($"{MessageKey}.kenmerken", out           errorMessages))
            {
                errorMessage = errorMessages[MessageIndex];

                return true;
            }
            // Dynamic fallback strategy, to retrieve an error message anyway
            else
            {
                KeyValuePair<string, string[]>[] errorDetailsPairs = errorDetails.ToArray();

                for (int index = 0; index < errorDetails.Count; index++)
                {
                    if (errorDetailsPairs[index].Key.StartsWith(MessageKey))
                    {
                        errorMessage = errorDetailsPairs[index].Value[MessageIndex];

                        return true;
                    }
                }
            }

            errorMessage = string.Empty;

            return false;
        }
        #endregion
    }
}