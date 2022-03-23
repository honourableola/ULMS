using Domain.Entities;
using System;
using System.Collections.Generic;

namespace Domain.DTOs
{
    public class AssignmentDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string AssignmentContent { get; set; }
        public string AssignmentPdfUpload { get; set; }
        public string ResponseContent { get; set; }
        public string ResponsePdfUpload { get; set; }
        public double LearnerScore { get; set; }
        public Guid CourseId { get; set; }
        public string CourseName { get; set; }
        public Course Course { get; set; }
        public ICollection<LearnerAssignment> LearnerAssignments = new List<LearnerAssignment>();
    }
}
