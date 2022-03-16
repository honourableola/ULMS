using Domain.Multitenancy;
using System;

namespace Domain.Entities
{
    public class InstructorCourse : BaseEntity, ITenant
    {
        public Guid InstructorId { get; set; }
        public Instructor Instructor { get; set; }
        public Guid CourseId { get; set; }
        public Course Course { get; set; }
        public string TenantId { get; set; }
    }
}
