using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Domain.Models.CategoryViewModel;

namespace Domain.Interfaces.Services
{
    public interface ICategoryService
    {
        public Task<BaseResponse> AddCategory(CreateCategoryRequestModel model);
        public Task<BaseResponse> UpdateCategory(Guid id, UpdateCategoryRequestModel model);
        public Task<BaseResponse> DeleteCategory(Guid id);
        public Task<CategoryResponseModel> GetCategory(Guid id);
        public Task<CategoriesResponseModel> GetAllCategories();

    }
}
