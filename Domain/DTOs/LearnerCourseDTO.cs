using Domain.Entities;
using Domain.Enums;
using System;

namespace Domain.DTOs
{
    public class LearnerCourseDTO
    {
        public Guid Id { get; set; }
        public string CourseName { get; set; }
        public string Description { get; set; }
        public string CategoryName { get; set; }
        public Guid CategoryId { get; set; }
        public CourseAvailabilityStatus AvailabilityStatus { get; set; }
        public LearnerCourseType CourseType { get; set; }
        public Guid LearnerId { get; set; } 
        public Course Course { get; set; }
        public Guid CourseId { get; set; }
       

    }
}
