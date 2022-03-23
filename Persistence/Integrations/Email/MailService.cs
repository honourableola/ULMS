
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using Persistence.Integrations.MailKitModels;
using Persistence.Integrations.MailKitModels.MailTemplates;
using System.IO;
using System.Threading.Tasks;

namespace Persistence.Integrations.Email
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;
        public MailService(IOptions<MailSettings> options)
        {
            _mailSettings = options.Value;
        }
        public async Task SendEmailAsync(MailRequest mailRequest)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
            email.Subject = mailRequest.Subject;
            var builder = new BodyBuilder();
            if(mailRequest.Attachments != null)
            {
                byte[] fileBytes;
                foreach (var file in mailRequest.Attachments)
                {
                    if(file.Length > 0)
                    {
                        using(var ms = new MemoryStream())
                        {
                            file.CopyTo(ms);
                            fileBytes = ms.ToArray();
                        }
                        builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                    }
                }
            }
            builder.HtmlBody = mailRequest.Body;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }

        public async Task SendSuccessfulCourseAssignmentToInstructorEmailAsync(SuccessfulCourseAssignmentToInstructor request)
        {
            string FilePath = "C:\\Users\\OWNER\\source\\repos\\ULMS\\src\\ULMS.API\\wwwroot\\Templates\\SuccessfulCourseAssignmentToInstructor.html";
            StreamReader str = new StreamReader(FilePath);
            string MailText = str.ReadToEnd();
            str.Close();
            MailText = MailText.Replace("[username]", request.ToEmail).Replace("[email]", request.ToEmail).Replace("[coursenames]", request.CourseNames).Replace("[firstname]", request.FirstName).Replace("[lastname]", request.LastName);
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            email.To.Add(MailboxAddress.Parse(request.ToEmail));
            email.Subject = $"Courses Assigned";
            var builder = new BodyBuilder();
            builder.HtmlBody = MailText;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }

        public async Task SendSuccessfulCourseAssignmentToLearnerEmailAsync(SuccessfulCourseAssignmentToLearner request)
        {
            string FilePath = "C:\\Users\\OWNER\\source\\repos\\ULMS\\src\\ULMS.API\\wwwroot\\Templates\\SuccessfulCourseAssignmentToLearner.html";
            StreamReader str = new StreamReader(FilePath);
            string MailText = str.ReadToEnd();
            str.Close();
            MailText = MailText.Replace("[username]", request.ToEmail).Replace("[email]", request.ToEmail).Replace("[coursenames]", request.CourseNames).Replace("[firstname]", request.FirstName).Replace("[lastname]", request.LastName);
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            email.To.Add(MailboxAddress.Parse(request.ToEmail));
            email.Subject = $"Courses Assigned";
            var builder = new BodyBuilder();
            builder.HtmlBody = MailText;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }

        public async Task SendSuccessfulCourseRequestApprovalEmailAsync(CourseRequestApproval request)
        {
            string FilePath = "C:\\Users\\OWNER\\source\\repos\\ULMS\\src\\ULMS.API\\wwwroot\\Templates\\SuccessfulCourseRequestApproval.html";
            StreamReader str = new StreamReader(FilePath);
            string MailText = str.ReadToEnd();
            str.Close();
            MailText = MailText.Replace("[username]", request.ToEmail).Replace("[email]", request.ToEmail).Replace("[coursename]", request.CourseName).Replace("[firstname]", request.FirstName).Replace("[lastname]", request.LastName);
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            email.To.Add(MailboxAddress.Parse(request.ToEmail));
            email.Subject = $"Approval of Course Request";
            var builder = new BodyBuilder();
            builder.HtmlBody = MailText;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }

        public async Task SendSuccessfulCourseRequestEmailAsync(SuccessfulCourseRequestMail request)
        {
            string FilePath = "C:\\Users\\OWNER\\source\\repos\\ULMS\\src\\ULMS.API\\wwwroot\\Templates\\SuccessfulCourseRequestMail.html";
            StreamReader str = new StreamReader(FilePath);
            string MailText = str.ReadToEnd();
            str.Close();
            MailText = MailText.Replace("[username]", request.ToEmail).Replace("[email]", request.ToEmail).Replace("[coursename]", request.CourseName).Replace("[firstname]", request.FirstName).Replace("[lastname]", request.LastName);
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            email.To.Add(MailboxAddress.Parse(request.ToEmail));
            email.Subject = $"Successful Course Request";
            var builder = new BodyBuilder();
            builder.HtmlBody = MailText;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }

        public async Task SendCourseRequestRejectionEmailAsync(CourseRequestRejection request)
        {
            string FilePath = "C:\\Users\\OWNER\\source\\repos\\ULMS\\src\\ULMS.API\\wwwroot\\Templates\\SuccessfulCourseRequestRejection.html";
            StreamReader str = new StreamReader(FilePath);
            string MailText = str.ReadToEnd();
            str.Close();
            MailText = MailText.Replace("[username]", request.ToEmail).Replace("[email]", request.ToEmail).Replace("[coursename]", request.CourseName).Replace("[firstname]", request.FirstName).Replace("[lastname]", request.LastName);
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            email.To.Add(MailboxAddress.Parse(request.ToEmail));
            email.Subject = $"Rejection of Course Request";
            var builder = new BodyBuilder();
            builder.HtmlBody = MailText;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }

        public async Task SendWelcomeEmailAsync(WelcomeMail request)
        {
            string FilePath = "C:\\Users\\OWNER\\source\\repos\\ULMS\\src\\ULMS.API\\wwwroot\\Templates\\WelcomeTemplate.html";
            StreamReader str = new StreamReader(FilePath);
            string MailText = str.ReadToEnd();
            str.Close();
            MailText = MailText.Replace("[username]", request.ToEmail).Replace("[email]", request.ToEmail).Replace("[password]", request.Password).Replace("[firstname]", request.FirstName).Replace("[lastname]", request.LastName);
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            email.To.Add(MailboxAddress.Parse(request.ToEmail));
            email.Subject = $"Welcome {request.FirstName} {request.LastName}";
            var builder = new BodyBuilder();
            builder.HtmlBody = MailText;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }

        public async Task SendAssignmentSuccessfullyAssignedToLearnerEmailAsync(AssignmentSuccessfullyAssigned request)
        {
            string FilePath = "C:\\Users\\OWNER\\source\\repos\\ULMS\\src\\ULMS.API\\wwwroot\\Templates\\AssignmentSuccessfullyAssigned.html";
            StreamReader str = new StreamReader(FilePath);
            string MailText = str.ReadToEnd();
            str.Close();
            MailText = MailText.Replace("[username]", request.ToEmail).Replace("[email]", request.ToEmail).Replace("[assignmentnames]", request.AssignmentNames).Replace("[firstname]", request.FirstName).Replace("[lastname]", request.LastName);
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            email.To.Add(MailboxAddress.Parse(request.ToEmail));
            email.Subject = $"Assignments Notification";
            var builder = new BodyBuilder();
            builder.HtmlBody = MailText;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
    }
}
