
using Microsoft.EntityFrameworkCore;
using WebApplication1.Hubs;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<FireDbContext>(op => op.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddScoped<ISensorService, SensorService>();
            builder.Services.AddScoped<ITwilioService, TwilioService>();
            builder.Services.AddScoped<INotificationService, NotificationService>();
            builder.Services.AddScoped<IAlertService, AlertService>();
            builder.Services.AddSingleton<AlertRateLimiter>();
            builder.Services.AddSignalR();
            builder.Services.AddCors(op =>
            op.AddPolicy("CorsPolicy", builder =>
            {
                builder.SetIsOriginAllowed(origin => true)
                       .AllowAnyHeader()
                       .AllowAnyMethod()
                       .AllowCredentials();
            })
            );
            builder.Services.AddHttpClient<IAIService, AIService>(client =>
            {
                client.BaseAddress = new Uri("https://hisham159-fire-detection-api.hf.space/");
            });
            builder.Services.AddScoped<CameraService>();
            var app = builder.Build();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseCors("CorsPolicy");
            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.MapHub<AlertHub>("/alertHub");

          
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<FireDbContext>(); 

                    
                    context.Database.EnsureCreated();
                    if (!context.Sensors.Any())
                    {
                        context.Sensors.AddRange(
                            new Sensor { Id = 1, Name = "Gas", LastSeen = DateTime.MinValue },
                            new Sensor { Id = 2, Name = "Motion", LastSeen = DateTime.MinValue },
                            new Sensor { Id = 3, Name = "Tempreture", LastSeen = DateTime.MinValue },
                            new Sensor { Id = 4, Name = "Flame", LastSeen = DateTime.MinValue },
                            new Sensor { Id = 5, Name = "Humidity", LastSeen = DateTime.MinValue }
                        );
                        context.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error during DB Seeding: {ex.Message}");
                }
            }

            app.Run();
        }
    }
}
