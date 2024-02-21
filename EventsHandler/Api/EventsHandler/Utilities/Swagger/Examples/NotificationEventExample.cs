// © 2023, Worth Systems.

using System.Diagnostics.CodeAnalysis;
using EventsHandler.Behaviors.Mapping.Enums.NotificatieApi;
using EventsHandler.Behaviors.Mapping.Models.POCOs.NotificatieApi;
using EventsHandler.Constants;
using Swashbuckle.AspNetCore.Filters;

namespace EventsHandler.Utilities.Swagger.Examples
{
    /// <summary>
    /// An example of <see cref="NotificationEvent"/> to be used on Swagger UI.
    /// </summary>
    /// <seealso cref="IExamplesProvider{T}"/>
    [ExcludeFromCodeCoverage]
    internal sealed class NotificationEventExample : IExamplesProvider<NotificationEvent>
    {
        /// <inheritdoc cref="IExamplesProvider{TModel}.GetExamples"/>
        public NotificationEvent GetExamples()
        {
            return new NotificationEvent
            {
                Action = Actions.Create,
                Channel = Channels.Cases,
                Resource = Resources.Status,
                Attributes = new EventAttributes
                {
                    ObjectType = DefaultValues.Models.EmptyUri,
                    CaseType = DefaultValues.Models.EmptyUri,
                    SourceOrganization = new string('0', 9),
                    ConfidentialityNotice = PrivacyNotices.NonConfidential
                },
                MainObject = DefaultValues.Models.EmptyUri,
                ResourceUrl = DefaultValues.Models.EmptyUri,
                CreateDate = DateTime.UtcNow
            };
        }
    }
}