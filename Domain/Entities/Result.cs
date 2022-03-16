using Domain.Multitenancy;
using System;

namespace Domain.Entities
{
    public class Result : BaseEntity, ITenant
    {
        public Guid AssessmentId { get; set; }
        public Assessment Assessment { get; set; }
        public Guid LearnerId { get; set; }
        public Learner Learner { get; set; }
        public int ObtainedMarks { get; set; }
        public int TotalMarks { get; set; }
        public string TenantId { get; set; }

    }
}
