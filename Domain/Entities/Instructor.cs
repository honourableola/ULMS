using Domain.Multitenancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Instructor: BaseEntity, ITenant
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string InstructorPhoto { get; set; }
        public string InstructorLMSCode { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public string TenantId { get; set; }
        public ICollection<InstructorCourse> InstructorCourses { get; set; } = new List<InstructorCourse>();
        public ICollection<Assessment> Assessments { get; set; } = new List<Assessment>();


    }
}
