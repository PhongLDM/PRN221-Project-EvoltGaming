using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace EvoltingStore.EmailService
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsyns(string email, string subject, string message)
        {
            var fromEmail = "codewithphinx@gmail.com";
            var fromPassword = "aocw cxgl vaga owhy";
            var displayName = "Evolting Store Support";

            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(fromEmail, fromPassword)
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(fromEmail, displayName),
                Subject = subject,
                Body = message,
                IsBodyHtml = true
            };

            mailMessage.To.Add(email);

            return client.SendMailAsync(mailMessage);
        }
    }
}