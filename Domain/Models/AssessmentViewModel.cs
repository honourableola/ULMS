using Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class AssessmentViewModel
    {
        public class CreateAssessmentRequestModel
        {
            public string Title { get; set; }
            public string Description { get; set; }
            public Guid ModeuleId { get; set; }
            public Guid CourseId { get; set; }
            public Guid InstructorId { get; set; }
            public TimeSpan DurationInMinutes { get; set; }
        }

        public class UpdateAssessmentRequestModel
        {
            public string Description { get; set; }
            public TimeSpan DurationInMinutes { get; set; }
        }
        public class AssessmentsResponseModel : BaseResponse
        {
            public IEnumerable<AssessmentDTO> Data { get; set; } = new List<AssessmentDTO>();
        }

        public class AssessmentResponseModel : BaseResponse
        {
            public AssessmentDTO Data { get; set; }
        }
    }
}
