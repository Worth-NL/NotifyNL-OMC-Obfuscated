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
    public interface IRespondingService
    {
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

    /// <summary>
    /// <inheritdoc cref="IRespondingService"/>
    /// <para>
    ///   Specialized in processing generic <typeparamref name="TResult"/> and <see cref="BaseEnhancedDetails"/>.
    /// </para>
    /// </summary>
    /// <typeparam name="TResult">The generic type of the result.</typeparam>
    public interface IRespondingService<in TResult> : IRespondingService
    {
        /// <summary>
        /// Gets standardized <see cref="IActionResult"/> based on the received generic <typeparamref name="TResult"/> and <see cref="BaseEnhancedDetails"/>.
        /// </summary>
        /// <param name="result">
        ///   <inheritdoc cref="IRespondingService{TResult}" path="/typeparam[@name='TResult']"/>
        /// </param>
        /// <param name="notificationDetails">The more insightful details about the processing outcome.</param>
        internal ObjectResult GetStandardized_Processing_ActionResult(TResult result, BaseEnhancedDetails notificationDetails);

        /// <summary>
        /// Gets standardized failure <see cref="IActionResult"/> based on the received <see cref="BaseEnhancedDetails"/>.
        /// </summary>
        /// <param name="notificationDetails">
        ///   <inheritdoc cref="IRespondingService{TResult}.GetStandardized_Processing_ActionResult(TResult, BaseEnhancedDetails)"
        ///               path="/param[@name='notificationDetails']"/>
        /// </param>
        internal ObjectResult GetStandardized_Processing_Failed_ActionResult(BaseEnhancedDetails notificationDetails);
    }
}