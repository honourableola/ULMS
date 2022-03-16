using System;
using System.Collections.Generic;

namespace Domain.DTOs
{
    public class LearnerDTO
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string LearnerPhoto { get; set; }
        public string LearnerLMSCode { get; set; }
        public ICollection<CourseDTO> LearnerCourses { get; set; } = new List<CourseDTO>();
    }
}
