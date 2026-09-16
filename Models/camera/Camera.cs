namespace WebApplication1.Models.camera
{
    public class Camera
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string RtspUrl { get; set; }

        public string Location { get; set; }

        public bool IsActive { get; set; }

        public DateTime LastSeen { get; set; }

        public ICollection<Alerts> Alerts { get; set; }

    }
}
