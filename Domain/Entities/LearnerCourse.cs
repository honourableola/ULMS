using Domain.Enums;
using Domain.Multitenancy;
using System;

namespace Domain.Entities
{
    public class LearnerCourse : BaseEntity, ITenant
    {
        public Guid LearnerId { get; set; }
        public Learner Learner { get; set; }
        public Guid CourseId { get; set; }
        public Course Course { get; set; }
        public LearnerCourseType CourseType { get; set; }
        public string TenantId { get; set; }

    }
}
