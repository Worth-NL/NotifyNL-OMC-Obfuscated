// © 2023, Worth Systems.

using System.Text.Json.Serialization;

namespace EventsHandler.Behaviors.Responding.Messages.Models.Details.Base
{
    /// <summary>
    /// Standard format how to display details about processed notification (with additional details).
    /// </summary>
    /// <seealso cref="BaseSimpleDetails"/>
    internal abstract record BaseEnhancedDetails : BaseSimpleDetails
    {
        [JsonPropertyName(nameof(Cases))]
        [JsonPropertyOrder(1)]
        public string Cases { get; internal set; } = string.Empty;

        [JsonPropertyName(nameof(Reasons))]
        [JsonPropertyOrder(2)]
        public string[] Reasons { get; internal set; } = Array.Empty<string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseEnhancedDetails"/> class.
        /// </summary>
        protected BaseEnhancedDetails() : base() { }

        /// <inheritdoc cref="BaseEnhancedDetails()"/>
        protected BaseEnhancedDetails(string message, string cases, string[] reasons)
            : base(message)
        {
            this.Cases = cases;
            this.Reasons = reasons;
        }
    }
}