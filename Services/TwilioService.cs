using Microsoft.EntityFrameworkCore;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public interface ITwilioService
    {
        Task SendSmsAsync(string message);
        Task SendWhatsappMessageAsync(string message);
    }
    public class TwilioService : ITwilioService
    {
        private readonly FireDbContext db;
        public TwilioService (FireDbContext Db)
        {
            db= Db;
        }

        public async Task SendSmsAsync(string message)
        {
            var settings = await db.NotificationSettings.FirstOrDefaultAsync();

            if (settings == null)
                throw new Exception("Notification settings not found.");

            TwilioClient.Init(settings.AccountSid, settings.AuthToken);

            await MessageResource.CreateAsync(
                body: message,
                from: new PhoneNumber(settings.TwilioPhoneNumber),
                to: new PhoneNumber(settings.ReceiverPhone)
            );
        }

        public async Task SendWhatsappMessageAsync(string message)
        {
            var settings = await db.NotificationSettings.FirstOrDefaultAsync();

            if (settings == null)
                throw new Exception("Notification settings not found.");

            TwilioClient.Init(settings.AccountSid, settings.AuthToken);

            await MessageResource.CreateAsync(
                body: message,
                from: new PhoneNumber($"whatsapp:{settings.TwilioWhatsappNumber}"),
                to: new PhoneNumber($"whatsapp:{settings.ReceiverPhone}")
            );
        }
    }
}
