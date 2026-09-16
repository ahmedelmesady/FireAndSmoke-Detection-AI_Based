using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Threading.RateLimiting;
using WebApplication1.Hubs;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public interface ISensorService
    {
        Task ProcessSensorReadingAsync(SensorReading reading);
    }
    public class SensorService : ISensorService
    {
        private readonly FireDbContext db;
        private readonly IHubContext<AlertHub> hubContext; 
        private readonly INotificationService NotificationService;
        private readonly AlertRateLimiter rateLimiter;

        public SensorService(FireDbContext context, IHubContext<AlertHub> hubcontext, INotificationService notificationService, AlertRateLimiter rateLimiter)
        {
            db = context;
            hubContext = hubcontext;
            NotificationService = notificationService;
            this.rateLimiter = rateLimiter;
        }

        public async Task ProcessSensorReadingAsync(SensorReading sensorReading)
        {

            sensorReading.CreatedAt = DateTime.UtcNow; 
            db.SensorReadings.Add(sensorReading);

            var SensorsName = new Dictionary<string, int>
            {
                { "Gas", sensorReading.Gas },
                { "Flame", sensorReading.Flame },
                { "Tempreture", (int)sensorReading.Temp },
                { "Humidity", (int)sensorReading.Humidity },
                { "Motion", sensorReading.Motion }
            };

            foreach (var s in SensorsName)
            {
                if (s.Value >= 0)
                {
                    var sensor=db.Sensors.FirstOrDefault(x => x.Name == s.Key);
                    if (sensor != null)
                    {
                        sensor.LastSeen = DateTime.UtcNow;
                    }
                }
            }
            var reading = new
            {
                gas = sensorReading.Gas,
                flame = sensorReading.Flame,
                temp = sensorReading.Temp,
                humidity = sensorReading.Humidity,
                motion = sensorReading.Motion
            };
           
            await hubContext.Clients.All.SendAsync("ReceiveLiveReading", reading);

            if (sensorReading.Gas > 1500 )
            {
                var alert = new Alerts
                {
                    Message = "Gas Is Higher!",
                    CreatedAt = DateTime.UtcNow,
                    Severity = "High",
                };
                if (rateLimiter.CanSendAlert())
                {
                    await hubContext.Clients.All.SendAsync("ReceiveAlert", alert);
                    await NotificationService.SendFireAlertAsync(alert);

                }

            }
            await db.SaveChangesAsync();
        }
    }
}
  