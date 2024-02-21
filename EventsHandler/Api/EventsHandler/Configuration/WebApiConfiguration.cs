// © 2023, Worth Systems.

using EventsHandler.Extensions;
using EventsHandler.Services.DataLoading.Interfaces;
using System.Collections.Concurrent;
using ConfigurationExtensions = EventsHandler.Extensions.ConfigurationExtensions;

namespace EventsHandler.Configuration
{
    /// <summary>
    /// The object to encapsulate <see cref="WebApplication"/> configurations
    /// from "appsettings.json" (public) and "secrets.json" (private).
    /// </summary>
    public sealed record WebApiConfiguration
    {
        /// <summary>
        /// Gets the configuration for Notify NL (internal) system.
        /// </summary>
        internal NotifyComponent Notify { get; }

        /// <summary>
        /// Gets the configuration for the user (external) system.
        /// </summary>
        internal UserComponent User { get; }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="WebApiConfiguration"/> class.
        /// </summary>
        /// <param name="configuration">The deserialized application configuration.</param>
        /// <param name="loadingService">The loading service used to obtain some configurations.</param>
        public WebApiConfiguration(IConfiguration configuration, ILoadingService loadingService)  // NOTE: The only constructor to be used with Dependency Injection
        {
            // Recreating structure of "appsettings.json" or "secrets.json" files to use them later as objects
            this.Notify = new NotifyComponent(configuration, loadingService, nameof(this.Notify));
            this.User = new UserComponent(configuration, loadingService, nameof(this.User));
        }

        /// <summary>
        /// The "Notify" part of the configuration.
        /// </summary>
        internal record NotifyComponent
        {
            /// <summary>
            /// A thread-safe dictionary, storing cached configuration <see langword="string"/> values.
            /// <para>
            ///   The reasons to use such solution are:
            ///   <para>
            ///     - Some values are validated during retrieval time, whether they have correct format or range.
            ///     After being loaded for the first time and then validated, there is no reason to check them again.
            ///   </para>
            ///   <para>
            ///     - The methods used to map specific configurations (like in fluent builder design pattern) is very handy
            ///     in terms of OOP approach, but might introduce some minimal overhead. Thanks to caching values by their
            ///     configuration nodes, both - flexibility and convenience as well as better performance can be achieved.
            ///   </para>
            /// </para>
            /// </summary>
            private static readonly ConcurrentDictionary<
                string /* Config path */,
                string /* Config value */> s_cachedValues = new();

            /// <inheritdoc cref="AuthorizationComponent"/>
            internal AuthorizationComponent Authorization { get; }

            /// <inheritdoc cref="ApiComponent"/>
            internal ApiComponent API { get; }
            
            /// <summary>
            /// Initializes a new instance of the <see cref="NotifyComponent"/> class.
            /// </summary>
            public NotifyComponent(IConfiguration configuration, ILoadingService loadingService, string parentName)
            {
                this.Authorization = new AuthorizationComponent(configuration, loadingService, parentName);
                this.API = new ApiComponent(configuration, loadingService, parentName);
            }

            /// <summary>
            /// The "Authorization" part of the configuration.
            /// </summary>
            internal sealed record AuthorizationComponent
            {
                /// <inheritdoc cref="JwtComponent"/>
                internal JwtComponent JWT { get; }

                /// <inheritdoc cref="KeyComponent"/>
                internal KeyComponent Key { get; }

                /// <summary>
                /// Initializes a new instance of the <see cref="AuthorizationComponent"/> class.
                /// </summary>
                internal AuthorizationComponent(IConfiguration configuration, ILoadingService loadingService, string parentPath)
                {
                    string currentPath = parentPath.GetPathWithNode(nameof(Authorization));

                    this.JWT = new JwtComponent(configuration, loadingService, currentPath);
                    this.Key = new KeyComponent(configuration, loadingService, currentPath);
                }

                /// <summary>
                /// The "JWT" part of the configuration.
                /// </summary>
                internal sealed record JwtComponent
                {
                    private readonly IConfiguration _configuration;
                    private readonly ILoadingService _loadingService;
                    private readonly string _currentPath;

                    /// <summary>
                    /// Initializes a new instance of the <see cref="JwtComponent"/> class.
                    /// </summary>
                    internal JwtComponent(IConfiguration configuration, ILoadingService loadingService, string parentPath)
                    {
                        this._configuration = configuration;
                        this._loadingService = loadingService;
                        this._currentPath = parentPath.GetPathWithNode(nameof(JWT));
                    }

                    /// <inheritdoc cref="ConfigurationExtensions.GetConfigValue(IConfiguration, string)"/>
                    internal string Secret() => Environment.GetEnvironmentVariable("NOTIFY_AUTHORIZATION_JWT_SECRET");
                        //=> GetCachedValue(s_cachedValues, this._configuration, this._currentPath, nameof(Secret));

                    /// <inheritdoc cref="ConfigurationExtensions.GetConfigValue(IConfiguration, string)"/>
                    internal string Issuer() => Environment.GetEnvironmentVariable("NOTIFY_AUTHORIZATION_JWT_ISSUER");
                        //=> GetCachedValue(s_cachedValues, this._configuration, this._currentPath, nameof(Issuer));

                    /// <inheritdoc cref="ConfigurationExtensions.GetConfigValue(IConfiguration, string)"/>
                    internal string Audience() => Environment.GetEnvironmentVariable("NOTIFY_AUTHORIZATION_JWT_AUDIENCE");
                        //=> GetCachedValue(s_cachedValues, this._configuration, this._currentPath, nameof(Audience));

                    /// <inheritdoc cref="ConfigurationExtensions.GetConfigValue(IConfiguration, string)"/>
                    internal ushort ExpiresInMin() => ushort.Parse(Environment.GetEnvironmentVariable("NOTIFY_AUTHORIZATION_JWT_EXPIRESINMIN"));
                        //=> GetCachedValue<ushort>(this._configuration, this._currentPath, nameof(ExpiresInMin));

                    /// <inheritdoc cref="ConfigurationExtensions.GetConfigValue(IConfiguration, string)"/>
                    internal string UserId() => Environment.GetEnvironmentVariable("NOTIFY_AUTHORIZATION_JWT_USERID");
                        //=> GetCachedValue(s_cachedValues, this._configuration, this._currentPath, nameof(UserId));

                    /// <inheritdoc cref="ConfigurationExtensions.GetConfigValue(IConfiguration, string)"/>
                    internal string UserName() => Environment.GetEnvironmentVariable("NOTIFY_AUTHORIZATION_JWT_USERNAME");
                        //=> GetCachedValue(s_cachedValues, this._configuration, this._currentPath, nameof(UserName));
                }

                /// <summary>
                /// The "Key" part of the configuration.
                /// </summary>
                internal sealed record KeyComponent
                {
                    private readonly IConfiguration _configuration;
                    private readonly ILoadingService _loadingService;
                    private readonly string _currentPath;

                    /// <summary>
                    /// Initializes a new instance of the <see cref="KeyComponent"/> class.
                    /// </summary>
                    internal KeyComponent(IConfiguration configuration, ILoadingService loadingService, string parentPath)
                    {
                        this._configuration = configuration;
                        this._loadingService = loadingService;
                        this._currentPath = parentPath.GetPathWithNode(nameof(Key));
                    }

                    /// <inheritdoc cref="ConfigurationExtensions.GetConfigValue(IConfiguration, string)"/>
                    internal string NotifyNL() => Environment.GetEnvironmentVariable("USER_AUTHORIZATION_KEY_NOTIFYNL");
                        //=> GetCachedValue(s_cachedValues, this._configuration, this._currentPath, nameof(NotifyNL));

                    /// <inheritdoc cref="ConfigurationExtensions.GetConfigValue(IConfiguration, string)"/>
                    internal string Objecten() => Environment.GetEnvironmentVariable("USER_AUTHORIZATION_KEY_OBJECTEN");
                        //=> GetCachedValue(s_cachedValues, this._configuration, this._currentPath, nameof(NotifyNL));
                }
            }

            /// <summary>
            /// The "API" part of the configuration.
            /// </summary>
            internal sealed record ApiComponent
            {
                /// <inheritdoc cref="BaseUrlComponent"/>
                internal BaseUrlComponent BaseUrl { get; }

                /// <summary>
                /// Initializes a new instance of the <see cref="ApiComponent"/> class.
                /// </summary>
                internal ApiComponent(IConfiguration configuration, ILoadingService loadingService, string parentPath)
                {
                    string currentPath = parentPath.GetPathWithNode(nameof(API));

                    this.BaseUrl = new BaseUrlComponent(configuration, loadingService, currentPath);
                }

                /// <summary>
                /// The "BaseUrl" part of the configuration.
                /// </summary>
                internal sealed record BaseUrlComponent
                {
                    private readonly IConfiguration _configuration;
                    private readonly ILoadingService _loadingService;
                    private readonly string _currentPath;

                    /// <summary>
                    /// Initializes a new instance of the <see cref="BaseUrlComponent"/> class.
                    /// </summary>
                    internal BaseUrlComponent(IConfiguration configuration, ILoadingService loadingService, string parentPath)
                    {
                        this._configuration = configuration;
                        this._loadingService = loadingService;
                        this._currentPath = parentPath.GetPathWithNode(nameof(BaseUrl));
                    }

                    /// <inheritdoc cref="ConfigurationExtensions.GetConfigValue(IConfiguration, string)"/>
                    internal string NotifyNL() => Environment.GetEnvironmentVariable("NOTIFY_API_BASEURL_NOTIFYNL");
                        //=> GetCachedValue(s_cachedValues, this._configuration, this._currentPath, nameof(NotifyNL));
                }
            }
        }

        /// <summary>
        /// The "Parent" part of the configuration.
        /// </summary>
        internal sealed record UserComponent : NotifyComponent
        {
            /// <inheritdoc cref="DomainComponent"/>
            internal DomainComponent Domain { get; }

            /// <inheritdoc cref="TemplateIdsComponent"/>
            internal TemplateIdsComponent TemplateIds { get; }

            /// <summary>
            /// Initializes a new instance of the <see cref="UserComponent"/> class.
            /// </summary>
            public UserComponent(IConfiguration configuration, ILoadingService loadingService, string parentName)
                : base(configuration, loadingService, parentName)
            {
                this.Domain = new DomainComponent(configuration, loadingService, parentName);
                this.TemplateIds = new TemplateIdsComponent(configuration, loadingService, parentName);
            }

            /// <summary>
            /// The "Domain" part of the configuration.
            /// </summary>
            internal sealed record DomainComponent
            {
                private static readonly ConcurrentDictionary<
                    string /* Config path */,
                    string /* Config value */> s_cachedDomainValues = new();

                private readonly IConfiguration _configuration;
                private readonly ILoadingService _loadingService;
                private readonly string _currentPath;

                /// <summary>
                /// Initializes a new instance of the <see cref="DomainComponent"/> class.
                /// </summary>
                internal DomainComponent(IConfiguration configuration, ILoadingService loadingService, string parentPath)
                {
                    this._configuration = configuration;
                    this._loadingService = loadingService;
                    this._currentPath = parentPath.GetPathWithNode(nameof(Domain));
                }

                /// <inheritdoc cref="ConfigurationExtensions.GetConfigValue(IConfiguration, string)"/>
                internal string OpenNotificaties() => Environment.GetEnvironmentVariable("USER_DOMAIN_OPENNOTIFICATIES");
                    //=> GetCachedDomainValue(s_cachedDomainValues, this._configuration, this._currentPath, nameof(OpenNotificaties));

                /// <inheritdoc cref="ConfigurationExtensions.GetConfigValue(IConfiguration, string)"/>
                internal string OpenZaak() => Environment.GetEnvironmentVariable("USER_DOMAIN_OPENZAAK");
                    //=> GetCachedDomainValue(s_cachedDomainValues, this._configuration, this._currentPath, nameof(OpenZaak));

                /// <inheritdoc cref="ConfigurationExtensions.GetConfigValue(IConfiguration, string)"/>
                internal string OpenKlant() => Environment.GetEnvironmentVariable("USER_DOMAIN_OPENKLANT");
                    //=> GetCachedDomainValue(s_cachedDomainValues, this._configuration, this._currentPath, nameof(OpenKlant));

                /// <inheritdoc cref="ConfigurationExtensions.GetConfigValue(IConfiguration, string)"/>
                internal string Objecten() => Environment.GetEnvironmentVariable("USER_DOMAIN_OBJECTEN");
                    //=> GetCachedDomainValue(s_cachedDomainValues, this._configuration, this._currentPath, nameof(Objecten));

                /// <inheritdoc cref="ConfigurationExtensions.GetConfigValue(IConfiguration, string)"/>
                internal string ObjectTypen() => Environment.GetEnvironmentVariable("USER_DOMAIN_OBJECTTYPEN");
                    //=> GetCachedDomainValue(s_cachedDomainValues, this._configuration, this._currentPath, nameof(ObjectTypen));
            }

            /// <summary>
            /// The "TemplateIds" part of the configuration.
            /// </summary>
            internal sealed record TemplateIdsComponent
            {
                /// <inheritdoc cref="SmsComponent"/>
                internal SmsComponent Sms { get; }

                /// <inheritdoc cref="EmailComponent"/>
                internal EmailComponent Email { get; }

                /// <summary>
                /// Initializes a new instance of the <see cref="TemplateIdsComponent"/> class.
                /// </summary>
                internal TemplateIdsComponent(IConfiguration configuration, ILoadingService loadingService, string parentPath)
                {
                    string currentPath = parentPath.GetPathWithNode(nameof(TemplateIds));

                    this.Sms = new SmsComponent(configuration, loadingService, currentPath);
                    this.Email = new EmailComponent(configuration, loadingService, currentPath);
                }

                /// <summary>
                /// The "Sms" part of the configuration.
                /// </summary>
                internal sealed record SmsComponent
                {
                    private static readonly ConcurrentDictionary<
                        string /* Config path */,
                        string /* Config value */> s_cachedSmsTemplateValues = new();

                    private readonly IConfiguration _configuration;
                    private readonly ILoadingService _loadingService;
                    private readonly string _currentPath;

                    /// <summary>
                    /// Initializes a new instance of the <see cref="SmsComponent"/> class.
                    /// </summary>
                    internal SmsComponent(IConfiguration configuration, ILoadingService loadingService, string parentPath)
                    {
                        this._configuration = configuration;
                        this._loadingService = loadingService;
                        this._currentPath = parentPath.GetPathWithNode(nameof(Sms));
                    }

                    /// <inheritdoc cref="ConfigurationExtensions.GetConfigValue(IConfiguration, string)"/>
                    internal string ZaakCreate() => Environment.GetEnvironmentVariable("USER_TEMPLATEIDS_SMS_ZAAKCREATE");
                        //=> GetCachedTemplateIdValue(s_cachedSmsTemplateValues, this._configuration, this._currentPath, nameof(ZaakCreate));

                    /// <inheritdoc cref="ConfigurationExtensions.GetConfigValue(IConfiguration, string)"/>
                    internal string ZaakUpdate() => Environment.GetEnvironmentVariable("USER_TEMPLATEIDS_SMS_ZAAKUPDATE");
                        //=> GetCachedTemplateIdValue(s_cachedSmsTemplateValues, this._configuration, this._currentPath, nameof(ZaakUpdate));

                    /// <inheritdoc cref="ConfigurationExtensions.GetConfigValue(IConfiguration, string)"/>
                    internal string ZaakClose() => Environment.GetEnvironmentVariable("USER_TEMPLATEIDS_SMS_ZAAKCLOSE");
                        //=> GetCachedTemplateIdValue(s_cachedSmsTemplateValues, this._configuration, this._currentPath, nameof(ZaakClose));
                }

                /// <summary>
                /// The "Email" part of the configuration.
                /// </summary>
                internal sealed record EmailComponent
                {
                    private static readonly ConcurrentDictionary<
                        string /* Config path */,
                        string /* Config value */> s_cachedEmailTemplateValues = new();

                    private readonly IConfiguration _configuration;
                    private readonly ILoadingService _loadingService;
                    private readonly string _currentPath;

                    /// <summary>
                    /// Initializes a new instance of the <see cref="EmailComponent"/> class.
                    /// </summary>
                    internal EmailComponent(IConfiguration configuration, ILoadingService loadingService, string parentPath)
                    {
                        this._configuration = configuration;
                        this._loadingService = loadingService;
                        this._currentPath = parentPath.GetPathWithNode(nameof(Email));
                    }

                    /// <inheritdoc cref="ConfigurationExtensions.GetConfigValue(IConfiguration, string)"/>
                    internal string ZaakCreate() => Environment.GetEnvironmentVariable("USER_TEMPLATEIDS_EMAIL_ZAAKCREATE");
                        //=> GetCachedTemplateIdValue(s_cachedEmailTemplateValues, this._configuration, this._currentPath, nameof(ZaakCreate));

                    /// <inheritdoc cref="ConfigurationExtensions.GetConfigValue(IConfiguration, string)"/>
                    internal string ZaakUpdate() => Environment.GetEnvironmentVariable("USER_TEMPLATEIDS_EMAIL_ZAAKUPDATE");
                        //=> GetCachedTemplateIdValue(s_cachedEmailTemplateValues, this._configuration, this._currentPath, nameof(ZaakUpdate));

                    /// <inheritdoc cref="ConfigurationExtensions.GetConfigValue(IConfiguration, string)"/>
                    internal string ZaakClose() => Environment.GetEnvironmentVariable("USER_TEMPLATEIDS_EMAIL_ZAAKCLOSE");
                        //=> GetCachedTemplateIdValue(s_cachedEmailTemplateValues, this._configuration, this._currentPath, nameof(ZaakClose));
                }
            }
        }

        #region Helper methods
        /// <summary>
        /// Retrieves cached configuration value.
        /// </summary>
        private static string GetCachedValue(
            ConcurrentDictionary<string, string> cachedValues,
            IConfiguration configuration,
            string currentPath,
            string nodeName)
        {
            return cachedValues.GetOrAdd(
                nodeName,
                GetCachedValue<string>(configuration, currentPath, nodeName));
        }

        /// <inheritdoc cref="GetCachedValue(ConcurrentDictionary{string,string}, IConfiguration, string, string)"/>
        private static T GetCachedValue<T>(
            IConfiguration configuration,
            string currentPath,
            string nodeName)
        {
            return configuration.GetConfigValueFromPathWithNode<T>(currentPath, nodeName);
        }

        /// <summary>
        /// Retrieves cached configuration value, ensuring it will be a domain (without http/s and API endpoint).
        /// </summary>
        private static string GetCachedDomainValue(
            ConcurrentDictionary<string, string> cachedValues,
            IConfiguration configuration,
            string currentPath,
            string nodeName)
        {
            return cachedValues.GetOrAdd(
                nodeName,
                configuration.GetConfigValueFromPathWithNode(currentPath, nodeName))
                // NOTE: Validate only once when the value is cached
                .WithoutHttp()
                .WithoutEndpoint();
        }

        /// <summary>
        /// Retrieves cached configuration value, ensuring it will be a valid Template Id.
        /// </summary>
        private static string GetCachedTemplateIdValue(
            ConcurrentDictionary<string, string> cachedValues,
            IConfiguration configuration,
            string currentPath,
            string nodeName)
        {
            return cachedValues.GetOrAdd(
                nodeName,
                configuration.GetConfigValueFromPathWithNode(currentPath, nodeName))
                // NOTE: Validate only once when the value is cached
                .ValidTemplateId();
        }
        #endregion
    }
}