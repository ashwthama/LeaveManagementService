using NotificationServiceMS.Models;

namespace NotificationServiceMS.Services
{
    public class NotificationHandler
    {
        public void Send(NotificationMessage message)
        {
            if (message.Type == "Email")
            {
                SendEmail(message.Recipient, message.Subject, message.Body);
            }
            else if (message.Type == "SMS")
            {
                SendSms(message.Recipient, message.Body);
            }
        }

        private void SendEmail(string to, string subject, string body)
        {
            // Replace this with actual email sending logic
            Console.WriteLine($"Sending EMAIL to {to}: Subject={subject}, Body={body}");
        }

        private void SendSms(string to, string body)
        {
            // Replace this with actual SMS sending logic
            Console.WriteLine($"Sending SMS to {to}: Body={body}");
        }
    }
}
