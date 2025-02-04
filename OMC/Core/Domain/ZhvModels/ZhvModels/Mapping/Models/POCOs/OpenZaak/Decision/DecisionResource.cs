﻿// © 2024, Worth Systems.

using Common.Constants;
using System.Text.Json.Serialization;
using ZhvModels.Mapping.Models.Interfaces;

namespace ZhvModels.Mapping.Models.POCOs.OpenZaak.Decision
{
    /// <summary>
    /// The decision resource retrieved from "OpenZaak" Web API service.
    /// </summary>
    /// <seealso cref="IJsonSerializable"/>
    public struct DecisionResource : IJsonSerializable
    {
        /// <summary>
        /// The reference to the <see cref="InfoObject"/> in <see cref="Uri"/> format:
        /// <code>
        /// http(s)://Domain/ApiEndpoint/[UUID]
        /// </code>
        /// </summary>
        [JsonRequired]
        [JsonPropertyName("informatieobject")]
        [JsonPropertyOrder(0)]
        public Uri InfoObjectUri { get; set; } = CommonValues.Default.Models.EmptyUri;

        /// <summary>
        /// The reference to the <see cref="Decision"/> in <see cref="Uri"/> format:
        /// <code>
        /// http(s)://Domain/ApiEndpoint/[UUID]
        /// </code>
        /// </summary>
        [JsonRequired]
        [JsonPropertyName("besluit")]
        [JsonPropertyOrder(1)]
        public Uri DecisionUri { get; set; } = CommonValues.Default.Models.EmptyUri;

        /// <summary>
        /// Initializes a new instance of the <see cref="DecisionResource"/> struct.
        /// </summary>
        public DecisionResource()
        {
        }
    }
}