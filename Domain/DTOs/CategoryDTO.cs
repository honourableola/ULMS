using System;
using System.Collections.Generic;

namespace Domain.DTOs
{
    public class CategoryDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<CourseDTO> Courses { get; set; } = new List<CourseDTO>();
    }
}
