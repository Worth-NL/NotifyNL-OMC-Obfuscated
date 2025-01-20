// © 2025, Worth Systems.

using System.Text.Json.Serialization;
using ZhvModels.Mapping.Models.Interfaces;
using ZhvModels.Mapping.Models.POCOs.OpenZaak;

namespace ZhvModels.Mapping.Models.POCOs.OpenProduct
{
    /// <summary>
    /// The type of the <see cref="Product"/> retrieved from "OpenProduct" Web API service.
    /// </summary>
    /// <seealso cref="IJsonSerializable"/>
    public struct ProductType : IJsonSerializable
    {
        /// <summary>
        /// The name of the <see cref="ProductType"/>.
        /// </summary>
        [JsonRequired]
        [JsonPropertyName("omschrijving")]
        [JsonPropertyOrder(0)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// The description of the <see cref="ProductType"/>.
        /// </summary>
        [JsonRequired]
        [JsonPropertyName("omschrijvingGeneriek")]
        [JsonPropertyOrder(1)]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="CaseType"/> struct.
        /// </summary>
        public ProductType()
        {
        }
    }
}
