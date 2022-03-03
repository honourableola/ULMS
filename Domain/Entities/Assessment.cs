using Domain.Multitenancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Assessment : BaseEntity, ITenant
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid ModeuleId { get; set; }
        public Guid InstructorId { get; set; }
        public Module Module { get; set; }
        public TimeSpan DurationInMinutes { get; set; }
        public Result Result { get; set; }
        public List<Question> Questions { get; set; } = new List<Question>();
        public string TenantId { get; set; }

    }
}
