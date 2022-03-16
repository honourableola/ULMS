using Domain.DTOs;
using System.Collections.Generic;

namespace Domain.Models
{
    public class CategoryViewModel
    {
        public class CreateCategoryRequestModel
        {
            public string Name { get; set; }

        }

        public class UpdateCategoryRequestModel
        {
            public string Name { get; set; }
        }
        public class CategoriesResponseModel : BaseResponse
        {
            public IEnumerable<CategoryDTO> Data { get; set; } = new List<CategoryDTO>();
        }

        public class CategoryResponseModel : BaseResponse
        {
            public CategoryDTO Data { get; set; }
        }
    }
}
