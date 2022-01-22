using Domain.Multitenancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Category : BaseEntity, ITenant
    {
        public string Name { get; set; }
        public ICollection<Course> Courses { get; set; } = new List<Course>();
        public string TenantId { get; set; }
    }
}
