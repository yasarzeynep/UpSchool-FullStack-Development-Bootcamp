using Application.Common.Helpers;
using Application.Common.Interfaces;
using Application.Common.Models.Email;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Infrastructure.Services
{
    public class EmailManager : IEmailService
    {
        private readonly EmailTemplateProvider _templateProvider;

        public EmailManager(EmailTemplateProvider templateProvider)
        {
            _templateProvider = templateProvider;
        }

        public void SendEmailConfiguration(SendEmailConfirmationDto sendEmailConfirmationDto)
        {
            var htmlContent = _templateProvider.GetEmailConfirmationTemplate(sendEmailConfirmationDto.Name, sendEmailConfirmationDto.ExcelProductFile);
            SendEmail(sendEmailConfirmationDto.Email, MessagesHelper.Email.Confirmation.Subject, htmlContent);
        }


        private void SendEmail(string toEmailAddress, string subject, string content)
        {
            MailMessage message = new MailMessage();

            message.To.Add(toEmailAddress);
            message.From = new MailAddress("crawlerdataup@gmail.com");
            message.Subject = subject;
            message.IsBodyHtml = true;
            message.Body = content;

            SmtpClient client = new SmtpClient();
            client.Port = 587;
            client.Host = "smtp.sendgrid.net";
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("apikey", "");
            client.DeliveryMethod = SmtpDeliveryMethod.Network;

            client.Send(message);
        }
    }
}