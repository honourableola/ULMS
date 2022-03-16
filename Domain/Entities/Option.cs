using Domain.Enums;
using Domain.Multitenancy;
using System;

namespace Domain.Entities
{
    public class Option : BaseEntity, ITenant
    {
        public string Label { get; set; }
        public string OptionText { get; set; }
        public OptionStatus Status { get; set; }
        public Guid QuestionId { get; set; }
        public Question Question { get; set; }
        public string TenantId { get; set; }

    }
}
