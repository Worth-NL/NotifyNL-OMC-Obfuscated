﻿// © 2023, Worth Systems.

using EventsHandler.Configuration;
using EventsHandler.Constants;
using EventsHandler.Properties;
using EventsHandler.Services.DataReceiving.Enums;
using EventsHandler.Services.DataReceiving.Factories.Interfaces;
using EventsHandler.Services.DataReceiving.Interfaces;
using Microsoft.IdentityModel.Tokens;
using SecretsManager.Services.Authentication.Encryptions.Strategy.Context;
using System.Collections.Concurrent;
using System.Net.Http.Headers;

namespace EventsHandler.Services.DataReceiving
{
    /// <inheritdoc cref="IHttpNetworkService"/>
    internal sealed class HttpNetworkService : IHttpNetworkService
    {
        private static readonly object s_padlock = new();

        private readonly WebApiConfiguration _configuration;
        private readonly EncryptionContext _encryptionContext;
        private readonly IHttpClientFactory<HttpClient, (string, string)[]> _httpClientFactory;
        private readonly SemaphoreSlim _semaphore;

        /// <summary>
        /// Cached reusable HTTP Clients with preconfigured settings (etc., "Authorization" or "Headers").
        /// </summary>
        private readonly ConcurrentDictionary<HttpClientTypes, HttpClient> _httpClients = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpNetworkService"/> class.
        /// </summary>
        public HttpNetworkService(
            WebApiConfiguration configuration,
            EncryptionContext encryptionContext,
            IHttpClientFactory<HttpClient, (string, string)[]> httpClientFactory)
        {
            this._configuration = configuration;
            this._encryptionContext = encryptionContext;
            this._httpClientFactory = httpClientFactory;

            // NOTE: Prevents "TaskCanceledException" in case there is a lot of simultaneous HTTP Requests being called
            this._semaphore = new SemaphoreSlim(
                this._configuration.AppSettings.Network.HttpRequestsSimultaneousNumber());
            
            // NOTE: This method is working like IHttpClientFactory: builder.Services.AddHttpClient("type_1", client => { });
            InitializeAvailableHttpClients();
        }

        #region Internal methods
        /// <inheritdoc cref="IHttpNetworkService.GetAsync(HttpClientTypes, Uri)"/>
        async Task<(bool Success, string JsonResponse)> IHttpNetworkService.GetAsync(HttpClientTypes httpClientType, Uri uri)
        {
            return await ExecuteCallAsync(httpClientType, uri);
        }

        /// <inheritdoc cref="IHttpNetworkService.PostAsync(HttpClientTypes, Uri, HttpContent)"/>
        async Task<(bool Success, string JsonResponse)> IHttpNetworkService.PostAsync(HttpClientTypes httpClientType, Uri uri, HttpContent body)
        {
            return await ExecuteCallAsync(httpClientType, uri, body);
        }
        #endregion

        #region HTTP Clients
        private void InitializeAvailableHttpClients()
        {
            this._httpClients.TryAdd(HttpClientTypes.OpenZaak_v1, this._httpClientFactory
                .GetHttpClient(new[] { ("Accept-Crs", "EPSG:4326"), ("Content-Crs", "EPSG:4326") }));

            this._httpClients.TryAdd(HttpClientTypes.OpenKlant_v1, this._httpClientFactory
                .GetHttpClient(new[] { ("Accept-Crs", "EPSG:4326"), ("Content-Crs", "EPSG:4326") }));

            this._httpClients.TryAdd(HttpClientTypes.OpenKlant_v2, this._httpClientFactory
                .GetHttpClient(new[] { ("Authorization", AuthorizeWithStaticToken(HttpClientTypes.OpenKlant_v2)) }));

            this._httpClients.TryAdd(HttpClientTypes.Telemetry_ContactMomenten, this._httpClientFactory
                .GetHttpClient(new[] { ("X-NLX-Logrecord-ID", ""), ("X-Audit-Toelichting", "") }));  // TODO: Put the right value here
        }

        /// <summary>
        /// Resolves a specific type of cached <see cref="HttpClient"/> or add a new one if it's not existing.
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        private HttpClient ResolveClient(HttpClientTypes httpClientType)
        {
            return httpClientType switch
            {
                // Clients requiring JWT token to be refreshed
                HttpClientTypes.OpenZaak_v1  or
                HttpClientTypes.OpenKlant_v1 or
                HttpClientTypes.Telemetry_ContactMomenten
                    => AuthorizeWithGeneratedJwt(this._httpClients.GetValueOrDefault(httpClientType)!),
                    
                // Clients using static tokens from configuration
                HttpClientTypes.OpenKlant_v2
                    => this._httpClients.GetValueOrDefault(httpClientType)!,
                
                _ => throw new ArgumentException(
                    $"{Resources.Authorization_ERROR_HttpClientTypeNotSuported} {httpClientType}"),
            };
        }
        #endregion

        #region Authorization
        /// <summary>
        /// Adds generated JSON Web Token to "Authorization" part of the HTTP Request.
        /// </summary>
        /// <remarks>
        /// The token will be initialized for the first time and then refreshed if it's time expired.
        /// </remarks>
        /// <returns>
        /// The source <see cref="HttpClient"/> with updated "Authorization" header.
        /// </returns>
        private HttpClient AuthorizeWithGeneratedJwt(HttpClient httpClient)
        {
            lock (s_padlock)
            {
                // TODO: Caching the token until the expiration time doesn't elapse yet
                SecurityKey securityKey = this._encryptionContext.GetSecurityKey(
                    this._configuration.User.Authorization.JWT.Secret());

                // Preparing JWT token
                string jwtToken = this._encryptionContext.GetJwtToken(securityKey,
                    this._configuration.User.Authorization.JWT.Issuer(),
                    this._configuration.User.Authorization.JWT.Audience(),
                    this._configuration.User.Authorization.JWT.ExpiresInMin(),
                    this._configuration.User.Authorization.JWT.UserId(),
                    this._configuration.User.Authorization.JWT.UserName());

                // Set Authorization header
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    DefaultValues.Authorization.OpenApiSecurityScheme.BearerSchema, jwtToken);

                return httpClient;
            }
        }

        /// <summary>
        /// Gets static token for "Headers" part of the HTTP Request.
        /// </summary>
        /// <remarks>
        /// The token will be got from a specific setting defined (per API service) in <see cref="WebApiConfiguration"/>.
        /// </remarks>
        /// <returns>
        /// The Key and Value of the "Header" used for authorization purpose.
        /// </returns>
        private string AuthorizeWithStaticToken(HttpClientTypes httpClientType)
        {
            return httpClientType switch
            {
                HttpClientTypes.OpenKlant_v2
                    => $"{DefaultValues.Authorization.Static.Token} {this._configuration.User.API.Key.OpenKlant_2()}",

                _ => throw new ArgumentException(
                    $"{Resources.Authorization_ERROR_HttpClientTypeNotSuported} {httpClientType}")
            };
        }
        #endregion

        #region HTTP Requests
        /// <summary>
        /// Executes the standard safety procedure before and after making the HTTP Request.
        /// </summary>
        private async Task<(bool Success, string JsonResponse)> ExecuteCallAsync(HttpClientTypes httpClientType, Uri uri, HttpContent? body = default)
        {
            try
            {
                // HTTPS protocol validation
                if (uri.Scheme != DefaultValues.Request.HttpsProtocol)
                {
                    return (false, Resources.HttpRequest_ERROR_HttpsProtocolExpected);
                }

                // TODO: To be removed after tests
                if (httpClientType == HttpClientTypes.OpenKlant_v2)
                {
                    uri = new Uri(uri.AbsoluteUri.Replace("https", "http"));
                }
                
                // Determine whether GET or POST call should be sent (depends on if HTTP body is required)
                await this._semaphore.WaitAsync();
                HttpResponseMessage result = body is null
                    // NOTE: This method is working as IHttpClientFactory: _httpClientFactory.CreateClient("type_1");
                    ? await ResolveClient(httpClientType).GetAsync(uri)
                    : await ResolveClient(httpClientType).PostAsync(uri, body);
                this._semaphore.Release();

                return (result.IsSuccessStatusCode, await result.Content.ReadAsStringAsync());  // Status + JSON response
            }
            catch (Exception exception)
            {
                return (false, exception.Message);
            }
        }
        #endregion
    }
}