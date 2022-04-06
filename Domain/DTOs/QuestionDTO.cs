using System;
using System.Collections.Generic;

namespace Domain.DTOs
{
    public class QuestionDTO
    {
        public Guid Id { get; set; }
        public string QuestionText { get; set; }
        public int Points { get; set; }
        public Guid AssessmentId { get; set; }
        public Guid ModuleId { get; set; }
        public string ModuleName { get; set; }
        public AssessmentDTO AssessmentDTO { get; set; }
        public ICollection<OptionDTO> Options = new List<OptionDTO>();

    }
}
