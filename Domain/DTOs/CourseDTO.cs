using Domain.Enums;
using System;
using System.Collections.Generic;

namespace Domain.DTOs
{
    public class CourseDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CategoryName { get; set; }
        public Guid CategoryId { get; set; }
        public CourseAvailabilityStatus AvailabilityStatus { get; set; }
        public ICollection<ModuleDTO> Modules { get; set; } = new List<ModuleDTO>();
        public ICollection<AssessmentDTO> Assessments { get; set; } = new List<AssessmentDTO>();
        public ICollection<AssignmentDTO> Assignments { get; set; } = new List<AssignmentDTO>();
        public ICollection<InstructorDTO> InstructorCourses { get; set; } = new List<InstructorDTO>();
        public ICollection<LearnerDTO> LearnerCourses { get; set; } = new List<LearnerDTO>();
    }
}
