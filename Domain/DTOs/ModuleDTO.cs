using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class ModuleDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public Guid CourseId { get; set; }
        public Course Course { get; set; }
        public string CourseName { get; set; }
        public string ModuleImage1 { get; set; }
        public string ModuleImage2 { get; set; }
        public string ModulePDF1 { get; set; }
        public string ModulePDF2 { get; set; }
        public string ModuleVideo1 { get; set; }
        public string ModuleVideo2 { get; set; }
        public ICollection<TopicDTO> Topics { get; set; } = new List<TopicDTO>();
    }
}
