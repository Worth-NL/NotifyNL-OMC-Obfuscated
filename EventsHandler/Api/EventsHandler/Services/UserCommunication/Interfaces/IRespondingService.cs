// © 2023, Worth Systems.

using EventsHandler.Behaviors.Mapping.Enums;
using EventsHandler.Behaviors.Mapping.Models.Interfaces;
using EventsHandler.Behaviors.Responding.Messages.Models.Details.Base;
using Microsoft.AspNetCore.Mvc;

namespace EventsHandler.Services.UserCommunication.Interfaces
{
    /// <summary>
    /// The service producing human user-friendly <see cref="IActionResult"/> API responses.
    /// </summary>
    public interface IRespondingService<TModel> where TModel : IJsonSerializable
    {
        /// <summary>
        /// Gets standardized <see cref="IActionResult"/> for a sunny path (the notification was recognized and sent).
        /// </summary>
        /// <param name="result">The result status + result description.</param>
        /// <param name="notificationDetails">The details from validated business POCO model.</param>
        internal ObjectResult GetStandardized_Processing_ActionResult((ProcessingResult Status, string Description) result, BaseEnhancedDetails notificationDetails);

        /// <summary>
        /// Gets standardized <see cref="IActionResult"/> based on the state of <typeparamref name="TModel"/>.
        /// </summary>
        /// <param name="notificationDetails">The details from validated business POCO model.</param>
        internal ObjectResult GetStandardized_Processing_Failed_ActionResult(BaseEnhancedDetails notificationDetails);

        /// <summary>
        /// Gets standardized <see cref="IActionResult"/> based on the received <see cref="Exception"/>.
        /// </summary>
        /// <param name="exception">The handled program exception.</param>
        internal ObjectResult GetStandardized_Exception_ActionResult(Exception exception);

        /// <summary>
        /// Gets standardized <see cref="IActionResult"/> based on the received error message.
        /// </summary>
        /// <param name="errorMessage">
        ///   The message (it can be a handled <see cref="Exception"/> message or intercepted validation error message).
        /// </param>
        internal ObjectResult GetStandardized_Exception_ActionResult(string errorMessage);
    }
}