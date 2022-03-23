using Domain.Multitenancy;
using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Learner : BaseEntity, ITenant
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string LearnerPhoto { get; set; }
        public string LearnerLMSCode { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public string TenantId { get; set; }
        public ICollection<LearnerCourse> LearnerCourses { get; set; } = new List<LearnerCourse>();
        public ICollection<CourseRequest> CourseRequests { get; set; } = new List<CourseRequest>();
        public ICollection<LearnerAssignment> LearnerAssignments = new List<LearnerAssignment>();
        public ICollection<Result> Results = new List<Result>();
    }
}
