using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class LoginViewModel
    {
        public class LoginRequestModel
        {
            [Required]
            public string Email { get; set; }

            [Required]
            public string Password { get; set; }
        }

        public class LoginResponseModel : BaseResponse
        {
            public LoginResponseData Data { get; set; }
        }

        public class LoginResponseData
        {
            public Guid UserId { get; set; }
            public string Email { get; set; }

            public string FirstName { get; set; }

            public string LastName { get; set; }

            public string ULMSIdentificationNumber { get; set; }

            public IEnumerable<string> Roles { get; set; } = new List<string>();

        }



    }
}
