using WebApplication1.Models;

namespace WebApplication1.Services
{
    public interface IAlertService
    {
        Task HandlePredictionAsync(PredictResponse prediction);
    }

    public class AlertService : IAlertService
    {
        private readonly FireDbContext db;
        private readonly INotificationService notification;
        private readonly AlertRateLimiter rateLimiter;

        public AlertService(FireDbContext db, INotificationService notification)
        {
            this.db = db;
            this.notification = notification;
        }

        public async Task HandlePredictionAsync(PredictResponse prediction)
        {

            if(!prediction.Fire_Detected && !prediction.Smoke_Detected)
            {   
                return;
            }

            var alert = new Alerts
            {
                CreatedAt = DateTime.Now,
                Message=prediction.Message,
                Severity=prediction.Detections.Max(d => d.Confidence).ToString()
            };  
            db.Alerts.Add(alert);
            await db.SaveChangesAsync();

                await notification.SendFireAlertAsync(alert);
                await Task.CompletedTask;

        }
    }
}
