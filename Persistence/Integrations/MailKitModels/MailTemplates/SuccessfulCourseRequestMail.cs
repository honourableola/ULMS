using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Integrations.MailKitModels
{
    public class SuccessfulCourseRequestMail
    {
        public string ToEmail { get; set; }
        public string CourseName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
