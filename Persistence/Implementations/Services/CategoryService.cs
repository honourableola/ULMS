using Domain.DTOs;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Domain.Models.CategoryViewModel;

namespace Persistence.Implementations.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public async Task<BaseResponse> AddCategory(CreateCategoryRequestModel model)
        {
            var categoryExist = await _categoryRepository.ExistsAsync(a => a.Name == model.Name);

            if (categoryExist)
            {
                throw new BadRequestException($"{model.Name} already exist and cannot be added");
            }

            var category = new Category
            {
                Id = Guid.NewGuid(),
                Name = model.Name,
                Created = DateTime.UtcNow

            };

            await _categoryRepository.AddAsync(category);
            await _categoryRepository.SaveChangesAsync();

            return new BaseResponse
            {
                Status = true,
                Message = $"{model.Name} added successfully"
            };
        }

        public async Task<BaseResponse> DeleteCategory(Guid id)
        {
            var categoryExist = await _categoryRepository.ExistsAsync(id);
            if (!categoryExist)
            {
                throw new BadRequestException($"Category with id {id} does not exist");
            }

            var category = await _categoryRepository.GetAsync(id);
            await _categoryRepository.DeleteAsync(category);
            await _categoryRepository.SaveChangesAsync();

            return new BaseResponse
            {
                Status = true,
                Message = $"{category.Name} deleted successfully"
            };
        }

        public async Task<CategoriesResponseModel> GetAllCategories()
        {
            var categories = await _categoryRepository.Query()
               .Select(n => new CategoryDTO
               {
                   Id = n.Id,
                   Name = n.Name,
                   Courses = n.Courses.Select(o => new CourseDTO
                   {
                       Id = o.Id,
                       Name = o.Name,
                       CategoryId = o.CategoryId,
                       CategoryName = o.Category.Name,
                       Description = o.Description,
                       AvailabilityStatus = o.AvailabilityStatus
                   }).ToList()
               }).ToListAsync();

            if (categories == null)
            {
                throw new BadRequestException($"Categories not found");
            }
            else if (categories.Count == 0)
            {
                return new CategoriesResponseModel
                {
                   
                    Message = $" No Category Found",
                    Status = true
                };
            }


            return new CategoriesResponseModel
            {
                Data = categories,
                Message = $" {categories.Count} Categories retrieved successfully",
                Status = true
            };
        }

        public async Task<CategoryResponseModel> GetCategory(Guid id)
        {
            var category = await _categoryRepository.Query()
                .SingleOrDefaultAsync(a => a.Id == id);

            if(category == null)
            {
                throw new BadRequestException($"Category with id {id} does not exist");
            }

            return new CategoryResponseModel
            {
                Data = new CategoryDTO
                {
                    Id = category.Id,
                    Name = category.Name,
                    Courses = category.Courses.Select(o => new CourseDTO
                    {
                        Id = o.Id,
                        Name = o.Name,
                        CategoryId = o.CategoryId,
                        CategoryName = o.Category.Name,
                        Description = o.Description,
                        AvailabilityStatus = o.AvailabilityStatus
                    }).ToList()
                },
                Message = $"Category retrieved successfully",
                Status = true
            };
         
        }

        public async Task<BaseResponse> UpdateCategory(Guid id, UpdateCategoryRequestModel model)
        {
            var category = await _categoryRepository.GetAsync(id);
            if (category == null)
            {
                throw new NotFoundException($"Category with id {id} not found");
            }

            category.Name = model.Name;
            category.Modified = DateTime.UtcNow;
            await _categoryRepository.UpdateAsync(category);
            await _categoryRepository.SaveChangesAsync();

            return new BaseResponse
            {
                Message = $"{category.Name} updated successfully",
                Status = true
            };
        }
    }
}
