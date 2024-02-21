// © 2023, Worth Systems.

namespace SecretsManager.Constants
{
    /// <summary>
    /// Predefined immutable values distributed all over the application.
    /// </summary>
    internal static class DefaultValues
    {
        internal static class FileNames
        {
            internal static string PrivateKeyPath { get; } = "private_key";

            internal static string JwtTokensPath { get; } = "token.json";
        }
    }
}