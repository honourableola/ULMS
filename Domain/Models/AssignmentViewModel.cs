using Domain.DTOs;
using Domain.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class AssignmentViewModel
    {
        public class CreateAssignmentRequestModel
        {
            public string Name { get; set; }
            public string AssignmentContent { get; set; }
            //public IFormFile PdfUpload { get; set; }
            public Guid CourseId { get; set; }          
        }

        public class UpdateAssignmentRequestModel
        {
            public string AssignmentContent { get; set; }
            public Guid CourseId { get; set; }
        }

        public class SubmitAssignmentRequestModel
        {
            public Guid AssignmentId { get; set; }
            public string ResponseContent { get; set; }
            public string ResponsePdfUpload { get; set; }
        }

        public class AssignAssignmentRequestModel
        {
            public Guid LearnerId { get; set; }
            public List<Guid> AssignmentIds { get; set; } = new List<Guid>();
        }

        public class FilterLearnerAssignmentsRequestModel
        {
            public Guid CourseId { get; set; }
            public AssignmentStatus Status { get; set; }
        }

        public class AssessAssignmentRequestModel
        {
            public Guid learnerId { get; set; }
            public Guid AssignmentId { get; set; }
            public double LearnerScore { get; set; }
        }
        public class AssignmentsResponseModel : BaseResponse
        {
            public IEnumerable<AssignmentDTO> Data { get; set; } = new List<AssignmentDTO>();
        }

        public class AssignmentResponseModel : BaseResponse
        {
            public AssignmentDTO Data { get; set; }
        }
    }
}
