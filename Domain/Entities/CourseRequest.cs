using Domain.Enums;
using Domain.Multitenancy;
using System;

namespace Domain.Entities
{
    public class CourseRequest : BaseEntity, ITenant
    {
        public Guid CourseId { get; set; }
        public Course Course { get; set; }
        public Guid LearnerId { get; set; }
        public Learner Learner { get; set; }
        public string RequestMessage { get; set; }
        public CourseRequestStatus RequestStatus { get; set; }
        public string TenantId { get; set; }
    }
}
