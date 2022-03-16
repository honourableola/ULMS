using Domain.Entities;
using System;
using System.Collections.Generic;

namespace Domain.DTOs
{
    public class RoleDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}
