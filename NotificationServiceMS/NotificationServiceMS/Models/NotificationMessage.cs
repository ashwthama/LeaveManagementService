namespace NotificationServiceMS.Models
{
    public class NotificationMessage
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string Type { get; set; } // Email or SMS
        public string Recipient { get; set; }
    }
}
