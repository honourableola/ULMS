using Domain.Multitenancy;
using System;

namespace Domain.Entities
{
    public class Admin : BaseEntity, ITenant
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string AdminPhoto { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public string TenantId { get; set; }
    }
}
