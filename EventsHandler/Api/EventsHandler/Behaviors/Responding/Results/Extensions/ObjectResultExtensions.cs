// © 2023, Worth Systems.

using EventsHandler.Behaviors.Responding.Messages.Models.Base;
using EventsHandler.Behaviors.Responding.Messages.Models.Details.Base;
using EventsHandler.Behaviors.Responding.Messages.Models.Errors;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EventsHandler.Behaviors.Responding.Results.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="ObjectResult"/>s.
    /// </summary>
    internal static class ObjectResultExtensions
    {
        /// <summary>
        /// Creates <see cref="HttpStatusCode.Accepted"/>
        /// </summary>
        /// <param name="response">The specific custom response to be passed into <see cref="IActionResult"/>.</param>
        internal static ObjectResult AsResult_202(this BaseEnhancedStandardResponseBody response)
        {
            return new ObjectResult(response)
            {
                StatusCode = StatusCodes.Status202Accepted
            };
        }

        /// <summary>
        /// Creates <see cref="HttpStatusCode.PartialContent"/>
        /// </summary>
        /// <param name="response">The specific custom response to be passed into <see cref="IActionResult"/>.</param>
        internal static ObjectResult AsResult_206(this BaseEnhancedStandardResponseBody response)
        {
            return new ObjectResult(response)
            {
                StatusCode = StatusCodes.Status206PartialContent
            };
        }

        /// <summary>
        /// Creates <see cref="HttpStatusCode.BadRequest"/>
        /// </summary>
        /// <param name="response">The specific custom response to be passed into <see cref="IActionResult"/>.</param>
        internal static ObjectResult AsResult_400(this BaseApiStandardResponseBody response)
        {
            return new BadRequestObjectResult(response);
        }

        /// <summary>
        /// Creates <see cref="HttpStatusCode.BadRequest"/>
        /// </summary>
        /// <param name="errorDetails">The error details to be passed into <see cref="IActionResult"/>.</param>
        internal static ObjectResult AsResult_400(this BaseEnhancedDetails errorDetails)
        {
            return new BadRequestObjectResult(new HttpRequestFailed(errorDetails));
        }

        /// <summary>
        /// Creates <see cref="HttpStatusCode.UnprocessableEntity"/>
        /// </summary>
        /// <param name="response">The specific custom response to be passed into <see cref="IActionResult"/>.</param>
        internal static ObjectResult AsResult_422(this BaseEnhancedStandardResponseBody response)
        {
            return new UnprocessableEntityObjectResult(response);
        }

        /// <summary>
        /// Creates <see cref="HttpStatusCode.UnprocessableEntity"/>
        /// </summary>
        /// <param name="errorDetails">The error details to be passed into <see cref="IActionResult"/>.</param>
        internal static ObjectResult AsResult_422(this BaseEnhancedDetails errorDetails)
        {
            return new UnprocessableEntityObjectResult(new DeserializationFailed(errorDetails));
        }

        /// <summary>
        /// Creates <see cref="HttpStatusCode.InternalServerError"/>
        /// </summary>
        /// <param name="details">The specific custom details to be passed into <see cref="IActionResult"/>.</param>
        internal static ObjectResult AsResult_500(this BaseSimpleDetails details)
        {
            return new ObjectResult(new InternalError(details))
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }

        /// <summary>
        /// Creates <see cref="HttpStatusCode.NotImplemented"/>
        /// </summary>
        internal static ObjectResult AsResult_501()
        {
            return new ObjectResult(new NotImplemented())
            {
                StatusCode = StatusCodes.Status501NotImplemented
            };
        }
    }
}