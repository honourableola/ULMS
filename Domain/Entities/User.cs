using Domain.Enums;
using Domain.Multitenancy;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class User : BaseEntity, ITenant
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string PasswordHash { get; set; }
        public string HashSalt { get; set; }
        public UserType UserType { get; set; }
        public Learner Learner { get; set; }
        public Instructor Instructor { get; set; }
        public Admin Admin { get; set; }
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        public string TenantId { get; set; }
    }
}
