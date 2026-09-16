namespace WebApplication1.Models
{
    public class NotificationSettings
    {
        public int Id { get; set; }

        public bool SmsEnabled { get; set; }

        public bool WhatsappEnabled { get; set; }

        public bool EmailEnabled { get; set; }

        public string AccountSid { get; set; }

        public string AuthToken { get; set; }

        public string TwilioPhoneNumber { get; set; }

        public string TwilioWhatsappNumber { get; set; }

        public string ReceiverPhone { get; set; }
    }
}
