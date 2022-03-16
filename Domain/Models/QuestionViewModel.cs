using Domain.DTOs;
using System;
using System.Collections.Generic;

namespace Domain.Models
{
    public class QuestionViewModel
    {
        public class CreateQuestionRequestModel
        {
            public string QuestionText { get; set; }
            public int Points { get; set; }
            public Guid ModuleId { get; set; }

        }

        public class UpdateQuestionRequestModel
        {
            public string QuestionText { get; set; }
            public int Points { get; set; }
        }
        public class QuestionsResponseModel : BaseResponse
        {
            public IEnumerable<QuestionDTO> Data { get; set; } = new List<QuestionDTO>();
        }

        public class QuestionResponseModel : BaseResponse
        {
            public QuestionDTO Data { get; set; }
        }
    }
}
