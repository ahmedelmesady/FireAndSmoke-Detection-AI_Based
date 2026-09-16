namespace WebApplication1.Models
{

    public class Detection
    {
        public string Class { get; set; }
        public double Confidence { get; set; }
        public BoundingBox Bbox { get; set; }

    }
}
