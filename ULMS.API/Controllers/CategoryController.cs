using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using static Domain.Models.CategoryViewModel;

namespace ULMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [Route("AddCategory")]
        [HttpPost]
        public async Task<IActionResult> AddCategory([FromBody] CreateCategoryRequestModel model)
        {
            var response = await _categoryService.AddCategory(model);
            return Ok(response);
        }

        [Route("Delete/{id}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteCategory([FromRoute] Guid id)
        {
            var response = await _categoryService.DeleteCategory(id);
            return Ok(response);
        }

        [Route("UpdateCategory/{id}")]
        [HttpPut]
        public async Task<IActionResult> UpdateCategory([FromRoute] Guid id, [FromBody] UpdateCategoryRequestModel model)
        {
            var response = await _categoryService.UpdateCategory(id, model);
            return Ok(response);
        }

        [Route("GetCategoryById/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetCategoryById([FromRoute] Guid id)
        {
            var response = await _categoryService.GetCategory(id);
            return Ok(response);
        }

        [Route("GetAllCategories")]
        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var response = await _categoryService.GetAllCategories();
            return Ok(response);
        }

        [Route("GetCategories")]
        [HttpPost]
        public async Task<IActionResult> GetCategories()
        {
            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault().ToLower();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                var categories = await _categoryService.GetAllCategories();
                var categoryData = categories.Data;
                /*if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    instructorData = instructorData.OrderBy(sortColumn + " " + sortColumnDirection);
                }*/
                if (!string.IsNullOrEmpty(searchValue))
                {
                    categoryData = categoryData.Where(m => m.Name.ToLower().Contains(searchValue));
                                                
                }
                recordsTotal = categoryData.Count();
                var data = categoryData.Skip(skip).Take(pageSize).ToList();
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


    }
}
