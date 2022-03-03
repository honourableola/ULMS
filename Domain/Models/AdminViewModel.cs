using Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            public string AdminPhoto { get; set; }

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
