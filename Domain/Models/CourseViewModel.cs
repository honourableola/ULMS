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

        public class CourseRequestRequestModel
        {
            public Guid courseId { get; set; }
            public Guid LearnerId { get; set; }
            public string RequestMessage { get; set; }

        }
        public class LearnerCourseAssignmentRequestModel
        {
            public Guid LearnerId { get; set; }
            public List<Guid> Ids { get; set; } = new List<Guid>();
        }

        public class InstructorCourseAssignmentRequestModel
        {
            public Guid InstructorId { get; set; }
            public List<Guid> Ids { get; set; } = new List<Guid>();
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

        public class CourseRequestsResponseModel : BaseResponse
        {
            public IEnumerable<CourseRequestDTO> Data { get; set; } = new List<CourseRequestDTO>();
        }

        public class CourseResponseModel : BaseResponse
        {
            public CourseDTO Data { get; set; }
        }

        public class LearnerCoursesResponseModel : BaseResponse
        {
            public IEnumerable<LearnerCourseDTO> Data { get; set; } = new List<LearnerCourseDTO>();
        }

        public class CourseRequestResponseModel : BaseResponse
        {
            public CourseRequestDTO Data { get; set; }
        }


    }
}
