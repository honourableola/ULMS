using Domain.DTOs;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Domain.Models
{
    public class AdminViewModel
    {
        public class CreateAdminRequestModel
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string PhoneNumber { get; set; }
            public string Image { get; set; }
        }

        public class UpdateAdminRequestModel
        {
            public string PhoneNumber { get; set; }
            public string AdminPhoto { get; set; }
        }

        public class UpdateAdminPhotoRequestModel
        {
            public string AdminPhoto { get; set; }
        }
        public class AdminsResponseModel : BaseResponse
        {
            public IEnumerable<AdminDTO> Data { get; set; } = new List<AdminDTO>();
        }

        public class AdminResponseModel : BaseResponse
        {
            public AdminDTO Data { get; set; }
        }
    }
}
