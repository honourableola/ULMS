using Domain.DTOs;
using System.Collections.Generic;

namespace Domain.Models
{
    public class InstructorViewModel
    {
        public class CreateInstructorRequestModel
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string PhoneNumber { get; set; }
            public string InstructorPhoto { get; set; }

        }

        public class UpdateInstructorRequestModel
        {
            public string PhoneNumber { get; set; }
            public string InstructorPhoto { get; set; }
        }
        public class InstructorsResponseModel : BaseResponse
        {
            public IEnumerable<InstructorDTO> Data { get; set; } = new List<InstructorDTO>();
        }

        public class InstructorResponseModel : BaseResponse
        {
            public InstructorDTO Data { get; set; }
        }
    }
}
