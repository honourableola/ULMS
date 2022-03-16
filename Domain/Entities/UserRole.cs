using Domain.Multitenancy;
using System;

namespace Domain.Entities
{
    public class UserRole : BaseEntity, ITenant
    {
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid RoleId { get; set; }
        public Role Role { get; set; }
        public string TenantId { get; set; }
        
    }
}
