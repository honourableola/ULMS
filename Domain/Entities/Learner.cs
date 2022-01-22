using Domain.Multitenancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Learner : BaseEntity, ITenant
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string LearnerPhoto { get; set; }
        public string TenantId { get; set; }
        public ICollection<LearnerCourse> LearnerCourses { get; set; } = new List<LearnerCourse>();
    }
}
