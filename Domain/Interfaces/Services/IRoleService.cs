using Domain.DTOs;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Domain.Models.RoleViewModel;

namespace Domain.Interfaces.Services
{
    public interface IRoleService
    {
        public Task<BaseResponse> AddRole(CreateRoleRequestModel model);
        public Task<RoleResponseModel> GetRoleByName(string name);
        public Task<RolesResponseModel> GetRoles();
    }
}
