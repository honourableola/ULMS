
using Persistence.Integrations.MailKitModels;
using Persistence.Integrations.MailKitModels.MailTemplates;
using System.Threading.Tasks;

namespace Persistence.Integrations.Email
{
    public interface IMailService
    {      
        Task SendEmailAsync(MailRequest mailRequest);
        Task SendWelcomeEmailAsync(WelcomeMail request);
        Task SendSuccessfulCourseRequestEmailAsync(SuccessfulCourseRequestMail request);
        Task SendSuccessfulCourseRequestApprovalEmailAsync(CourseRequestApproval request);
        Task SendCourseRequestRejectionEmailAsync(CourseRequestRejection request);
        Task SendSuccessfulCourseAssignmentToLearnerEmailAsync(SuccessfulCourseAssignmentToLearner request);
        Task SendAssignmentSuccessfullyAssignedToLearnerEmailAsync(AssignmentSuccessfullyAssigned request);
        Task SendSuccessfulCourseAssignmentToInstructorEmailAsync(SuccessfulCourseAssignmentToInstructor request);
    }
}
