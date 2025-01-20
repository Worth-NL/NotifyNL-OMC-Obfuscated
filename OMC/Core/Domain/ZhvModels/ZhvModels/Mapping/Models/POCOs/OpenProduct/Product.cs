// © 2025, Worth Systems.

using Common.Constants;
using System.Text.Json.Serialization;
using ZhvModels.Mapping.Models.Interfaces;

namespace ZhvModels.Mapping.Models.POCOs.OpenProduct
{
    /// <summary>
    /// The product retrieved from "OpenProduct" Web API service.
    /// </summary>
    /// <seealso cref="IJsonSerializable"/>
    public struct Product : IJsonSerializable
    {
        /// <summary>
        /// The reference to the <see cref="Product"/> in <see cref="System.Uri"/> format:
        /// <code>
        /// http(s)://Domain/ApiEndpoint/[UUID]
        /// </code>
        /// </summary>
        [JsonRequired]
        [JsonPropertyName("url")]
        [JsonPropertyOrder(0)]
        public Uri Uri { get; set; } = CommonValues.Default.Models.EmptyUri;

        /// <summary>
        /// The identification of the <see cref="Product"/> in the following format:
        /// <code>
        /// ZAAK-2023-0000000010
        /// </code>
        /// </summary>
        [JsonRequired]
        [JsonPropertyName("identificatie")]
        [JsonPropertyOrder(1)]
        public string Identification { get; set; } = string.Empty;

        /// <summary>
        /// The name of the <see cref="Product"/>.
        /// </summary>
        [JsonRequired]
        [JsonPropertyName("omschrijving")]
        [JsonPropertyOrder(2)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// The BSN (citizen service number) of the citizen pertaining the <see cref="Product"/>.
        /// </summary>
        [JsonRequired]
        [JsonPropertyName("Bsn")]
        [JsonPropertyOrder(3)]
        public string Bsn { get; set; } = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="Product"/> struct.
        /// </summary>
        public Product()
        {
        }
    }
}
