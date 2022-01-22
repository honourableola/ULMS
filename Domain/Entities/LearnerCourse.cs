using Domain.Multitenancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class LearnerCourse : BaseEntity, ITenant
    {
        public Guid LearnerId { get; set; }
        public Learner Learner { get; set; }
        public Guid CourseId { get; set; }
        public Course Course { get; set; }
        public string TenantId { get; set; }

    }
}
