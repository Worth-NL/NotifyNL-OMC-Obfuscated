// © 2023, Worth Systems.

using EventsHandler.Behaviors.Responding.Messages.Models.Base;
using EventsHandler.Behaviors.Responding.Messages.Models.Details.Base;
using System.Net;

namespace EventsHandler.Behaviors.Responding.Messages.Models.Successes
{
    /// <summary>
    /// Processing of notification was successful.
    /// </summary>
    /// <seealso cref="BaseEnhancedStandardResponseBody"/>
    internal sealed class ProcessingSucceeded : BaseEnhancedStandardResponseBody
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessingSucceeded"/> class.
        /// </summary>
        /// <param name="processingResult">The description of the notification processing result.</param>
        /// <param name="details">The details to be included.</param>
        internal ProcessingSucceeded(string processingResult, BaseEnhancedDetails details)
            : base(HttpStatusCode.Accepted, processingResult, details)
        {
        }
    }
}