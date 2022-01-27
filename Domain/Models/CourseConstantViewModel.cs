using Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class CourseConstantViewModel
    {
        public class CreateCourseConstantRequestModel
        {
            public int MaximumNoOfMajorCourses { get; set; }
            public int MaximumNoOfAdditionalCourses { get; set; }

        }

        public class UpdateCourseConstantRequestModel
        {
            public int MaximumNoOfMajorCourses { get; set; }
            public int MaximumNoOfAdditionalCourses { get; set; }
        }
        public class CourseConstantResponseModel : BaseResponse
        {
            public CourseConstantDTO Data { get; set; }
        }

    }
}
