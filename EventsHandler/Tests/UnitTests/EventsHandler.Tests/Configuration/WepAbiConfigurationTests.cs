// © 2023, Worth Systems.

using EventsHandler.Configuration;
using EventsHandler.Properties;
using EventsHandler.Services.DataLoading.Interfaces;
using EventsHandler.Utilities._TestHelpers;
using Moq;

namespace EventsHandler.UnitTests.Configuration
{
    [TestFixture]
    public sealed class WepAbiConfigurationTests
    {
        [Ignore("Hack"), Test]
        public void InConfig_ExistingProperties_AllAreMapped()
        {
            // Assert
            Assert.Multiple(() =>
            {
                #pragma warning disable IDE0008  // Explicit types are too long and not necessary to be used here
                // ReSharper disable SuggestVarOrType_SimpleTypes
                var configuration = new WebApiConfiguration(ConfigurationHandler.GetConfiguration(), new Mock<ILoadingService>().Object);

                // Authorization | JWT
                var jwt = configuration.Notify.Authorization.JWT;
                Assert.That(jwt.Secret(), Is.Not.Null.Or.Empty);
                Assert.That(jwt.Issuer(), Is.Not.Null.Or.Empty);
                Assert.That(jwt.Audience(), Is.Not.Null.Or.Empty);
                Assert.That(jwt.ExpiresInMin(), Is.Not.Zero);
                Assert.That(jwt.UserId(), Is.Not.Null.Or.Empty);
                Assert.That(jwt.UserName(), Is.Not.Null.Or.Empty);

                // API | BaseUrl
                var apiBaseUrl = configuration.Notify.API.BaseUrl;
                Assert.That(apiBaseUrl.NotifyNL(), Is.Not.Null.Or.Empty);

                // Authorization | Key
                var key = configuration.User.Authorization.Key;
                Assert.That(key.NotifyNL(), Is.Not.Null.Or.Empty);
                Assert.That(key.Objecten(), Is.Not.Null.Or.Empty);

                // API | Domain
                var apiDomain = configuration.User.Domain;
                Assert.That(apiDomain.OpenNotificaties(), Is.Not.Null.Or.Empty);
                Assert.That(apiDomain.OpenZaak(), Is.Not.Null.Or.Empty);
                Assert.That(apiDomain.OpenKlant(), Is.Not.Null.Or.Empty);  // NOTE: Objecten and ObjectTypen are invalid on purpose; they are covered by another test

                // Templates
                var templateIds = configuration.User.TemplateIds;  // NOTE: SMS templates contains intentional mistakes and are tested by another test against errors
                Assert.That(templateIds.Email.ZaakCreate(), Is.Not.Null.Or.Empty);
                Assert.That(templateIds.Email.ZaakUpdate(), Is.Not.Null.Or.Empty);
                Assert.That(templateIds.Email.ZaakClose(), Is.Not.Null.Or.Empty);
                #pragma warning restore IDE0008
            });
        }

        [Ignore("Hack"), TestCaseSource(nameof(GetTestCases))]
        public void InConfig_InvalidProperties_ThrowsExpectedExceptions(
            (string CaseId, TestDelegate Logic, string ExpectedErrorMessage) test)
        {
            // Act & Assert
            ArgumentException? exception = Assert.Throws<ArgumentException>(test.Logic);  
            Assert.That(exception?.Message.StartsWith(test.ExpectedErrorMessage), Is.True,
                message: $"{test.CaseId}: {exception?.Message}");
        }

        private static IEnumerable<(string CaseId, TestDelegate ActualMethod, string ExpectedErrorMessage)> GetTestCases()
        {
            var webApiConfiguration = new WebApiConfiguration(ConfigurationHandler.GetConfiguration(), new Mock<ILoadingService>().Object);

            // Invalid: Not existing
            yield return ("#1", () => webApiConfiguration.User.API.BaseUrl.NotifyNL(), Resources.Configuration_ERROR_ValueNotFoundOrEmpty);
            // Invalid: Empty
            yield return ("#2", () => webApiConfiguration.User.Authorization.JWT.Audience(), Resources.Configuration_ERROR_ValueNotFoundOrEmpty);
            // Invalid: https://domain
            yield return ("#3", () => webApiConfiguration.User.Domain.Objecten(), Resources.Configuration_ERROR_ContainsHttp);
            // Invalid: domain/api/v1/typen
            yield return ("#4", () => webApiConfiguration.User.Domain.ObjectTypen(), Resources.Configuration_ERROR_ContainsEndpoint);
            // Invalid: Whitespace
            yield return ("#5", () => webApiConfiguration.User.TemplateIds.Sms.ZaakCreate(), Resources.Configuration_ERROR_ValueNotFoundOrEmpty);
            // Invalid: 8-4-(2-2)-4-12
            yield return ("#6", () => webApiConfiguration.User.TemplateIds.Sms.ZaakUpdate(), Resources.Configuration_ERROR_InvalidTemplateId);
            // Invalid: (9)-4-4-4-12
            yield return ("#7", () => webApiConfiguration.User.TemplateIds.Sms.ZaakClose(), Resources.Configuration_ERROR_InvalidTemplateId);
        }
    }
}