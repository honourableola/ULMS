using Domain.Multitenancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Topic : BaseEntity, ITenant
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public Guid ModuleId { get; set; }

        public Module Module { get; set; }
        public string TenantId { get; set; }
    }
}
