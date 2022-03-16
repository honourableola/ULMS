using Domain.Multitenancy;
using System;

namespace Domain.Entities
{
    public class Topic : BaseEntity, ITenant
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public Guid ModuleId { get; set; }

        public Module Module { get; set; }
        public bool IsTaken { get; set; } = false;
        public string TenantId { get; set; }
    }
}
