using MailKit.Security;
using MimeKit;
using NotificationServiceMS.Models;
using System.Net.Mail;

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
            string myEmail = "ashwanikumarnt@gmail.com";
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(myEmail));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;

            email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = body
            };

            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            try
            {
                smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                smtp.Authenticate(myEmail, "frua kwlp vjnx wccr");
                smtp.Send(email);
                Console.WriteLine($"EMAIL sent to {to}: Subject={subject}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send email: {ex.Message}");
            }
            finally
            {
                smtp.Disconnect(true);
            }
        }

        private void SendSms(string to, string body)
        {
            //My Twilio Creds are Expired.
            Console.WriteLine($"Sending SMS to {to}: Body={body}");
        }
    }
}
