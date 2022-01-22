using Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class CourseViewModel
    {
        public class CreateCourseRequestModel
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public Guid CategoryId { get; set; }
            

        }

        public class UpdateCourseRequestModel
        {
            public string Name { get; set; }
            public string Description { get; set; }
        }
        public class CoursesResponseModel : BaseResponse
        {
            public IEnumerable<CourseDTO> Data { get; set; } = new List<CourseDTO>();
        }

        public class CourseResponseModel : BaseResponse
        {
            public CourseDTO Data { get; set; }
        }
    }
}
