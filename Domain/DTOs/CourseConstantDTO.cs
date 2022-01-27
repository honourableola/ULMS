using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class CourseConstantDTO
    {
        public Guid Id { get; set; }
        public int MaximumNoOfMajorCourses { get; set; }
        public int MaximumNoOfAdditionalCourses { get; set; }
    }
}
