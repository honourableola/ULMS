using Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class RoleViewModel
    {
        public class CreateRoleRequestModel
        {
            public string Name { get; set; }
            public string Description { get; set; }

        }

        public class UpdateRoleRequestModel
        {
            public string Name { get; set; }
            public string Description { get; set; }
        }
        public class RolesResponseModel : BaseResponse
        {
            public IEnumerable<RoleDTO> Data { get; set; } = new List<RoleDTO>();
        }

        public class RoleResponseModel : BaseResponse
        {
            public RoleDTO Data { get; set; }
        }
    }
}
