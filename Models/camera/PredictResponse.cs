namespace WebApplication1.Models
{
    public class PredictResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public bool Fire_Detected { get; set; }
        public bool Smoke_Detected { get; set; }
        public string Timestamp { get; set; }
        public string Boxes { get; set; }
        public List<Detection> Detections { get; set; }
        public string ImageUrl { get; set; }
    }
}
