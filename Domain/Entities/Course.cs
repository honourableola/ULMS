using Domain.Enums;
using Domain.Multitenancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Course: BaseEntity, ITenant
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string TenantId { get; set; }
        public Category Category { get; set; }
        public Guid CategoryId { get; set; }
        public CourseAvailabilityStatus AvailabilityStatus { get; set; }
        public CourseAssignmentStatus AssignmentStatus { get; set; }
        public ICollection<Module> Modules { get; set; } = new List<Module>();
        public ICollection<InstructorCourse> InstructorCourses { get; set; } = new List<InstructorCourse>();
        public ICollection<LearnerCourse> LearnerCourses { get; set; } = new List<LearnerCourse>();
        
    }
}
