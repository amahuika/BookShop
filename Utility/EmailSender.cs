using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;

namespace BookShop.Utility
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // instanciate new mimeMessage class
            var emailToSend = new MimeMessage();

            // configure email
            emailToSend.From.Add(MailboxAddress.Parse("bboyakorn@gmail.com"));
            emailToSend.To.Add(MailboxAddress.Parse("amahuika@hotmail.com"));
            emailToSend.Subject = subject;
            emailToSend.Body = new TextPart(MimeKit.Text.TextFormat.Html){ Text = htmlMessage};

            // send email
            // using statements calls the dispose method when it goes out of scope
            using (var emailClient = new SmtpClient())
            {
                emailClient.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                emailClient.Authenticate("bboyakorn@gmail.com", "Akorn1309");
            }


            return Task.CompletedTask;
        }
    }
}
