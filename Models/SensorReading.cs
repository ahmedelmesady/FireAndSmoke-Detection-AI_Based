using Microsoft.VisualBasic;

namespace WebApplication1.Models
{
    public class SensorReading
    {
        public int Id { get; set; }
        public int Gas { get; set; }
        public int Motion { get; set; }
        public int Flame { get; set; }
        public double Temp { get; set; }
        public double Humidity { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
