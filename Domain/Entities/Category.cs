using Domain.Multitenancy;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Category : BaseEntity, ITenant
    {
        public string Name { get; set; }
        public ICollection<Course> Courses { get; set; } = new List<Course>();
        public string TenantId { get; set; }
    }
}
