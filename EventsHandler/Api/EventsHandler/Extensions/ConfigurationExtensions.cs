// © 2023, Worth Systems.

using EventsHandler.Configuration;
using EventsHandler.Constants;
using EventsHandler.Properties;
using System.Text.RegularExpressions;

namespace EventsHandler.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="IConfiguration"/> used in <see cref="Program"/>.cs
    /// where the <see cref="WebApiConfiguration"/> service is not existing yet so, it could
    /// not be retrieved from <see cref="IServiceCollection"/>.
    /// </summary>
    internal static partial class ConfigurationExtensions  // NOTE: "partial" is introduced by the new RegEx generation approach
    {
        private static readonly string s_worthJwtPath =
            $"{nameof(WebApiConfiguration.Notify)}" +
            $"{GetNodePath(nameof(WebApiConfiguration.Notify.Authorization))}" +
            $"{GetNodePath(nameof(WebApiConfiguration.Notify.Authorization.JWT))}";
    
        /// <summary>
        /// Gets the <see cref="LogLevel"/> defined for Application Insights (Azure service).
        /// </summary>
        /// <param name="configuration">The configuration manager to be used.</param>
        /// <returns>
        ///   The value of <see cref="LogLevel"/>.
        /// </returns>
        internal static LogLevel GetApplicationInsightsLogLevel(this IConfiguration configuration)
        {
            return configuration.GetConfigValue<LogLevel>(
                $"Logging" +
                $"{GetNodePath("ApplicationInsights")}" +
                $"{GetNodePath("LogLevel")}" +
                $"{GetNodePath("Default")}");
        }

        /// <inheritdoc cref="GetConfigValue(IConfiguration, string)"/>
        internal static bool IsEncryptionAsymmetric(this IConfiguration configuration)
            => configuration.GetConfigValue<bool>($"Encryption{GetNodePath("IsAsymmetric")}");

        /// <inheritdoc cref="GetConfigValue(IConfiguration, string)"/>
        internal static string GetWorthJwtSecret(this IConfiguration configuration) => Environment.GetEnvironmentVariable("NOTIFY_AUTHORIZATION_JWT_SECRET");
            //=> configuration.GetConfigValue($"{s_worthJwtPath}{GetNodePath(nameof(WebApiConfiguration.Notify.Authorization.JWT.Secret))}");

        /// <inheritdoc cref="GetConfigValue(IConfiguration, string)"/>
        internal static string GetWorthJwtIssuer(this IConfiguration configuration) => Environment.GetEnvironmentVariable("NOTIFY_AUTHORIZATION_JWT_ISSUER");
            //=> configuration.GetConfigValue($"{s_worthJwtPath}{GetNodePath(nameof(WebApiConfiguration.Notify.Authorization.JWT.Issuer))}");

        /// <inheritdoc cref="GetConfigValue(IConfiguration, string)"/>
        internal static string GetWorthJwtAudience(this IConfiguration configuration) => Environment.GetEnvironmentVariable("NOTIFY_AUTHORIZATION_JWT_AUDIENCE");
            //=> configuration.GetConfigValue($"{s_worthJwtPath}{GetNodePath(nameof(WebApiConfiguration.Notify.Authorization.JWT.Audience))}");

        /// <summary>
        /// Gets the <typeparamref name="T"/> value from the configuration.
        /// </summary>
        /// <exception cref="ArgumentException"/>
        internal static T GetConfigValue<T>(this IConfiguration configuration, string path)
        {
            if (typeof(T) == typeof(string))  // NOTE: To cover string value cases however, faster would be dedicated string method
            {
                return (T)Convert.ChangeType(configuration.GetConfigValue(path), typeof(T));
            }

            return configuration.GetValue<T>(path) ??
                throw new ArgumentException(Resources.Configuration_ERROR_ValueNotFoundOrEmpty + Separated(path));
        }

        /// <summary>
        /// Gets the <see langword="string"/> value from the configuration.
        /// </summary>
        /// <exception cref="ArgumentException"/>
        internal static string GetConfigValue(this IConfiguration configuration, string path)
        {
            string? stringValue = configuration.GetValue<string>(path);

            return string.IsNullOrWhiteSpace(stringValue)
                ? throw new ArgumentException(Resources.Configuration_ERROR_ValueNotFoundOrEmpty + Separated(path))
                : stringValue;
        }

        /// <summary>
        /// Ensures that the value from configuration file does not contain "http" or "https" protocol.
        /// </summary>
        /// <exception cref="ArgumentException"/>
        internal static string WithoutHttp(this string value)
        {
            return !value.StartsWith(DefaultValues.Request.HttpProtocol)
                ? value
                : throw new ArgumentException(Resources.Configuration_ERROR_ContainsHttp + Separated(value));
        }

        /// <summary>
        /// Ensures that the value from configuration file does not contain API endpoint.
        /// </summary>
        /// <exception cref="ArgumentException"/>
        internal static string WithoutEndpoint(this string value)
        {
            return !value.Contains('/')
                ? value
                : throw new ArgumentException(Resources.Configuration_ERROR_ContainsEndpoint + Separated(value));
        }

        [GeneratedRegex(@"^\w{8}\-\w{4}\-\w{4}\-\w{4}\-\w{12}$", RegexOptions.Compiled)]
        private static partial Regex TemplateIdRegex();

        /// <summary>
        /// Ensures that the value from configuration file conforms format of Template Id.
        /// </summary>
        /// <exception cref="ArgumentException"/>
        internal static string ValidTemplateId(this string value)
        {
            return TemplateIdRegex().Match(value).Success
                ? value
                : throw new ArgumentException(Resources.Configuration_ERROR_InvalidTemplateId + Separated(value));
        }

        private static string Separated(string text) => $" {text}";
        
        internal static string GetConfigValueFromPathWithNode(this IConfiguration configuration, string currentPath, string nodeName)
            => configuration.GetConfigValue(GetPathWithNode(currentPath, nodeName));
        
        internal static T GetConfigValueFromPathWithNode<T>(this IConfiguration configuration, string currentPath, string nodeName)
            => configuration.GetConfigValue<T>(GetPathWithNode(currentPath, nodeName));
    }
}