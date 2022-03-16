using Domain.DTOs;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Models;
using System;
using System.Threading.Tasks;
using static Domain.Models.RoleViewModel;

namespace Persistence.Implementations.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;

        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }
        public async Task<BaseResponse> AddRole(CreateRoleRequestModel model)
        {
            var roleExists = await _roleRepository.ExistsAsync(r => r.Name == model.Name);

            if (roleExists)
            {
                throw new BadRequestException($"Role already exist");
            }

            var role = new Role
            {
                Id = Guid.NewGuid(),
                Name = model.Name,
                Description = model.Description
            };

            await _roleRepository.AddAsync(role);
            await _roleRepository.SaveChangesAsync();

            return new BaseResponse
            {
                Message = $"Role Created Successfully",
                Status = true
            };
        }

        public async Task<RoleResponseModel> GetRoleByName(string name)
        {
            var role = await _roleRepository.GetAsync(r => r.Name == name);
            if (role == null)
            {
                throw new NotFoundException($"\"{name}\" Role does not exist ");
            }

            return new RoleResponseModel
            {
                Data = new RoleDTO
                {
                    Id = role.Id,
                    Name = role.Name,
                    Description = role.Description
                },
                Message = $"Role Retrieved Successfully",
                Status = true

            };
        }

        public Task<RolesResponseModel> GetRoles()
        {
            throw new NotImplementedException();
        }
    }
}
