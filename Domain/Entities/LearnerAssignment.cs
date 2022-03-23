using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class LearnerAssignment : BaseEntity
    {
        public Guid LearnerId { get; set; }
        public Guid AssignmentId { get; set; }
        public Learner Learner { get; set; }
        public Assignment Assignment { get; set; }
        public AssignmentStatus Status { get; set; }
        public string TenantId { get; set; }
    }
}
