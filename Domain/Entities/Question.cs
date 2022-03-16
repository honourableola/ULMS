using Domain.Multitenancy;
using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Question : BaseEntity, ITenant
    {
        public string QuestionText { get; set; }
        public int Points { get; set; }
        public int QuestionNumber { get; set; }
        public Guid ModuleId { get; set; }
        public Module Module { get; set; }
        public ICollection<Option> Options = new List<Option>();
        public string TenantId { get; set; }
    }
}
