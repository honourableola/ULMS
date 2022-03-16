using Domain.Enums;
using System;

namespace Domain.DTOs
{
    public class OptionDTO 
    {
        public Guid Id { get; set; }
        public string Label { get; set; }
        public string OptionText { get; set; }
        public OptionStatus Status { get; set; }
        public Guid QuestionId { get; set; }
        public QuestionDTO QuestionDTO { get; set; }
    }
}
