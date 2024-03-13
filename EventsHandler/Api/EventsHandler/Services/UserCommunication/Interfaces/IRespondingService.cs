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
    ///   Specialized in processing generic <typeparamref name="TResult"/> and <typeparamref name="TDetails"/>.
    /// </para>
    /// </summary>
    /// <typeparam name="TResult">The generic type of the processing result.</typeparam>
    /// <typeparam name="TDetails">The more insightful details about the processing outcome.</typeparam>
    /// <seealso cref="IRespondingService"/>
    public interface IRespondingService<in TResult, in TDetails> : IRespondingService
    {
        /// <summary>
        /// Gets standardized <see cref="IActionResult"/> based on the received generic <typeparamref name="TResult"/> and <typeparamref name="TDetails"/>.
        /// </summary>
        /// <param name="result">
        ///   <inheritdoc cref="IRespondingService{TResult, TDetails}" path="/typeparam[@name='TResult']"/>
        /// </param>
        /// <param name="details">
        ///   <inheritdoc cref="IRespondingService{TResult, TDetails}" path="/typeparam[@name='TDetails']"/>
        /// </param>
        internal ObjectResult GetStandardized_Processing_ActionResult(TResult result, TDetails details);

        /// <summary>
        /// Gets standardized failure <see cref="IActionResult"/> based on the received <typeparamref name="TDetails"/>.
        /// </summary>
        /// <param name="details">
        ///   <inheritdoc cref="IRespondingService{TResult, TDetails}.GetStandardized_Processing_ActionResult(TResult, TDetails)"
        ///               path="/param[@name='details']"/>
        /// </param>
        internal ObjectResult GetStandardized_Processing_Failed_ActionResult(TDetails details);
    }

    /// <summary>
    /// <inheritdoc cref="IRespondingService{TResult, TDetails}"/>
    /// <para>
    ///   Specialized to be used with <typeparamref name="TModel"/> objects.
    /// </para>
    /// </summary>
    /// <seealso cref="IRespondingService{TResult, TDetails}"/>
    public interface IRespondingService<TModel> : IRespondingService<(ProcessingResult, string), BaseEnhancedDetails>
        where TModel : IJsonSerializable
    {
        /// <inheritdoc cref="IRespondingService{TResult, TDetails}.GetStandardized_Processing_ActionResult(TResult, TDetails)"/>
        internal new ObjectResult GetStandardized_Processing_ActionResult((ProcessingResult Status, string Description) result, BaseEnhancedDetails details);

        /// <inheritdoc cref="IRespondingService{TResult, TDetails}.GetStandardized_Processing_Failed_ActionResult(TDetails)"/>
        internal new ObjectResult GetStandardized_Processing_Failed_ActionResult(BaseEnhancedDetails details);
    }
}