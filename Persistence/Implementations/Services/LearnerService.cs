using Domain.DTOs;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Interfaces.Identity;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.Integrations.Email;
using Persistence.Integrations.MailKitModels;
using System;
using System.Linq;
using System.Threading.Tasks;
using static Domain.Models.LearnerViewModel;

namespace Persistence.Implementations.Services
{
    public class LearnerService : ILearnerService
    {
        //private readonly IPasswordHasher<User> _passwordHasher;
        private readonly ILearnerRepository _learnerRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMailService _mailService;
       //private readonly UserManager<User> _userManager;
        private readonly IIdentityService _identityService;
        private readonly IRoleRepository _roleRepository;
        

        public LearnerService(ILearnerRepository learnerRepository, IUserRepository userRepository, /*UserManager<User> userManager,*/ IIdentityService identityService, IRoleRepository roleRepository, IMailService mailService)
        {
            _learnerRepository = learnerRepository;
            _userRepository = userRepository;
            _mailService = mailService;
           // _passwordHasher = passwordHasher;
            //_userManager = userManager;
            _identityService = identityService;
            _roleRepository = roleRepository;
        }
        public async Task<BaseResponse> AddLearner(CreateLearnerRequestModel model)
        {
            var usa = await _userRepository.GetAsync(a => a.Email == model.Email);
            var userExist = await _userRepository.ExistsAsync(a => a.Email == model.Email);

            if (userExist)
            {
                throw new BadRequestException($"{model.FirstName} {model.LastName} already exist and cannot be added");
            }

            var learnerExist = await _learnerRepository.ExistsAsync(a => a.Email == model.Email);

            if (learnerExist)
            {
                throw new BadRequestException($"{model.FirstName} {model.LastName} already exist and cannot be added");
            }
            var salt = Guid.NewGuid().ToString();
            var user = new User
            {
                Id = Guid.NewGuid(),
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                UserType = UserType.Learner,
                HashSalt = salt,
            };
            //var password = $"ULMS{Guid.NewGuid().ToString().Substring(1, 6).ToUpper()}";
            var password = "password";
            user.PasswordHash = _identityService.GetPasswordHash(password, salt);
            //user.PasswordHash = password;
            var learner = new Learner
            {
                Id = Guid.NewGuid(),
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                LearnerPhoto = model.LearnerPhoto,
                PhoneNumber = model.PhoneNumber,
                LearnerLMSCode = $"ULMS{Guid.NewGuid().ToString().ToUpper().Substring(1, 5)}L",
                UserId = user.Id,
                User = user
                
            };
            var role = await _roleRepository.GetAsync(r => r.Name == "learner");
            var userRole = new UserRole
            {
                Id = Guid.NewGuid(),
                Role = role,
                RoleId = role.Id,
                User = user,
                UserId = user.Id
            };

            user.UserRoles.Add(userRole);
            await _learnerRepository.AddAsync(learner);
            await _userRepository.AddAsync(user);         
            await _learnerRepository.SaveChangesAsync();
            await _userRepository.SaveChangesAsync();

            var request = new WelcomeMail
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Password = password,
                ToEmail = user.Email
            };

            await _mailService.SendWelcomeEmailAsync(request);
            return new BaseResponse
            {
                Status = true,
                Message = $"{model.FirstName} {model.LastName} added successfully. Check your mail for login details"
            };
        }

        public async Task<BaseResponse> DeleteLearner(Guid id)
        {
            var learnerExist = await _learnerRepository.ExistsAsync(id);
            if (!learnerExist)
            {
                throw new BadRequestException($"Learner with id {id} does not exist");
            }

            var learner = await _learnerRepository.GetAsync(id);
            await _learnerRepository.DeleteAsync(learner);
            await _learnerRepository.SaveChangesAsync();

            return new BaseResponse
            {
                Status = true,
                Message = $"{learner.FirstName} {learner.LastName} deleted successfully"
            };
        }

        public async Task<LearnersResponseModel> GetAllLearners()
        {
            var learners = await _learnerRepository.Query()
                .Include(u => u.LearnerCourses)
                .ThenInclude(a => a.Course)
               .Select(n => new LearnerDTO
               {
                   Id = n.Id,
                   FirstName = n.FirstName,
                   LastName = n.LastName,
                   Email = n.Email,
                   LearnerPhoto = n.LearnerPhoto,
                   LearnerLMSCode = n.LearnerLMSCode,
                   PhoneNumber = n.PhoneNumber,
                   LearnerCourses = n.LearnerCourses.Select(o => new CourseDTO
                   {
                       Id = o.Id,
                       Name = o.Course.Name,
                       CategoryId = o.Course.CategoryId,
                       CategoryName = o.Course.Category.Name,
                       Description = o.Course.Description,
                       AvailabilityStatus = o.Course.AvailabilityStatus
                   }).ToList()

               }).ToListAsync();

            if (learners == null)
            {
                throw new BadRequestException($"Learners not found");
            }
            else if (learners.Count == 0)
            {
                return new LearnersResponseModel
                {                   
                    Message = $" No Learner Found",
                    Status = true
                };
            }
            return new LearnersResponseModel
            {
                Data = learners,
                Message = $" {learners.Count} Learners retrieved successfully",
                Status = true
            };
        }

        public async Task<LearnerResponseModel> GetLearnerByEmail(string email)
        {
            var learner = await _learnerRepository.Query()
                .Include(u => u.LearnerCourses)
                .ThenInclude(a => a.Course)
                .SingleOrDefaultAsync(a => a.Email == email);
            if(learner == null)
            {
                throw new BadRequestException($"Learner with eamil {email} does not exist");
            }

            return new LearnerResponseModel
            {
                Data = new LearnerDTO
                {
                    Id = learner.Id,
                    FirstName = learner.FirstName,
                    LastName = learner.LastName,
                    Email = learner.Email,
                    LearnerPhoto = learner.LearnerPhoto,
                    LearnerLMSCode = learner.LearnerLMSCode,
                    PhoneNumber = learner.PhoneNumber,
                    LearnerCourses = learner.LearnerCourses.Select(o => new CourseDTO
                    {
                        Id = o.Id,
                        Name = o.Course.Name,
                        CategoryId = o.Course.CategoryId,
                        CategoryName = o.Course.Category.Name,
                        Description = o.Course.Description,
                        AvailabilityStatus = o.Course.AvailabilityStatus
                    }).ToList()
                },
                Message = $"Learner retrieved successfully",
                Status = true
            };
        }

        public async Task<LearnerResponseModel> GetLearnerById(Guid id)
        {
            var learner = await _learnerRepository.Query()
                .Include(u => u.LearnerCourses)
                .ThenInclude(a => a.Course)
                .ThenInclude(d => d.Category)
                .SingleOrDefaultAsync(a => a.Id == id);

            if (learner == null)
            {
                throw new BadRequestException($"Learner with id {id} does not exist");
            }
            return new LearnerResponseModel
            {
                Data = new LearnerDTO
                {
                    Id = learner.Id,
                    FirstName = learner.FirstName,
                    LastName = learner.LastName,
                    Email = learner.Email,
                    LearnerPhoto = learner.LearnerPhoto,
                    LearnerLMSCode = learner.LearnerLMSCode,
                    PhoneNumber = learner.PhoneNumber,
                    LearnerCourses = learner.LearnerCourses.Select(o => new CourseDTO
                    {
                        Id = o.Id,
                        Name = o.Course.Name,
                        CategoryId = o.Course.CategoryId,
                        CategoryName = o.Course.Category.Name,
                        Description = o.Course.Description,
                        AvailabilityStatus = o.Course.AvailabilityStatus
                        
                    }).ToList()
                },
                Message = $"Learner retrieved successfully",
                Status = true
            };
        }

        public async Task<LearnersResponseModel> GetLearnersByCourse(Guid courseId)
        {
            var learners = await _learnerRepository.GetLearnersByCourse(courseId);

            if (learners == null)
            {
                throw new BadRequestException($"Learners not found");
            }
            else if (learners.Count == 0)
            {
                return new LearnersResponseModel
                {
                  
                    Message = $" No Learner Found",
                    Status = true
                };
            }


            var learnersReturned = learners.Select(learner => new LearnerDTO
            {
                Id = learner.Id,
                FirstName = learner.FirstName,
                LastName = learner.LastName,
                Email = learner.Email,
                LearnerPhoto = learner.LearnerPhoto,
                PhoneNumber = learner.PhoneNumber,
                LearnerLMSCode = learner.LearnerLMSCode,
                LearnerCourses = learner.LearnerCourses.Select(o => new CourseDTO
                {
                    Id = o.Id,
                    Name = o.Course.Name,
                    CategoryId = o.Course.CategoryId,
                    CategoryName = o.Course.Category.Name,
                    Description = o.Course.Description,
                    AvailabilityStatus = o.Course.AvailabilityStatus
                }).ToList()
            });
            return new LearnersResponseModel
            {
                Data = learnersReturned,
                Message = $"{learners.Count} Learners offering course with id {courseId} retrieved successfully",
                Status = true
            };
        }

        public async Task<LearnersResponseModel> SearchLearnersByName(string searchText)
        {
            var learners = await _learnerRepository.SearchLearnersByName(searchText);

            if (learners == null)
            {
                throw new BadRequestException($"Learners not found");
            }

            var learnersReturned = learners.Select(learner => new LearnerDTO
            {
                Id = learner.Id,
                FirstName = learner.FirstName,
                LastName = learner.LastName,
                Email = learner.Email,
                LearnerPhoto = learner.LearnerPhoto,
                PhoneNumber = learner.PhoneNumber,
                LearnerLMSCode = learner.LearnerLMSCode,
                LearnerCourses = learner.LearnerCourses.Select(o => new CourseDTO
                {
                    Id = o.Id,
                    Name = o.Course.Name,
                    CategoryId = o.Course.CategoryId,
                    CategoryName = o.Course.Category.Name,
                    Description = o.Course.Description,
                    AvailabilityStatus = o.Course.AvailabilityStatus
                }).ToList()
            });
            return new LearnersResponseModel
            {
                Data = learnersReturned,
                Message = $"{learners.Count()} Learners retrieved successfully",
                Status = true
            };
       
        }

        public async Task<BaseResponse> UpdateLearner(Guid id, UpdateLearnerRequestModel model)
        {
            var learner = await _learnerRepository.GetAsync(id);
            if (learner == null)
            {
                throw new NotFoundException($"Learner with id {id} not found");
            }

            learner.PhoneNumber = model.PhoneNumber;
            learner.LearnerPhoto = model.LearnerPhoto;
            learner.Modified = DateTime.UtcNow;
            await _learnerRepository.UpdateAsync(learner);
            await _learnerRepository.SaveChangesAsync();

            return new BaseResponse
            {
                Message = $"{learner.FirstName} {learner.LastName} updated successfully",
                Status = true
            };
        }
    }
}
