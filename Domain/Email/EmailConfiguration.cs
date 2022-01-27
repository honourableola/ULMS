using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Email
{
    public class EmailConfiguration
    {
        public string ApiKey { get; set; }
        public string FromEmail { get; set; }
        public string FromName { get; set; }
        public string ForgotPasswordSubject { get; set; }
        public string WelcomeSubject { get; set; }
        public string ChangePasswordSubject { get; set; }
        public string CourseAssigned { get; set; }
        public string CourseRequestApproval { get; set; }
        public string ResetPasswordSubject { get; set; }
    }

}
