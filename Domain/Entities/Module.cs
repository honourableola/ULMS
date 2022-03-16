using Domain.Multitenancy;
using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Module : BaseEntity, ITenant
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public Guid CourseId { get; set; }
        public Course Course { get; set; }
        public string ModuleImage1 { get; set; }
        public string ModuleImage2 { get; set; }
        public string ModulePDF1 { get; set; }
        public string ModulePDF2 { get; set; }
        public string ModuleVideo1 { get; set; }
        public string ModuleVideo2 { get; set; }
        public Assessment Assessment { get; set; }
        public bool IsTaken { get; set; } = false;
        public bool AssessmentGenerated { get; set; } = false;
        public string TenantId { get; set; }
        public ICollection<Topic> Topics { get; set; } = new List<Topic>();
    }
}
