using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public interface INotificationService
    {
        Task SendFireAlertAsync(Alerts alert);
    }
    public class NotificationService : INotificationService
    {
        private readonly FireDbContext db;
        private readonly ITwilioService _twilio;

        public NotificationService(FireDbContext context, ITwilioService twilio)
        {
            db = context;
            _twilio = twilio;
        }

        public async Task SendFireAlertAsync(Alerts alert)
        {
            var settings = await db.NotificationSettings.FirstOrDefaultAsync();

            if (settings == null)
                return;

            var message =
                $"Fire detected:\nTime: {alert.CreatedAt.AddHours(3)}";

            if (settings.WhatsappEnabled)
            {
                await _twilio.SendWhatsappMessageAsync(message);
            }
        }
    }
}
