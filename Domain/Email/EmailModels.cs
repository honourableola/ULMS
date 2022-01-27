using Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Email
{
    public class WelcomeEmail
    {
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class ForgotPasswordEmail
    {
        public string Name { get; set; }

        public string PasswordResetLink { get; set; }
    }

    public class ResetPasswordEmail
    {
        public string Name { get; set; }
    }

    public class VerifyMail
    {
        public string Name { get; set; }

        public string Email { get; set; }
    }

    public class ChangePasswordMail
    {
        public string Name { get; set; }
    }
    public class  CourseAssigned
    {
        public string Name { get; set; }

        public IList<LearnerCourseDTO> Courses { get; set; } = new List<LearnerCourseDTO>();
    }
}
