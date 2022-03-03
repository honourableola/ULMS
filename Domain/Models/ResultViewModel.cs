using Domain.DTOs;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class ResultViewModel
    {
        public class CreateResultRequestModel
        {
            public Guid AssessmentId { get; set; }
            public Assessment Assessment { get; set; }
            public Guid LearnerId { get; set; }
            public Learner Learner { get; set; }
            /*public Guid InstructorId { get; set; }
              public Instructor Instructor { get; set; }*/
            public int ObtainedMarks { get; set; }
            public int TotalMarks { get; set; }

        }

        /* public class UpdateResultRequestModel
         {
             public Guid AssessmentId { get; set; }
             public Assessment Assessment { get; set; }
             public Guid LearnerId { get; set; }
             public Learner Learner { get; set; }
             *//*public Guid InstructorId { get; set; }
               public Instructor Instructor { get; set; }*//*
             public int ObtainedMarks { get; set; }
             public int TotalMarks { get; set; }
         }*/
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
