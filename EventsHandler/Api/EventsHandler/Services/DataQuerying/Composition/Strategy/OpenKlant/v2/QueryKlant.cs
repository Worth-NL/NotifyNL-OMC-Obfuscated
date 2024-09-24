﻿// © 2024, Worth Systems.

using EventsHandler.Mapping.Models.POCOs.OpenKlant;
using EventsHandler.Mapping.Models.POCOs.OpenKlant.Converters;
using EventsHandler.Mapping.Models.POCOs.OpenKlant.v2;
using EventsHandler.Services.DataQuerying.Composition.Interfaces;
using EventsHandler.Services.DataQuerying.Composition.Strategy.OpenKlant.Interfaces;
using EventsHandler.Services.DataSending.Clients.Enums;
using EventsHandler.Services.DataSending.Interfaces;
using EventsHandler.Services.DataSending.Responses;
using EventsHandler.Services.Settings.Configuration;
using EventsHandler.Services.Versioning.Interfaces;
using Resources = EventsHandler.Properties.Resources;

namespace EventsHandler.Services.DataQuerying.Composition.Strategy.OpenKlant.v2
{
    /// <inheritdoc cref="IQueryKlant"/>
    /// <remarks>
    ///   Version: "OpenKlant" (v2+) Web API service | "OMC workflow" v2.
    /// </remarks>
    /// <seealso cref="IVersionDetails"/>
    internal sealed class QueryKlant : IQueryKlant
    {
        /// <inheritdoc cref="IQueryKlant.Configuration"/>
        WebApiConfiguration IQueryKlant.Configuration { get; set; } = null!;

        /// <inheritdoc cref="IVersionDetails.Version"/>
        string IVersionDetails.Version => "2.0.0";

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryKlant"/> class.
        /// </summary>
        public QueryKlant(WebApiConfiguration configuration)
        {
            ((IQueryKlant)this).Configuration = configuration;
        }

        #region Polymorphic (Citizen details)
        /// <inheritdoc cref="IQueryKlant.TryGetPartyDataAsync(IQueryBase, string, string)"/>
        async Task<CommonPartyData> IQueryKlant.TryGetPartyDataAsync(IQueryBase queryBase, string openKlantDomain, string bsnNumber)
        {
            // TODO: BSN number validation

            // Predefined URL components
            string partiesEndpoint = $"https://{openKlantDomain}/klantinteracties/api/v1/partijen";

            string partyIdentifier = ((IQueryKlant)this).Configuration.AppSettings.Variables.PartyIdentifier();
            string partyCodeTypeParameter = $"?partijIdentificator__codeSoortObjectId={partyIdentifier}";
            string partyObjectIdParameter = $"&partijIdentificator__objectId={bsnNumber}";
            const string expandParameter = "&expand=digitaleAdressen";

            // Request URL
            Uri partiesByTypeIdAndExpand = new($"{partiesEndpoint}{partyCodeTypeParameter}{partyObjectIdParameter}{expandParameter}");

            return (await GetCitizenDetailsV2Async(queryBase, partiesByTypeIdAndExpand))
                .Party(((IQueryKlant)this).Configuration)
                .ConvertToUnified();
        }

        private static async Task<PartyResults> GetCitizenDetailsV2Async(IQueryBase queryBase, Uri partyUri)
        {
            return await queryBase.ProcessGetAsync<PartyResults>(
                httpClientType: HttpClientTypes.OpenKlant_v2,
                uri: partyUri,  // Request URL
                fallbackErrorMessage: Resources.HttpRequest_ERROR_NoCitizenDetails);
        }
        #endregion

        #region Polymorphic (Telemetry)
        /// <inheritdoc cref="IQueryKlant.CreateContactMomentAsync(IQueryBase, string, string)"/>
        async Task<ContactMoment> IQueryKlant.CreateContactMomentAsync(IQueryBase queryBase, string openKlantDomain, string jsonBody)
        {
            // Predefined URL components
            Uri klantContactMomentUri = new($"https://{openKlantDomain}/klantinteracties/api/v1/klantcontacten");

            // Sending the request
            return await queryBase.ProcessPostAsync<ContactMoment>(
                httpClientType: HttpClientTypes.Telemetry_Klantinteracties,
                uri: klantContactMomentUri,  // Request URL
                jsonBody,
                fallbackErrorMessage: Resources.HttpRequest_ERROR_NoFeedbackKlant);
        }

        /// <inheritdoc cref="IQueryKlant.LinkCaseToContactMomentAsync(IHttpNetworkService, string, string)"/>
        async Task<RequestResponse> IQueryKlant.LinkCaseToContactMomentAsync(IHttpNetworkService networkService, string openKlantDomain, string jsonBody)
        {
            // Predefined URL components
            Uri objectContactMomentUri = new($"https://{openKlantDomain}/klantinteracties/api/v1/onderwerpobjecten");
            
            // Sending the request
            return await networkService.PostAsync(
                httpClientType: HttpClientTypes.Telemetry_Klantinteracties,
                uri: objectContactMomentUri,  // Request URL
                jsonBody);
        }

        /// <inheritdoc cref="IQueryKlant.LinkCustomerToContactMomentAsync(IHttpNetworkService, string, string)"/>
        async Task<RequestResponse> IQueryKlant.LinkCustomerToContactMomentAsync(IHttpNetworkService networkService, string openKlantDomain, string jsonBody)
        {
            // Predefined URL components
            Uri customerContactMomentUri = new($"https://{openKlantDomain}/betrokkenen");

            // Sending the request
            return await networkService.PostAsync(
                httpClientType: HttpClientTypes.Telemetry_Klantinteracties,
                uri: customerContactMomentUri,  // Request URL
                jsonBody);
        }
        #endregion
    }
}