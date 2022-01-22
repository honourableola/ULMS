using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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

        
    }
}
