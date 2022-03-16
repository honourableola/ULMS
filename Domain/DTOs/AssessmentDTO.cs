using Domain.Entities;
using System;
using System.Collections.Generic;

namespace Domain.DTOs
{
    public class AssessmentDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid ModeuleId { get; set; }
        public ModuleDTO Module { get; set; }
        public TimeSpan DurationInMinutes { get; set; }
        public Result Result { get; set; }
        public List<QuestionDTO> Questions { get; set; } = new List<QuestionDTO>();
    }
}
