using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class ResultDTO
    {
        public Guid Id { get; set; }
        public Guid AssessmentId { get; set; }
        public AssessmentDTO Assessment { get; set; }
        public Guid LearnerId { get; set; }
        public LearnerDTO Learner { get; set; }
        public int ObtainedMarks { get; set; }
        public int TotalMarks { get; set; }
    }
}
