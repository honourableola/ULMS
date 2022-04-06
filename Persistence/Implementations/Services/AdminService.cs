using Domain.DTOs;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Interfaces.Identity;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Persistence.Integrations.Email;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static Domain.Models.AdminViewModel;

namespace Persistence.Implementations.Services
{
    public class AdminService : IAdminService
    {
        //private readonly IIdentityService _identityService;
        private readonly IAdminRepository _adminRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;

        public AdminService(IAdminRepository adminRepository, IUserRepository userRepository, /*IIdentityService identityService,*/ IRoleRepository roleRepository)//, IMailSender mailSender)
        {
            _adminRepository = adminRepository;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            //_identityService = identityService;
            //_mailSender = mailSender;
        }

        public async Task<BaseResponse> AddAdmin(CreateAdminRequestModel model/*, IFormFile file*/)
        {
            /*string adminImage = "";
            if (file != null)
            {
                var path = "C:\\Users\\OWNER\\source\\repos\\ULMS\\src\\Persistence\\Images\\";
                string imageDirectory = Path.Combine(path, "AdminImages");
                Directory.CreateDirectory(imageDirectory);
                string contentType = file.ContentType.Split('/')[1];
                adminImage = $"{Guid.NewGuid()}.{contentType}";
                string fullPath = Path.Combine(imageDirectory, adminImage);
                using (var fileStream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
            }*/
                var userExist = await _userRepository.ExistsAsync(u => u.Email.ToLower() == model.Email.ToLower());
            if (userExist)
            {
                throw new BadRequestException($"User with email {model.Email} already exist");
            }
            var adminExist = await _adminRepository.ExistsAsync(a => a.Email.ToLower() == model.Email.ToLower());
            if (adminExist)
            {
                throw new BadRequestException($"Admin already exist");
            }

            var salt = Guid.NewGuid().ToString();
            var user = new User
            {
                Id = Guid.NewGuid(),
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                UserType = UserType.Admin,
                HashSalt = salt
            };
            //var password = Guid.NewGuid().ToString().Substring(1, 6);
            var password = "password";
            //var passwordHash = _identityService.GetPasswordHash(password, salt);
            user.PasswordHash = password;

            var admin = new Admin
            {
                Id = Guid.NewGuid(),
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                AdminPhoto = model.Image,
                UserId = user.Id,
                User = user
            };

            var role = await _roleRepository.GetAsync(r => r.Name == "admin");
            var userRole = new UserRole
            {
                Id = Guid.NewGuid(),
                User = user,
                UserId = user.Id,
                Role = role,
                RoleId = role.Id
            };

            user.UserRoles.Add(userRole);

            await _adminRepository.AddAsync(admin);
            await _userRepository.AddAsync(user);
            await _adminRepository.SaveChangesAsync();
            await _userRepository.SaveChangesAsync();

            //await _mailSender.SendWelcomeMail(user.Email, $"{user.FirstName} {user.LastName}", password);
            return new BaseResponse
            {
                Message = $"Admin added successfully",
                Status = true
            };
        }

        public async Task<BaseResponse> DeleteAdmin(Guid id)
        {
            var adminExist = await _adminRepository.ExistsAsync(id);
            if (!adminExist)
            {
                throw new BadRequestException($"Admin with id {id} does not exist");
            }

            var admin = await _adminRepository.GetAsync(id);
            await _adminRepository.DeleteAsync(admin);
            await _adminRepository.SaveChangesAsync();

            return new BaseResponse
            {
                Status = true,
                Message = $"{admin.FirstName} {admin.LastName} deleted successfully"
            };
        }

        public async Task<AdminResponseModel> GetAdmin(Guid id)
        {
            var admin = await _adminRepository.Query()
                .SingleOrDefaultAsync(a => a.Id == id);

            if (admin == null)
            {
                throw new BadRequestException($"Admin with id {id} does not exist");
            }
            return new AdminResponseModel
            {
                Data = new AdminDTO
                {
                    Id = admin.Id,
                    FirstName = admin.FirstName,
                    LastName = admin.LastName,
                    Email = admin.Email,
                    AdminPhoto = admin.AdminPhoto,
                    PhoneNumber = admin.PhoneNumber                   
                },
                Message = $"Admin retrieved successfully",
                Status = true
            };
        }

        public async Task<AdminsResponseModel> GetAllAdmins()
        {
            var admins = await _adminRepository.Query()
                 .Select(a => new AdminDTO
                 {
                     Id = a.Id,
                     FirstName = a.FirstName,
                     LastName = a.LastName,
                     Email = a.Email,
                     AdminPhoto = a.AdminPhoto,
                     PhoneNumber = a.PhoneNumber
                 }).ToListAsync();

            if (admins == null)
            {
                throw new BadRequestException($"No Admins found");
            }
            else if (admins.Count == 0)
            {
                return new AdminsResponseModel
                {
                    Message = $"No Admin found",
                    Status = true
                };
            }

                return new AdminsResponseModel
            {
                Data = admins,
                Message = $"Administrators retrieved successfully",
                Status = true
            };
        }

        public async Task<BaseResponse> UpdateAdmin(Guid id, UpdateAdminRequestModel model)
        {
            var admin = await _adminRepository.GetAsync(id);
            if (admin == null)
            {
                throw new NotFoundException($"Admin with id {id} does not exist");
            }

            admin.PhoneNumber = model.PhoneNumber;
            admin.AdminPhoto = model.AdminPhoto;
            admin.Modified = DateTime.Now;
            await _adminRepository.UpdateAsync(admin);
            await _adminRepository.SaveChangesAsync();

            return new BaseResponse
            {
                Message = $"Admin {admin.FirstName} {admin.LastName} updated successfully",
                Status = true
            };
        }

        public async Task<BaseResponse> UploadPhoto(Guid adminId, UpdateAdminPhotoRequestModel model)
        {
            var admin = await _adminRepository.GetAsync(adminId);
            if (admin == null)
            {
                throw new NotFoundException($"Admin with id {adminId} does not exist");
            }


            admin.AdminPhoto = model.AdminPhoto;
            admin.Modified = DateTime.Now;
            await _adminRepository.UpdateAsync(admin);
            await _adminRepository.SaveChangesAsync();

            return new BaseResponse
            {
                Message = $"Admin {admin.FirstName} {admin.LastName} picture uploaded successfully",
                Status = true
            };
        }
    }
}
