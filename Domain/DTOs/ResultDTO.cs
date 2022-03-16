using System;

namespace Domain.DTOs
{
    public class ResultDTO
    {
        public Guid Id { get; set; }
        public string AssessmentName { get; set; }
        public AssessmentDTO Assessment { get; set; }
        public string LearnerName { get; set; }
        public LearnerDTO Learner { get; set; }
        public int ObtainedMarks { get; set; }
        public int TotalMarks { get; set; }
    }
}
