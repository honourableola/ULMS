using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Integrations.MailKitModels.MailTemplates
{
    public class AssignmentSuccessfullyAssigned
    {
        public string ToEmail { get; set; }
        public string AssignmentNames { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
