using Domain.DTOs;
using System;

namespace Domain.Models
{
    public class CourseConstantViewModel
    {
        public class CreateCourseConstantRequestModel
        {
            public int MaximumNoOfMajorCourses { get; set; }
            public int MaximumNoOfAdditionalCourses { get; set; }
            public int NoOfAssessmentQuestions { get; set; }
            public TimeSpan DurationOfAssessment { get; set; }

        }

        public class UpdateCourseConstantRequestModel
        {
            public int MaximumNoOfMajorCourses { get; set; }
            public int MaximumNoOfAdditionalCourses { get; set; }
            public int NoOfAssessmentQuestions { get; set; }
            public TimeSpan DurationOfAssessment { get; set; }
        }
        public class CourseConstantResponseModel : BaseResponse
        {
            public CourseConstantDTO Data { get; set; }
        }

    }
}
