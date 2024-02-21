// © 2023, Worth Systems.

using ApiClientWrapper.Configuration;
using ApiClientWrapper.Managers;
using ApiClientWrapper.Managers.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
// External library from Notify UK (gov.uk)
using Notify.Client;
using Notify.Models;
using Notify.Models.Responses;

namespace ApiClientWrapper
{
    internal static class Program
    {
        private static IHost? s_host;

        private static void Main()
        {
            s_host = Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    // Register singletons
                    services.AddSingleton<ApiClientConfiguration>();

                    // Register instances
                    services.AddTransient<IApiClientManager, ApiClientManager>();
                })
                .Build();

            DebuggingCalls(s_host.Services.GetRequiredService<IApiClientManager>());
        }

        private static void DebuggingCalls(
            IApiClientManager apiManager)
        {
            NotificationClient notifyClient = apiManager.Client;

            // TEMPLATES
            List<TemplateResponse> templates = notifyClient.GetAllTemplates().templates;

            // Email template
            TemplateResponse emailTemplate = templates.First(template => template.type == "email");

            // SMS template
            TemplateResponse smsTemplate = templates.First(template => template.type == "sms");

            // Personalization
            Dictionary<string, dynamic> personalization = new()
            {
                { "city",    "Rotterdam" },
                { "address", "Coolsingel 40, 3011 AD Rotterdam" },
                { "hour",    "14:00" }
            };

            // Sending email
            EmailNotificationResponse emailResponse = notifyClient.SendEmail("tkrystyan@worth.systems", emailTemplate.id, clientReference: "Email #1");

            // Sending SMS
            SmsNotificationResponse smsResponse = notifyClient.SendSms("+4917634598307", smsTemplate.id, clientReference: "SMS #1");

            // NOTIFICATIONS
            NotificationList notifications = notifyClient.GetNotifications();
        }
    }
}