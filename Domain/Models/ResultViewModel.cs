using Domain.DTOs;
using Domain.Entities;
using System;
using System.Collections.Generic;

namespace Domain.Models
{
    public class ResultViewModel
    {
        public class MarkAssessmentRequestModel
        {
            public Guid LearnerId { get; set; }
            public Guid AssessmentId { get; set; }
            public IList<Guid> OptionIds { get; set; } = new List<Guid>();

        }
        public class ResultsResponseModel : BaseResponse
        {
            public IEnumerable<ResultDTO> Data { get; set; } = new List<ResultDTO>();
        }

        public class ResultResponseModel : BaseResponse
        {
            public ResultDTO Data { get; set; }
        }
    }
}
