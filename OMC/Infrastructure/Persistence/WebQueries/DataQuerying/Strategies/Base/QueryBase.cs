﻿// © 2024, Worth Systems.

using WebQueries.DataQuerying.Models.Responses;
using WebQueries.DataQuerying.Strategies.Interfaces;
using WebQueries.DataSending.Clients.Enums;
using WebQueries.DataSending.Interfaces;
using WebQueries.Exceptions;
using ZhvModels.Mapping.Models.Interfaces;
using ZhvModels.Mapping.Models.POCOs.NotificatieApi;
using ZhvModels.Serialization.Interfaces;

namespace WebQueries.DataQuerying.Strategies.Base
{
    /// <inheritdoc cref="IQueryBase"/>
    public sealed class QueryBase : IQueryBase
    {
        private readonly ISerializationService _serializer;
        private readonly IHttpNetworkService _networkService;

        /// <inheritdoc cref="IQueryBase.Notification"/>
        NotificationEvent IQueryBase.Notification { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryBase"/> class.
        /// </summary>
        public QueryBase(ISerializationService serializer, IHttpNetworkService networkService)  // Dependency Injection (DI)
        {
            this._serializer = serializer;
            this._networkService = networkService;
        }

        #region Internal methods
        /// <inheritdoc cref="IQueryBase.ProcessGetAsync{TModel}(HttpClientTypes, Uri, string)"/>
        async Task<TModel> IQueryBase.ProcessGetAsync<TModel>(HttpClientTypes httpClientType, Uri uri, string fallbackErrorMessage)
        {
            HttpRequestResponse response = await this._networkService.GetAsync(httpClientType, uri);

            return GetApiResult<TModel>(httpClientType, response.IsSuccess, response.JsonResponse, uri, fallbackErrorMessage);
        }

        /// <inheritdoc cref="IQueryBase.ProcessPostAsync{TModel}(HttpClientTypes, Uri, string, string)"/>
        async Task<TModel> IQueryBase.ProcessPostAsync<TModel>(HttpClientTypes httpClientType, Uri uri, string jsonBody, string fallbackErrorMessage)
        {
            HttpRequestResponse response = await this._networkService.PostAsync(httpClientType, uri, jsonBody);

            return GetApiResult<TModel>(httpClientType, response.IsSuccess, response.JsonResponse, uri, fallbackErrorMessage);
        }
        #endregion

        #region Helper methods
        private TModel GetApiResult<TModel>(HttpClientTypes httpClientType, bool isSuccess, string jsonResult, Uri uri, string fallbackErrorMessage)
            where TModel : struct, IJsonSerializable
        {
            return isSuccess
                ? this._serializer.Deserialize<TModel>(jsonResult)

                // Logging errors
                : httpClientType is HttpClientTypes.Telemetry_Contactmomenten
                                 or HttpClientTypes.Telemetry_Klantinteracties

                    // Soft error: HTTP Status Code 206
                    ? throw new TelemetryException(GetMessage(jsonResult, uri, fallbackErrorMessage))

                    // Hard error: HTTP Status Code 400
                    : throw new HttpRequestException(GetMessage(jsonResult, uri, fallbackErrorMessage));
        }

        private static string GetMessage(string jsonResult, Uri uri, string fallbackErrorMessage)
        {
            return $"{fallbackErrorMessage} | URI: {uri} | JSON response: {jsonResult}";
        }
        #endregion
    }
}