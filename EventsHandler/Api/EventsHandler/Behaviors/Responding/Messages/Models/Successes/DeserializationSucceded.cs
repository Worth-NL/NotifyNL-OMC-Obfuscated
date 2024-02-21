// © 2023, Worth Systems.

using EventsHandler.Behaviors.Mapping.Models.POCOs.NotificatieApi;
using EventsHandler.Behaviors.Responding.Messages.Models.Base;
using EventsHandler.Properties;
using System.Net;

namespace EventsHandler.Behaviors.Responding.Messages.Models.Successes
{
    /// <summary>
    /// Serialization of <see cref="NotificationEvent"/> was successful.
    /// </summary>
    /// <seealso cref="BaseApiStandardResponseBody"/>
    internal sealed class DeserializationSucceded : BaseApiStandardResponseBody
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeserializationSucceded"/> class.
        /// </summary>
        internal DeserializationSucceded()
            : base(HttpStatusCode.OK, Resources.Operation_RESULT_Deserialization_Success)
        {
        }
    }
}