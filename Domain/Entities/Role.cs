using Domain.Multitenancy;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Role : BaseEntity, ITenant
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string TenantId { get; set; }
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}
