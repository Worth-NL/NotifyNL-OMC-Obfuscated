﻿// © 2023, Worth Systems.

using EventsHandler.Mapping.Models.Interfaces;
using System.Text.Json.Serialization;

namespace EventsHandler.Mapping.Models.POCOs.OpenZaak
{
    /// <summary>
    /// The case retrieved from "OpenZaak" Web API service.
    /// </summary>
    /// <seealso cref="IJsonSerializable"/>
    public struct Case : IJsonSerializable
    {
        /// <summary>
        /// The identification of the <see cref="Case"/> which is an equivalent to the <see cref="Case"/> number.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("identificatie")]
        [JsonPropertyOrder(0)]
        public string Identification { get; internal set; } = string.Empty;

        /// <summary>
        /// The description of the <see cref="Case"/> which is an equivalent of its name.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("omschrijving")]
        [JsonPropertyOrder(1)]
        public string Name { get; internal set; } = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="Case"/> struct.
        /// </summary>
        public Case()
        {
        }
    }
}