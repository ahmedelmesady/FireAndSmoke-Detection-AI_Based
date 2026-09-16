using Microsoft.EntityFrameworkCore;
using WebApplication1.Models.camera;

namespace WebApplication1.Models
{
    public class FireDbContext : DbContext
    {
        public FireDbContext(DbContextOptions<FireDbContext> options) : base(options)
        {
        }
        public DbSet<Sensor> Sensors { get; set; }
        public DbSet<SensorReading> SensorReadings { get; set; }
        public DbSet<Alerts> Alerts  { get; set; }
        public DbSet<NotificationSettings> NotificationSettings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Sensor>().ToTable("Sensors");
            modelBuilder.Entity<Sensor>().HasData(
                new Sensor { Id = 1, Name = "Gas", LastSeen = DateTime.MinValue },
                new Sensor { Id = 2, Name = "Motion", LastSeen = DateTime.MinValue },
                new Sensor { Id = 3, Name = "Tempreture", LastSeen = DateTime.MinValue },
                new Sensor { Id = 4, Name = "Flame", LastSeen = DateTime.MinValue },
                new Sensor { Id = 5, Name = "Humidity", LastSeen = DateTime.MinValue }
            );
            
            modelBuilder.Entity<SensorReading>().ToTable("SensorReadings");
            modelBuilder.Ignore<BoundingBox>();
            modelBuilder.Ignore<Detection>();
            modelBuilder.Ignore<PredictResponse>();

        }

    }
}
