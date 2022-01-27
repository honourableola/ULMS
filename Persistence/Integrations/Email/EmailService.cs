using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Email;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Persistence.Integrations.Email
{ 
    public class EmailService : IEmailService
    {
        private readonly EmailConfiguration _emailConfiguration;
        public EmailService(IOptions<EmailConfiguration> options)
        {
            _emailConfiguration = options.Value;
        }
        public async Task Send(string from, string fromName, string to, string toName, string subject, string message, IDictionary<string, Stream> attachments = null)
        {
           var sendGridClient = new SendGridClient(_emailConfiguration.ApiKey);
           var fromEmailAddress = new EmailAddress(from, fromName);
           var toEmailAddress = new EmailAddress(to, toName);
           var emailMessage = MailHelper.CreateSingleEmail(fromEmailAddress, toEmailAddress, subject, message, message);
           if (attachments != null)
           {
               foreach (var attachment in attachments)
               {
                   await emailMessage.AddAttachmentAsync(attachment.Key, attachment.Value);
               }
           }

           await sendGridClient.SendEmailAsync(emailMessage);

        }

        public async Task SendBulk(string from, string fromName, IDictionary<string, string> tos, string subject, string message, IDictionary<string, Stream> attachments = null)
        {
            var sendGridClient = new SendGridClient(_emailConfiguration.ApiKey);
            var fromEmailAddress = new EmailAddress(from, fromName);
            var toEmailAddresses = tos.Select(t => new EmailAddress(t.Key, t.Value)).ToList();
            var emailMessage = MailHelper.CreateSingleEmailToMultipleRecipients(fromEmailAddress, toEmailAddresses, subject, message, message);
            if (attachments != null)
            {
                foreach (var attachment in attachments)
                {
                    await emailMessage.AddAttachmentAsync(attachment.Key, attachment.Value);
                }
            }
            await sendGridClient.SendEmailAsync(emailMessage);
        }
    }
}
