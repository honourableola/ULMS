
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Integrations.Email
{
    public interface IMailSender
    {
        public Task SendWelcomeMail(string email, string fullName, string userName, string password);

        public Task SendForgotPasswordMail(string email, string name, string passwordResetLink);

        public Task SendChangePasswordMail(string email, string name);

        public Task SendResetPasswordMail(string email, string name);

        //public Task SendVerifyMail(string email, string name);

        //public Task SendAccountUpgradeMail(string email, string name, string activationLink);

        
    }
}
