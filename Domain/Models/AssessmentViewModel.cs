using Domain.DTOs;
using System;
using System.Collections.Generic;

namespace Domain.Models
{
    public class AssessmentViewModel
    {
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
