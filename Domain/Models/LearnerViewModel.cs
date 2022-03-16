using Domain.DTOs;
using System.Collections.Generic;

namespace Domain.Models
{
    public class LearnerViewModel
    {
        public class CreateLearnerRequestModel
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string PhoneNumber { get; set; }
            public string LearnerPhoto { get; set; }

        }

        public class UpdateLearnerRequestModel
        {
            public string PhoneNumber { get; set; }
            public string LearnerPhoto { get; set; }
        }
        public class LearnersResponseModel : BaseResponse
        {
            public IEnumerable<LearnerDTO> Data { get; set; } = new List<LearnerDTO>();
        }

        public class LearnerResponseModel : BaseResponse
        {
            public LearnerDTO Data { get; set; }
        }
    }
}
